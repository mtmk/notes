rm -rf rootfs
mkdir rootfs
cc hi.c -o rootfs/hi -static
sudo runc run hi1
rm -rf rootfs
