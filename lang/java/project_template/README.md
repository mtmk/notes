Java Project Template
---------------------
This is an how to on creating a new java project. Asumptions are:
* Java 17
* Gradle
* Service or command line application

Instructions
------------
* Install Java 17
* Install Gradle 7.4

* `gradle init` then select:
   * application
   * Java
   * only one application project
   * Groovy
   * Generate build using new APIs and behavior: no
   * JUnit Jupiter

Edit `app/build.gradle`:

* Dependencies:
  * Search for libraries in [Maven](https://mvnrepository.com)
  * Add e.g. `implementation 'org.json:json:20220320'` to `dependencies`

* Add Java version required:
```groovy
java {
  toolchain {
    languageVersion = JavaLanguageVersion.of(17)
  }
}
```

* Add any JVM args for `./gradlew run`
  * E.g. add `applicationDefaultJvmArgs = ["--add-opens", "java.base/sun.nio.ch=ALL-UNNAMED"]` to `application`

* Shadow / executable jar config:
  * Add `id 'com.github.johnrengelman.shadow' version '7.1.2'` to plugins
  * Add manifest:
```groovy
tasks.jar {
    manifest.attributes["Main-Class"] = "project_template.App"
}
```

Build and Run
-------------
* Run using gradle: `./gradlew run`
* Build executable jar: `./gradlew shadowJar`
  * Run executable jar: `java -jar app/build/libs/app-all.jar`
