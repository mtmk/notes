package prj02.archive;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import prj02.archive_multi.client.ArchiveClient;
import prj02.archive_multi.host.ArchiveHost;

import java.util.Objects;

public class ArchiveSamples {

    private static final Logger LOGGER = LoggerFactory.getLogger(ArchiveSamples.class);

    public void start(String[] args) {

        if (args.length != 1) {
            LOGGER.error("Expected one command: <simple|host|client>");
            return;
        }

        String cmd = args[0];

        if (Objects.equals(cmd, "simple")) {
            new SimplestCase().start();
        } else if (Objects.equals(cmd, "host")) {
            new ArchiveHost().start();
        } else if (Objects.equals(cmd, "client")) {
            new ArchiveClient().start();
        } else {
            LOGGER.error("Unknown command {}", cmd);
        }
    }

}
