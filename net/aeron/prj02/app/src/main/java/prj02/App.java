package prj02;

import prj02.archive.ArchiveSamples;
//import prj02.mdcpub.MdcApp;
//import prj02.ipcagents.StartHere;

public class App {

    public static void main(String[] args) {
        System.out.println("Starting [Java version " + System.getProperty("java.version") + "]");

        // new StartHere().start();

        // new MdcApp().start(args);

        new ArchiveSamples().start(args);
    }

}
