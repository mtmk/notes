# .Net 6 gRPC Server Testing

In this example I demonstrate how a basic project structure can be arranged for a gRPC server
written in .Net 6. Server is based on the Greeter example comes with the standard template.

As for testing, I am starting the web server on an ephemeral port which let's the system
deal with the port allocation so that we don't get issues while running the tests due to
ports being used. When running tests I then use the servers URL to connect and run all my tests
using a real gRPC client over the wire (well, not strictly wire but localhost).

**Test performance considerations:**

* Starting a server and talking over the wire obviously adds a lot of overhead compared to
  calling methods directly.
* You can reduce server startup times by placing the server startup call in a higher level
  location in test life-cycle e.g. fixture or even tests assembly level. You then have to manage
  state carefully to avoid false positives because of dirty state in your application
* If you're running thousands of tests and starting the server on every tests case you might
  run out of available ports on the host.
