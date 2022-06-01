Maven Java Project Template
---------------------------

* Jave 17
* Maven 3.8.5

Create project: (see also https://maven.apache.org/guides/getting-started/maven-in-five-minutes.html)

```shell
mvn archetype:generate -DgroupId=com.mycompany.app -DartifactId=my-app -DarchetypeArtifactId=maven-archetype-quickstart -DarchetypeVersion=1.4 -DinteractiveMode=false
```

* Dependencies:
   * Search Maven for packages: e.g. https://mvnrepository.com/search?q=json
   * Add dependency into `pom.xml` e.g.:
```xml
<!-- https://mvnrepository.com/artifact/org.json/json -->
<dependency>
    <groupId>org.json</groupId>
    <artifactId>json</artifactId>
    <version>20220320</version>
</dependency>
```

* Executable JAR (Shade plugin)
  * https://maven.apache.org/plugins/maven-shade-plugin/examples/executable-jar.html
  * Also filtering: https://maven.apache.org/plugins/maven-shade-plugin/examples/includes-excludes.html
  * Add the plugin with main class:
```xml
<project>
  <!-- ... -->
  <build>
    <pluginManagement>
    <!-- ... -->
    </pluginManagement>

    <!-- NOTE plugin location: NOT under 'pluginManagement' -->
    <plugins>
      <plugin>
        <groupId>org.apache.maven.plugins</groupId>
        <artifactId>maven-shade-plugin</artifactId>
        <version>3.3.0</version>
        <executions>
          <execution>
            <phase>package</phase>
            <goals>
              <goal>shade</goal>
            </goals>
            <configuration>
              <transformers>
                <transformer implementation="org.apache.maven.plugins.shade.resource.ManifestResourceTransformer">
                  <mainClass>com.mycompany.app.App</mainClass>
                </transformer>
              </transformers>
              <filters>
                <filter>
                  <artifact>org.json:json</artifact>
                  <excludes>
                    <exclude>META-INF/MANIFEST.MF</exclude>
                  </excludes>
                </filter>
              </filters>
            </configuration>
          </execution>
        </executions>
      </plugin>
    </plugins>
  </build>
</project>
```

Build and Run
-------------
```shell
rm -rf target
mvn clean package
java -jar target/my-app-1.0-SNAPSHOT.jar
```
