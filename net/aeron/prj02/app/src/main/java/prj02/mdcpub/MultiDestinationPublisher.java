package prj02.mdcpub;

import org.agrona.CloseHelper;
import org.agrona.concurrent.AgentRunner;
import org.agrona.concurrent.ShutdownSignalBarrier;
import org.agrona.concurrent.SleepingMillisIdleStrategy;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

public class MultiDestinationPublisher
{
    private static final Logger LOGGER = LoggerFactory.getLogger(MultiDestinationPublisher.class);

    public static void start()
    {
        // Multicast:
        // export MDC_PUB_AERON_URI='aeron:udp?endpoint=224.1.1.1:8000'

        // MDC:
        // export MDC_PUB_AERON_URI='aeron:udp?control-mode=dynamic|control=10.2.1.1:8000'
        // host publisher is on is 10.2.1.1

        final var uri = System.getenv().get("MDC_PUB_AERON_URI");

        if (uri == null)
        {
            LOGGER.error("requires 2 env vars: MDC_PUB_AERON_URI");
        }
        else
        {
            final var barrier = new ShutdownSignalBarrier();
            final var hostAgent = new MultiDestinationPublisherAgent(uri);
            final var runner =
                    new AgentRunner(new SleepingMillisIdleStrategy(), MultiDestinationPublisher::errorHandler,
                            null, hostAgent);

            AgentRunner.startOnThread(runner);

            barrier.await();

            CloseHelper.quietClose(runner);
        }
    }

    private static void errorHandler(Throwable throwable)
    {
        LOGGER.error("agent failure {}", throwable.getMessage(), throwable);
    }
}
