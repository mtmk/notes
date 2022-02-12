# Examples of How to Use The GNU Privacy Guard

[GnuPG] is Free Software. It helps to secure bunch of things but I will be
focusing on the bits I use rather than trying to cover everything. In this
document I list [GnuPG] features I find useful for my day-to-day work; **signing
git commits and tags**, keeping ansible vault passwords (essentially **data
encryption**) and using gpg-agent over **ssh tunnel** as well as a few
examples of **key management** as I use `gpg` in my day-to-day work.

## Quick Background

There are only two areas you need to be familiar with to able to use GnuPG:
asymmetric cryptography (or aka Public-key cryptography) and how to use a
shell on a unix-like system. You don't need to be an expert. Just find out
what a key-pair is (public and private keys - if you use ssh or dealt with TLS
you already know) and as for unix shell or command-line, if you can run a few
commands, know how to pass options or arguments and simple pipe to files etc.
would do just fine.

[GnuPG] is an implementation of [OpenPGP] standard which very roughly describes
a security system using public-private key pairs where users can establish authenticity
of public key owners by creating so called a web of trust (this document does not cover web of trust).

## Installation

[GnuPG] installation and `gpg --version` used in these examples (as of February 2022):

* macOS Big Sur (Version 11.6.2)
```
$ brew install gpg
$ gpg --version
gpg (GnuPG) 2.3.4
libgcrypt 1.10.0
```

* Debian GNU/Linux 11 (bullseye):
```
$ sudo apt install gpg
$ gpg --version
gpg (GnuPG) 2.2.27
libgcrypt 1.8.8
```

Also make sure to have `pinentry` installed. If you are using terminal
i.e. `pinentry-curses` you must set GPG_TTY in your shell profile:
```sh 
export GPG_TTY=$(tty)
```

## Key Management

* Creating Secret Key
```sh
# Only fill in email and real name
gpg --gen-key

# ...with more options
gpg --full-generate-key
```

* Listing keys
```sh
# List public, private or specific keys
gpg --list-keys
gpg --list-secret-keys
gpg --list-key me@example.com

# Various places might need your short key id
gpg --list-keys --keyid-format=short
gpg --list-secret-keys --keyid-format=long
```

* Point to a key server in `~/.gnupg/gpg.conf`
```conf
keyserver keyserver.ubuntu.com
```

* Send your key to keyserver
```sh
$ gpg --send-key BA5EBA11
gpg: sending key 5CA1AB1EBA5EBA11 to hkp://keyserver.ubuntu.com

# You can also search the keyservers
$ gpg --search-keys <email-or-name-of-your-friend>
```

* Use verbose mode with any command:
```
gpg -v
gpg -vv
gpg -vvv
```

* Encrypt & decrypt
```sh
echo "Secret test" | gpg --armor --encrypt --recipient me@example.com > secret.txt
gpg --decrypt < secret.txt
```

* [How can I restart gpg-agent]
```sh
gpgconf --kill gpg-agent
gpg-connect-agent reloadagent /bye
```

* Import & export
```sh
# Export public keys
gpg --armor --export 4AEE18F83AFDEB23

# Import from GitHub users
curl https://github.com/username.gpg | gpg --import

# Refresh or import keys from keyserver
gpg --refresh-keys
gpg --recv-keys 4AEE18F83AFDEB23
```

* [Remove keys from your public keyring]
```sh
gpg --delete-keys 
```

* Backup your private key
```sh
gpg --export-secret-keys --armor
gpg --export-secret-keys --armor me@example.com > me@example.com.gpg
```

* [Display GPG key details without importing]
```sh
gpg --list-packets me@example.com.gpg
gpg --import-options=show-only --import me@example.com.gpg
```

