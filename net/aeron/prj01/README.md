Aeron Simple Example
--------------------
Example taken from [Aeron Cookbook](https://aeroncookbook.com/aeron/basic-sample/)

How to run
----------
* Install java 17
* `./gradlew run`

How to run as executable jar
----------------------------
* Install Java 17
* `./gradlew shadowJar`
* `java --add-opens=java.base/sun.nio.ch=ALL-UNNAMED -jar app/build/libs/app-all.jar`

How to create project from scratch
----------------------------------
* gradle init (select: app, java)
* Edit app/build.gradle
  * Search for `aeron-all` in [Maven](https://mvnrepository.com)
  * Add e.g. `implementation 'io.aeron:aeron-all:1.38.1'` to `dependencies`
  * Add `applicationDefaultJvmArgs = ["--add-opens", "java.base/sun.nio.ch=ALL-UNNAMED"]` to `application`
  * Add Java version required:
```groovy
java {
  toolchain {
    languageVersion = JavaLanguageVersion.of(17)
  }
}
```
  * Shadow / executable jar config:
  * Add `id 'com.github.johnrengelman.shadow' version '7.1.2'` to plugins
  * Add manifest:
```groovy
tasks.jar {
    manifest.attributes["Main-Class"] = "prj01.App"
}
```
