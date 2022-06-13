package prj02.archive;

import io.aeron.Aeron;
import io.aeron.ChannelUri;
import io.aeron.ExclusivePublication;
import io.aeron.Subscription;
import io.aeron.archive.Archive;
import io.aeron.archive.ArchivingMediaDriver;
import io.aeron.archive.client.AeronArchive;
import io.aeron.archive.client.RecordingDescriptorConsumer;
import io.aeron.archive.codecs.SourceLocation;
import io.aeron.archive.status.RecordingPos;
import io.aeron.driver.MediaDriver;
import io.aeron.logbuffer.Header;
import org.agrona.CloseHelper;
import org.agrona.DirectBuffer;
import org.agrona.ExpandableArrayBuffer;
import org.agrona.collections.MutableLong;
import org.agrona.concurrent.IdleStrategy;
import org.agrona.concurrent.SleepingIdleStrategy;
import org.agrona.concurrent.status.CountersReader;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.File;

public class SimplestCase {
    private static final Logger LOGGER = LoggerFactory.getLogger(SimplestCase.class);

    private final File tmp = Utils.createTempDir();
    private Aeron aeron;
    private ArchivingMediaDriver driver;
    private AeronArchive archive;
    private final String channel = "aeron:ipc";
    private final int streamCapture = 16;
    private final int streamReplay = 17;
    int sendCount = 100;
    IdleStrategy idle = new SleepingIdleStrategy();
    ExpandableArrayBuffer buffer = new ExpandableArrayBuffer();

    boolean complete = false;

    public void start() {
        setup();

        LOGGER.info("Archive dir: {}", tmp);

        LOGGER.info("writing...");
        write();

        LOGGER.info("reading...");
        read();

        cleanUp();
    }

    private void cleanUp() {
        CloseHelper.quietClose(archive);
        CloseHelper.quietClose(aeron);
        CloseHelper.quietClose(driver);
    }

    private void read() {

        try (Aeron aeron1 = Aeron.connect();
             AeronArchive reader = AeronArchive.connect(new AeronArchive.Context().aeron(aeron1))) {

            final long recordingId = findLatestRecording(reader, channel, streamCapture);
            final long position = 0L;
            final long length = Long.MAX_VALUE;

            final long sessionId = reader.startReplay(recordingId, position, length, channel, streamReplay);
            final String channelRead = ChannelUri.addSessionId(channel, (int) sessionId);

            final Subscription subscription = reader.context().aeron().addSubscription(channelRead, streamReplay);

            while (!subscription.isConnected())
                idle.idle();

            while (!complete) {
                int fragments = subscription.poll(this::archiveReader, 1);
                idle.idle(fragments);
            }
        }
    }

    public void archiveReader(DirectBuffer buffer, int offset, int length, Header header) {
        final int valueRead = buffer.getInt(offset);
        LOGGER.info("Received {}", valueRead);
        if (valueRead == sendCount) {
            complete = true;
        }
    }

    private long findLatestRecording(final AeronArchive archive, String channel, int stream) {
        final MutableLong lastRecordingId = new MutableLong();

        final RecordingDescriptorConsumer consumer =
                (controlSessionId, correlationId, recordingId,
                 startTimestamp, stopTimestamp, startPosition,
                 stopPosition, initialTermId, segmentFileLength,
                 termBufferLength, mtuLength, sessionId,
                 streamId, strippedChannel, originalChannel,
                 sourceIdentity) -> lastRecordingId.set(recordingId);

        final long fromRecordingId = 0L;
        final int recordCount = 100;

        final int foundCount = archive.listRecordingsForUri(fromRecordingId, recordCount, channel, stream, consumer);

        if (foundCount == 0) {
            throw new IllegalStateException("no recordings found");
        }

        return lastRecordingId.get();
    }

    private void write() {
        archive.startRecording(channel, streamCapture, SourceLocation.LOCAL);

        try (ExclusivePublication publication = aeron.addExclusivePublication(channel, streamCapture)) {
            while (!publication.isConnected())
                idle.idle();

            for (int i = 0; i <= sendCount; i++) {
                buffer.putInt(0, i);
                while (publication.offer(buffer, 0, Integer.BYTES) < 0)
                    idle.idle();
            }

            long position = publication.position();
            int sessionId = publication.sessionId();
            CountersReader countersReader = aeron.countersReader();

            int counterId;
            do {
                counterId = RecordingPos.findCounterIdBySession(countersReader, sessionId);
                idle.idle();
            } while (counterId < 0);

            while (countersReader.getCounterValue(counterId) < position)
                idle.idle();
        }
    }


    public void setup() {
        MediaDriver.Context contextDriver = new MediaDriver.Context()
                .spiesSimulateConnection(true)
                .dirDeleteOnStart(true);

        Archive.Context contextArchive = new Archive.Context()
                .deleteArchiveOnStart(true)
                .archiveDir(tmp);

        driver = ArchivingMediaDriver.launch(contextDriver, contextArchive);

        aeron = Aeron.connect();

        AeronArchive.Context ctx = new AeronArchive.Context()
                .aeron(aeron);

        archive = AeronArchive.connect(ctx);
    }
}
