#include <iostream>
#include "template_example.h"

int main() {
    std::cout << "Hello, World!" << std::endl;

    template_example::say_hi();

    auto ri = template_example::add(1, 2);
    auto rs = template_example::add<std::string>("one", "two");

    std::cout << "ri=" << ri << std::endl;
    std::cout << "rs=" << rs << std::endl;

    return 0;
}
