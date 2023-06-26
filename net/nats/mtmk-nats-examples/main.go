package main

import "github.com/nats-io/nats.go"

func main() {
	nc, _ := nats.Connect(nats.DefaultURL)
	nc.Publish("foo", []byte("Hello World"))

	nc.Drain()
	nc.Close()
}
