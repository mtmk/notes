c++ hi.cpp -o hicpp -static
sudo docker image rm hicpp
sudo docker build -t hicpp .
rm hicpp
sudo docker image ls hicpp
sudo docker run --rm hicpp

