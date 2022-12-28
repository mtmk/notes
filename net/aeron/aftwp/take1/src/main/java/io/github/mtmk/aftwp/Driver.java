package io.github.mtmk.aftwp;

import io.aeron.driver.MediaDriver;

import java.io.Closeable;
import java.io.IOException;
import java.nio.file.Path;
import java.nio.file.Paths;

public class Driver implements Closeable {
    private MediaDriver driver;
    private final String driver_path;

    public Driver(String path) {
        driver_path = Paths.get(path).toAbsolutePath().toString();
    }

    public void run() {
        MediaDriver.Context context = new MediaDriver.Context()
                .dirDeleteOnStart(true)
                .aeronDirectoryName(driver_path);
        driver = MediaDriver.launch(context);
    }

    @Override
    public void close() throws IOException {
        driver.close();
    }
}
