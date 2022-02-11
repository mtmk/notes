cc hi.c -o hi -static
sudo docker image rm hic
sudo docker build -t hic .
rm hi
sudo docker image ls hic
sudo docker run --rm hic
