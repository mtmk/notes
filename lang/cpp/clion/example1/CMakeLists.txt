cmake_minimum_required(VERSION 3.26)
project(example1)

set(CMAKE_CXX_STANDARD 17)

find_package(fmt CONFIG REQUIRED)

target_link_libraries(main PRIVATE fmt::fmt)

add_executable(example1 main.cpp)
