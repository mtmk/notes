cd hi
sh build.sh
cd ..
# docker create mcr.microsoft.com/dotnet/runtime-deps
rm -rf rootfs
mkdir rootfs
sudo docker export $DOTNET_RUNTIME_DEPS_CONTAINER_ID | tar xf - -C rootfs
mv hi/hi rootfs
sudo runc run d1

