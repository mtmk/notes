rm -rf rootfs hi
go build -ldflags=-extldflags=-static hi.go
mkdir rootfs
mv hi rootfs
sudo runc run g1
rm -rf rootfs hi
