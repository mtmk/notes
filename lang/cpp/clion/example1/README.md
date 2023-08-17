# CLion Simple Setup

Settings > Build Exec Deploy > Toolchains > Visual Studio (default)

Using vcpkg:
* Set *vcpkg* root
* Using the *vcpkg* window add remove libs

Refresh *cmake* and all libs and dependencies in `vcpkg.json` are downloaded!

All dependencies are kept under `.\cmake-build-debug\vcpkg_installed` folder.

(vcpkg might take a while downloading and building libraries and their dependencies)
