package io.github.mtmk.aftwp;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.Objects;

public class AppMain
{
    private static final Logger LOG = LoggerFactory.getLogger(AppMain.class);

    public static void main( String[] args ) throws Exception {
        System.out.println( "App version 1.0.1" );
        if (args.length == 0){
            System.out.println( "usage: <client|server>" );
            return;
        }
        if (Objects.equals(args[0], "driver")) {
            if (args.length < 2) {
                LOG.error("usage: driver <directory>");
                System.exit(1);
            }
            try(final Driver driver = new Driver(args[1])){
                LOG.error("Starting driver...");
                driver.run();
                System.in.read();
            }
        }
        if (Objects.equals(args[0], "client")) {
            if (args.length < 6) {
                LOG.error("usage: client <directory> <local-address> <local-port> <remote-address> <remote-port>");
                System.exit(1);
            }

            final Path directory = Paths.get(args[1]);
            final InetAddress local_name = InetAddress.getByName(args[2]);
            final Integer local_port = Integer.valueOf(args[3]);
            final InetAddress remote_name = InetAddress.getByName(args[4]);
            final Integer remote_port = Integer.valueOf(args[5]);

            final InetSocketAddress local_address =
                    new InetSocketAddress(local_name, local_port.intValue());
            final InetSocketAddress remote_address =
                    new InetSocketAddress(remote_name, remote_port.intValue());

            try (final EchoClient client = EchoClient.create(directory, local_address, remote_address)) {
                client.run();
            }
        }
        if (Objects.equals(args[0], "server")) {
            if (args.length < 4) {
                LOG.error("usage: server <directory> <local-address> <local-port>");
                System.exit(1);
            }

            final Path directory = Paths.get(args[1]);
            final InetAddress local_name = InetAddress.getByName(args[2]);
            final Integer local_port = Integer.valueOf(args[3]);

            final InetSocketAddress local_address =
                    new InetSocketAddress(local_name, local_port.intValue());

            try (final EchoServer server = EchoServer.create(directory, local_address)) {
                server.run();
            }
        }
    }
}
