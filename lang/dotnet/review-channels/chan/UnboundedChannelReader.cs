using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace review.channels.chan;


public sealed class UnboundedChannelReader<T>// : ChannelReader<T>//, IDebugEnumerable<T>
{
    /**************************************/
    /// <summary>Creates an <see cref="IAsyncEnumerable{T}"/> that enables reading all of the data from the channel.</summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use to cancel the enumeration.</param>
    /// <remarks>
    /// Each <see cref="IAsyncEnumerator{T}.MoveNextAsync"/> call that returns <c>true</c> will read the next item out of the channel.
    /// <see cref="IAsyncEnumerator{T}.MoveNextAsync"/> will return false once no more data is or will ever be available to read.
    /// </remarks>
    /// <returns>The created async enumerable.</returns>
    public async IAsyncEnumerable<T> ReadAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (await WaitToReadAsync(cancellationToken).ConfigureAwait(false))
        {
            while (TryRead(out T? item))
            {
                yield return item;
            }
        }
    }
    /**************************************/
        
    internal readonly UnboundedChannel<T> _parent;
    private readonly AsyncOperation<T> _readerSingleton;
    private readonly AsyncOperation<bool> _waiterSingleton;

    internal UnboundedChannelReader(UnboundedChannel<T> parent)
    {
        _parent = parent;
        _readerSingleton = new AsyncOperation<T>(parent._runContinuationsAsynchronously, pooled: true);
        _waiterSingleton = new AsyncOperation<bool>(parent._runContinuationsAsynchronously, pooled: true);
    }

    public /*override*/ Task Completion => _parent._completion.Task;

    public /*override*/ bool CanCount => true;

    public /*override*/ bool CanPeek => true;

    public /*override*/ int Count => _parent._items.Count;

    public /*override*/ ValueTask<T> ReadAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return new ValueTask<T>(Task.FromCanceled<T>(cancellationToken));
        }

        // Dequeue an item if we can.
        UnboundedChannel<T> parent = _parent;
        if (parent._items.TryDequeue(out T? item))
        {
            CompleteIfDone(parent);
            return new ValueTask<T>(item);
        }

        lock (parent.SyncObj)
        {
            parent.AssertInvariants();

            // Try to dequeue again, now that we hold the lock.
            if (parent._items.TryDequeue(out item))
            {
                CompleteIfDone(parent);
                return new ValueTask<T>(item);
            }

            // There are no items, so if we're done writing, fail.
            if (parent._doneWriting != null)
            {
                return ChannelUtilities.GetInvalidCompletionValueTask<T>(parent._doneWriting);
            }

            // If we're able to use the singleton reader, do so.
            if (!cancellationToken.CanBeCanceled)
            {
                AsyncOperation<T> singleton = _readerSingleton;
                if (singleton.TryOwnAndReset())
                {
                    parent._blockedReaders.EnqueueTail(singleton);
                    return singleton.ValueTaskOfT;
                }
            }

            // Otherwise, create and queue a reader.
            var reader = new AsyncOperation<T>(parent._runContinuationsAsynchronously, cancellationToken);
            parent._blockedReaders.EnqueueTail(reader);
            return reader.ValueTaskOfT;
        }
    }

    public /*override*/ bool TryRead([MaybeNullWhen(false)] out T item)
    {
        X.Log();
        UnboundedChannel<T> parent = _parent;

        // Dequeue an item if we can
        if (parent._items.TryDequeue(out item))
        {
            X.Log("dequeue");
            CompleteIfDone(parent);
            return true;
        }

        X.Log("default");
        item = default;
        return false;
    }

    public /*override*/ bool TryPeek([MaybeNullWhen(false)] out T item) =>
        _parent._items.TryPeek(out item);

    private static void CompleteIfDone(UnboundedChannel<T> parent)
    {
        if (parent._doneWriting != null && parent._items.IsEmpty)
        {
            // If we've now emptied the items queue and we're not getting any more, complete.
            ChannelUtilities.Complete(parent._completion, parent._doneWriting);
        }
    }

    public /*override*/ ValueTask<bool> WaitToReadAsync(CancellationToken cancellationToken)
    {
        X.Log();
        if (cancellationToken.IsCancellationRequested)
        {
            return new ValueTask<bool>(Task.FromCanceled<bool>(cancellationToken));
        }

        if (!_parent._items.IsEmpty)
        {
            X.Log("not empty");
            return new ValueTask<bool>(true);
        }

        UnboundedChannel<T> parent = _parent;

        lock (parent.SyncObj)
        {
            parent.AssertInvariants();

            // Try again to read now that we're synchronized with writers.
            if (!parent._items.IsEmpty)
            {
                return new ValueTask<bool>(true);
            }

            // There are no items, so if we're done writing, there's never going to be data available.
            if (parent._doneWriting != null)
            {
                return parent._doneWriting != ChannelUtilities.s_doneWritingSentinel ?
                    new ValueTask<bool>(Task.FromException<bool>(parent._doneWriting)) :
                    default;
            }

            // If we're able to use the singleton waiter, do so.
            if (!cancellationToken.CanBeCanceled)
            {
                AsyncOperation<bool> singleton = _waiterSingleton;
                if (singleton.TryOwnAndReset())
                {
                    ChannelUtilities.QueueWaiter(ref parent._waitingReadersTail, singleton);
                    return singleton.ValueTaskOfT;
                }
            }

            // Otherwise, create and queue a waiter.
            var waiter = new AsyncOperation<bool>(parent._runContinuationsAsynchronously, cancellationToken);
            ChannelUtilities.QueueWaiter(ref parent._waitingReadersTail, waiter);
            return waiter.ValueTaskOfT;
        }
    }

    /// <summary>Gets an enumerator the debugger can use to show the contents of the channel.</summary>
    // IEnumerator<T> IDebugEnumerable<T>.GetEnumerator() => _parent._items.GetEnumerator();
}