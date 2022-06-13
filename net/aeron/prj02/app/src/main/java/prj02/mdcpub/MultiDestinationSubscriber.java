package prj02.mdcpub;

import org.agrona.CloseHelper;
import org.agrona.concurrent.AgentRunner;
import org.agrona.concurrent.ShutdownSignalBarrier;
import org.agrona.concurrent.SleepingMillisIdleStrategy;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class MultiDestinationSubscriber
{
    private static final Logger LOGGER = LoggerFactory.getLogger(MultiDestinationSubscriber.class);

    public static void start()
    {
        // Multicast:
        // export MDC_PUB_AERON_URI='aeron:udp?endpoint=224.1.1.1:8000'

        // MDC:
        // THIS HOST=10.2.1.3
        // MDC HOST=10.2.1.1
        // CONTROL PORT=8000
        // export MDC_PUB_AERON_URI='aeron:udp?endpoint=10.2.1.3:12001|control=10.2.1.1:8000|control-mode=dynamic'

        final var uri = System.getenv().get("MDC_PUB_AERON_URI");

        if (uri == null)
        {
            LOGGER.error("env vars required: MDC_PUB_AERON_URI");
        }
        else
        {
            final var barrier = new ShutdownSignalBarrier();
            final MultiDestinationSubscriberAgent hostAgent =
                    new MultiDestinationSubscriberAgent(uri);

            final var runner =
                    new AgentRunner(new SleepingMillisIdleStrategy(), MultiDestinationSubscriber::errorHandler,
                            null, hostAgent);
            AgentRunner.startOnThread(runner);

            barrier.await();

            CloseHelper.quietClose(runner);
        }
    }

    private static void errorHandler(Throwable throwable)
    {
        LOGGER.error("agent error {}", throwable.getMessage(), throwable);
    }
}
