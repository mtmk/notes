# Initializers, Pointers, Arrays and References (and a little CMake)

```
// primatives
double pi1 = 3.14; // initialize pi1 to 3.14
double pi2 {3.14}; // same thing

// other types
vector<int> vec = {1,2,3,4}; // fill vector
vector<int> vec {1,2,3,4};   // same, '=' is redundant
```

## CMake

(See also [cmake tutorial](https://cmake.org/cmake/help/latest/guide/tutorial/A%20Basic%20Starting%20Point.html#build-and-run))

A minimal CMakeLists.txt
```
cmake_minimum_required(VERSION 3.10)
project(prj03 VERSION 1.0)
set(CMAKE_CXX_STANDARD 11)
add_executable(prj03 main.cpp)
```
