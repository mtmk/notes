
```sh
# build
maven package

# server
java -Dorg.slf4j.simpleLogger.defaultLogLevel=debug --add-opens=java.base/sun.nio.ch=ALL-UNNAMED -jar target/take1-1.0-SNAPSHOT.jar server ~/tmp/take1 127.0.0.1 4321


# client
java -Dorg.slf4j.simpleLogger.defaultLogLevel=debug --add-opens=java.base/sun.nio.ch=ALL-UNNAMED -jar target/take1-1.0-SNAPSHOT.jar client ~/tmp/take1 127.0.0.1 4322 127.0.0.1 4321
```

