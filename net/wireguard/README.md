# Point to point Wireguard Setup

[Wireguard](https://www.wireguard.com) is the best secure network tunneling protocol and product known to man.
In this document I describe a point to point installation of Wireguard using static private IPs.
Based on this you can create multiple peers and create a simple VPN to access your machines anywhere on the network
as long as you can access a UDP port of the peers.

## Linux
```shell
# apt install wireguard
# vi /etc/wireguard/wg0.conf
[Interface]
PrivateKey = bAkb/KF.....
Address = 10.0.0.1/16
ListenPort = 51280

[Peer]
PublicKey = 1kbPS.....
AllowedIPs = 10.0.0.2/32
# systemctl enable wg-quick@wg0.service
# systemctl daemon-reload
# systemctl start wg-quick@wg0.service
```

## Windows / Mac
- Install package for your system as described in [Wireguard installation document](https://www.wireguard.com/install/).
- Configure your client:
```ini
[Interface]
PrivateKey = BR2Qa.....
Address = 10.0.0.2/16

[Peer]
PublicKey = B6Kry.....
AllowedIPs = 10.0.0.1/32
Endpoint = <IP-address-of-Linux-box-above>:51280
```

### Note to poor souls behind corporate proxies
Extract `wireguard.exe` and place it under e.g. `c:\wg`
```
# Edit c:\wg\wg0.conf as above
> wireguard.exe /intalltunnelservice "c:\wg\wg0.conf"
```
