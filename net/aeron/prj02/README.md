Aeron Example Projects
----------------------
Examples from https://aeroncookbook.com

Build and Run
-------------
```shell
./gradlew run
```

Run as jar
----------
```shell
./gradlew shadowJar
java --add-opens=java.base/sun.nio.ch=ALL-UNNAMED -jar app/build/libs/app-all.jar
```
