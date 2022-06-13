package prj02.mdcpub;

import io.aeron.logbuffer.FragmentHandler;
import io.aeron.logbuffer.Header;
import org.agrona.DirectBuffer;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class MultiDestinationSubscriberFragmentHandler implements FragmentHandler
{
    private static final Logger LOGGER = LoggerFactory.getLogger(MultiDestinationSubscriberFragmentHandler.class);

    @Override
    public void onFragment(DirectBuffer buffer, int offset, int length, Header header)
    {
        final var read = buffer.getLong(offset);

        LOGGER.info("received {}", read);
    }
}
