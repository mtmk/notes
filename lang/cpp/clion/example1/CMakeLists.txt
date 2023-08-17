cmake_minimum_required(VERSION 3.26)
project(example1)

set(CMAKE_CXX_STANDARD 17)

add_executable(example1 main.cpp)

find_package(fmt CONFIG REQUIRED)
find_package(cnats CONFIG REQUIRED)

target_link_libraries(example1 PRIVATE fmt::fmt)
target_link_libraries(example1 PRIVATE cnats::nats)
