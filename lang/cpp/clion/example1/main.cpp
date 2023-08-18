#include <iostream>
#include <fmt/core.h>
#include <nats/nats.h>

int main() {
    std::cout << "Hello, World!" << std::endl;
    fmt::print("Hi!\n");

    natsStatus s;
    natsOptions *opts = nullptr;
    natsConnection *nc = nullptr;

    const char *servers[] = {"localhost:4222" };

    s = natsOptions_Create(&opts);
    if (s != NATS_OK)
    {
        std::cout << "Can't create options" << std::endl;
        return 1;
    }

    s = natsOptions_SetServers(opts, servers, 1);
    if (s != NATS_OK)
    {
        std::cout << "Can't set servers" << std::endl;
        return 1;
    }

    fmt::print("Connecting to NATS!\n");

    s = natsConnection_Connect(&nc, opts);
    if (s != NATS_OK)
    {
        std::cout << "Can't connect" << std::endl;
        return 1;
    }

    fmt::print("Publishing to foo!\n");
    s = natsConnection_Publish(nc, "foo", (const void*)"hello", 5);
    if (s != NATS_OK)
    {
        std::cout << "Can't publish" << std::endl;
        return 1;
    }

    natsConnection_Drain(nc);
    natsConnection_Destroy(nc);
    natsOptions_Destroy(opts);
    nats_Close();

    return 0;
}
