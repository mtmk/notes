package main

import "github.com/nats-io/nats.go"

func main() {
	nc, _ := nats.Connect(nats.DefaultURL)
	nc.Publish("foo", []byte("Hello World"))

	//s, _ := nc.SubscribeSync("foo")
	//s.Drain()

	sub, _ := nc.Subscribe("foo", func(msg *nats.Msg) {})
	sub.AutoUnsubscribe(1)

	nc.Drain()
	nc.Close()
}
