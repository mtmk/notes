# iptables

https://fabianlee.org/2019/06/05/ubuntu-debug-iptables-by-inserting-a-log-rule/
```sh
sudo iptables -I FORWARD 4 -j LOG --log-prefix "RULE4:" --log-level 7

# delete rule at position 4
sudo iptables -D FORWARD 4
```

# Container Networking / Network Namespace

https://iximiuz.com/en/posts/container-networking-is-simple/
