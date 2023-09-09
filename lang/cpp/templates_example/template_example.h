//
// Created by mtmk on 09/09/2023.
//
#ifndef TEMPLATES_EXAMPLE_TEMPLATE_EXAMPLE_H
#define TEMPLATES_EXAMPLE_TEMPLATE_EXAMPLE_H

#include <iostream>

namespace template_example
{
    void say_hi()
    {
        using namespace std;
        cout << "hi" << endl;
    }

    template<typename T>
    T add(T a1, T a2)
    {
        return a1 + a2;
    }
}

#endif //TEMPLATES_EXAMPLE_TEMPLATE_EXAMPLE_H