* [What is GitHub's public GPG key?]: You can import the
  [GPG key GitHub uses for merges]
```sh
$ curl https://github.com/web-flow.gpg | gpg --import
$ gpg --edit-key noreply@github.com
gpg> trust
gpg> save

# You can also sign GitHubs key
$ gpg --lsign-key noreply@github.com

# Now git log can show the signature as github
git log --show-signature
```

## Signing Git Commits and Tags

You can use flags or change your `~/.gitconfig` to sign everything by default:
```ini
[user]
        email = me@example.com
        name = Mia Els
        # Key id seems to be optional if you only have one key
        # signingkey = 5CA1AB1EBA5EBA11
[commit]
        gpgsign = true
[tag]
        gpgsign = true
```

See commits with signatures:
```sh
git log --show-signature
```

## SSH Forwarding

[Forwarding gpg-agent to a remote system over SSH] can be done by adding a few lines
of `~/.ssh/config` entries:
```conf
Host debian
    Hostname debian.example.com
    # 'S.gpg-agent.extra' didn't work for me. This is supposed to be the 'more secure' setup
    # so use 'S.gpg-agent' at your own risk. 
    # RemoteForward /run/user/1000/gnupg/S.gpg-agent /Users/me/.gnupg/S.gpg-agent.extra
    RemoteForward /run/user/1000/gnupg/S.gpg-agent /Users/me/.gnupg/S.gpg-agent
    StreamLocalBindUnlink yes
    ForwardAgent yes
```
Check your paths:
```sh
gpgconf --list-dir agent-socket
```

On Linux server side make sure to call `gpg` without running a local agent:
```
gpg --batch --use-agent --no-autostart --decrypt secret.txt
```

## Using with Ansible Vault

You can [encrypt Ansible Vault passphrase using GPG]:

```sh
# Generate a strong passphrase
$ pwgen -n 71 -C | head -n1 | gpg --armor --recipient me@example.com -e -o vault_passphrase.gpg

# See what it is
$ gpg --batch --use-agent --decrypt vault_passphrase.gpg
```

* `vault_pass.sh`
```sh
#!/bin/sh
gpg --batch --use-agent --decrypt vault_passphrase.gpg
```

* `ansible.cfg`
```ini
[defaults]
vault_password_file=vault_pass.sh
```

## Compiling GnuPG

If you're on an older version of `gpg` on your system than you're connecting to over `ssh`
(e.g. Ubuntu 20.04.3 LTS to Debian GNU/Linux 11 bullseye), you'd get an error about
older version of `gpg-agent`:
```
gpg: WARNING: server 'gpg-agent' is older than us (2.2.19 < 2.2.27)
gpg: Note: Outdated servers may lack important security fixes.
gpg: Note: Use the command "gpgconf --kill all" to restart them.
```
You can remove the system gpg:
```sh
sudo apt remove libgpgmepp6
sudo apt remove gpg
sudo apt remove gpg-agent
sudo apt autoremove
```
You can [download and compile the latest GnuPG]:
```sh
cd ~/Downloads
version=gnupg-2.2.34 # Update to latest
wget https://gnupg.org/ftp/gcrypt/gnupg/$version.tar.bz2
wget https://gnupg.org/ftp/gcrypt/gnupg/$version.tar.bz2.sig
tar xf $version.tar.bz2
cd $version
sudo apt-get update
sudo apt-get install -y libldap2-dev
sudo apt-get install -y gtk+-2
sudo apt-get install -y rng-tools
sudo apt-get install -y libbz2-dev
sudo apt-get install -y zlib1g-dev
sudo apt-get install -y libgmp-dev
sudo apt-get install -y nettle-dev
sudo apt-get install -y libgnutls28-dev
sudo apt-get install -y libsqlite3-dev
sudo apt-get install -y adns-tools
sudo apt-get install -y libreadline-dev
sudo apt-get install -y pinentry-gtk2
sudo apt-get install -y pcscd scdaemon
sudo make -f build-aux/speedo.mk native INSTALL_PREFIX=/usr/local
sudo ldconfig
```


[GnuPG]: https://gnupg.org

[OpenPGP]: https://datatracker.ietf.org/wg/openpgp/about/

[What is GitHub's public GPG key?]: https://stackoverflow.com/questions/60482588/what-is-githubs-public-gpg-key/60482908#60482908

[GPG key GitHub uses for merges]: https://github.community/t/where-can-i-find-the-gpg-key-github-uses-for-merges/563/10

[Display GPG key details without importing]: https://stackoverflow.com/questions/22136029/how-to-display-gpg-key-details-without-importing-it/22147722#22147722

[Remove keys from your public keyring]: https://blog.chapagain.com.np/gpg-remove-keys-from-your-public-keyring/

[How can I restart gpg-agent]: https://superuser.com/questions/1075404/how-can-i-restart-gpg-agent

[Forwarding gpg-agent to a remote system over SSH]: https://wiki.gnupg.org/AgentForwarding

[encrypt Ansible Vault passphrase using GPG]: https://disjoint.ca/til/2016/12/14/encrypting-the-ansible-vault-passphrase-using-gpg/

[download and compile the latest GnuPG]: https://askubuntu.com/questions/681041/trying-to-compile-gnupg-from-source/681085#681085
