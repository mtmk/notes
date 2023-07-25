// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using System.Diagnostics;

namespace review.channels.chan;

public class UnboundedChannel<T>
{
    /// <summary>Task that indicates the channel has completed.</summary>
    internal readonly TaskCompletionSource _completion;
    /// <summary>The items in the channel.</summary>
    internal readonly ConcurrentQueue<T> _items = new ConcurrentQueue<T>();
    /// <summary>Readers blocked reading from the channel.</summary>
    internal readonly Deque<AsyncOperation<T>> _blockedReaders = new Deque<AsyncOperation<T>>();
    /// <summary>Whether to force continuations to be executed asynchronously from producer writes.</summary>
    internal readonly bool _runContinuationsAsynchronously;

    /// <summary>Readers waiting for a notification that data is available.</summary>
    internal AsyncOperation<bool>? _waitingReadersTail;
    /// <summary>Set to non-null once Complete has been called.</summary>
    internal Exception? _doneWriting;

    /// <summary>Initialize the channel.</summary>
    public UnboundedChannel(bool runContinuationsAsynchronously)
    {
        _runContinuationsAsynchronously = runContinuationsAsynchronously;
        _completion = new TaskCompletionSource(runContinuationsAsynchronously ? TaskCreationOptions.RunContinuationsAsynchronously : TaskCreationOptions.None);
        Reader = new UnboundedChannelReader<T>(this);
        Writer = new UnboundedChannelWriter<T>(this);
    }

    public UnboundedChannelWriter<T> Writer { get; }

    public UnboundedChannelReader<T> Reader { get; }

    [DebuggerDisplay("Items={Count}")]
    // [DebuggerTypeProxy(typeof(DebugEnumeratorDebugView<>))]



    /// <summary>Gets the object used to synchronize access to all state on this instance.</summary>
    internal object SyncObj => _items;

    [Conditional("DEBUG")]
    internal void AssertInvariants()
    {
        Debug.Assert(SyncObj != null, "The sync obj must not be null.");
        Debug.Assert(Monitor.IsEntered(SyncObj), "Invariants can only be validated while holding the lock.");

        if (!_items.IsEmpty)
        {
            if (_runContinuationsAsynchronously)
            {
                Debug.Assert(_blockedReaders.IsEmpty, "There's data available, so there shouldn't be any blocked readers.");
                Debug.Assert(_waitingReadersTail == null, "There's data available, so there shouldn't be any waiting readers.");
            }
            Debug.Assert(!_completion.Task.IsCompleted, "We still have data available, so shouldn't be completed.");
        }
        if ((!_blockedReaders.IsEmpty || _waitingReadersTail != null) && _runContinuationsAsynchronously)
        {
            Debug.Assert(_items.IsEmpty, "There are blocked/waiting readers, so there shouldn't be any data available.");
        }
        if (_completion.Task.IsCompleted)
        {
            Debug.Assert(_doneWriting != null, "We're completed, so we must be done writing.");
        }
    }

    /// <summary>Gets the number of items in the channel.  This should only be used by the debugger.</summary>
    private int ItemsCountForDebugger => _items.Count;

    /// <summary>Report if the channel is closed or not. This should only be used by the debugger.</summary>
    private bool ChannelIsClosedForDebugger => _doneWriting != null;

    /// <summary>Gets an enumerator the debugger can use to show the contents of the channel.</summary>
    // IEnumerator<T> IDebugEnumerable<T>.GetEnumerator() => _items.GetEnumerator();
}