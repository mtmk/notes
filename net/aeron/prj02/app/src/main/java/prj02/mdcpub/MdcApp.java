package prj02.mdcpub;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.Objects;

public class MdcApp {

    private static final Logger LOGGER = LoggerFactory.getLogger(MdcApp.class);

    /*

     You need three hosts:
     ---------------------
     1) Publisher on 10.2.1.1
     2) Subscriber 1 on 10.2.1.2
     3) Subscriber 2 on 10.2.1.3


     Build:
     ------
       $ gradle shadowJar
       $ scp app/build/libs/app-all.jar 10.2.1.1:
       $ scp app/build/libs/app-all.jar 10.2.1.2:
       $ scp app/build/libs/app-all.jar 10.2.1.3:


     Using multicast:
     ----------------
     Publisher:
      $ export MDC_PUB_AERON_URI='aeron:udp?endpoint=224.1.1.1:8000'
      $ java --add-opens=java.base/sun.nio.ch=ALL-UNNAMED -jar app-all.jar pub

     Subscriber 1 and 2:
      $ export MDC_PUB_AERON_URI='aeron:udp?endpoint=224.1.1.1:8000'
      $ java --add-opens=java.base/sun.nio.ch=ALL-UNNAMED -jar app-all.jar sub


     Using Multi-Destination-Cast:
     -----------------------------
     Publisher:
       $ export MDC_PUB_AERON_URI='aeron:udp?control-mode=dynamic|control=10.2.1.1:8000'
       $ java --add-opens=java.base/sun.nio.ch=ALL-UNNAMED -jar app-all.jar pub

     Subscriber 1:
       $ export MDC_PUB_AERON_URI='aeron:udp?endpoint=10.2.1.2:12001|control=10.2.1.1:8000|control-mode=dynamic'
       $ java --add-opens=java.base/sun.nio.ch=ALL-UNNAMED -jar app-all.jar sub

     Subscriber 2:
       $ export MDC_PUB_AERON_URI='aeron:udp?endpoint=10.2.1.3:12001|control=10.2.1.1:8000|control-mode=dynamic'
       $ java --add-opens=java.base/sun.nio.ch=ALL-UNNAMED -jar app-all.jar sub

     */
    public void start(String[] args) {

        if (args.length != 1) {
            LOGGER.error("No command found");
            return;
        }

        String cmd = args[0];

        LOGGER.info("Starting {}...", cmd);

        if (Objects.equals(cmd, "pub")) {
            MultiDestinationPublisher.start();
        } else if (Objects.equals(cmd, "sub")) {
            MultiDestinationSubscriber.start();
        } else {
            LOGGER.error("Invalid command: {}", cmd);
        }
    }
}
