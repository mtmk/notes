cd `dirname $0`      && \
mkdir -p build       && \
cd build             && \
cmake .. > /dev/null && \
make     > /dev/null && \
./prj03
