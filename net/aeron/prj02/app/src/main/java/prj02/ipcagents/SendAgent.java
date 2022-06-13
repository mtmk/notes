package prj02.ipcagents;

import io.aeron.Publication;
import org.agrona.concurrent.Agent;
import org.agrona.concurrent.UnsafeBuffer;

import java.nio.ByteBuffer;

public class SendAgent implements Agent {

    private Publication publication;
    private int sendCount;
    private final UnsafeBuffer buffer;
    private int count = 1;

    public SendAgent(final Publication publication, int sendCount) {
        this.publication = publication;
        this.sendCount = sendCount;
        this.buffer = new UnsafeBuffer(ByteBuffer.allocate(64));
        this.buffer.putInt(0, count);
    }

    @Override
    public int doWork() throws Exception {
        if (count > sendCount)
            return 0;

        if (publication.isConnected()) {
            if (publication.offer(buffer) > 0) {
                count++;
                buffer.putInt(0, count);
            }
        }

        return 0;
    }

    @Override
    public String roleName() {
        return "sender";
    }
}
