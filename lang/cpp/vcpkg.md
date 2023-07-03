# install on windows
https://youtu.be/0h1lC3QHLHU
```
> cd src
> git clone git@github.com:microsoft/vcpkg.git
> cd .\vcpkg\
> .\bootstrap-vcpkg.bat
> ./vcpkg integrate install
Applied user-wide integration for this vcpkg root.
CMake projects should use: "-DCMAKE_TOOLCHAIN_FILE=C:/Users/mtmk/src/vcpkg/scripts/buildsystems/vcpkg.cmake"

All MSBuild C++ projects can now #include any installed libraries. Linking will be handled automatically.
Installing new libraries will make them instantly available.
```
