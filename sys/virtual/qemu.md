### Install

Install Ubuntu (22.04.2 LTS (Jammy Jellyfish)) on a VM with virtual extensions.

For Hyper-V:
```
> Set-VMProcessor -VMName ubuntu -ExposeVirtualizationExtensions $True
```

Check KVM:
```
$ sudo apt install cpu-checker
$ sudo kvm-ok
INFO: /dev/kvm exists
KVM acceleration can be used
```

Install QEMU:
```
$ sudo apt install qemu
$ sudo apt install qemu-system-x86
$ qemu-system-i386 -M help
$ qemu-system-x86_64 -device help
$ qemu-system-x86_64 -device isa-debugcon,help
```

See also [OpenSecurityTraining2: Debuggers 1016: Introductory QEMU](https://p.ost2.fyi/courses/course-v1:OpenSecurityTraining2+Dbg1016_2023v1+2023_v1/course/)

Start a machine:
```
$ qemu-system-x86_64 -nographic
(ctrl-a, h)
C-a h    print this help
C-a x    exit emulator
C-a s    save disk data back to file (if -snapshot)
C-a t    toggle console timestamps
C-a b    send break (magic sysrq)
C-a c    switch between console and monitor
```

Stop a machine:
```
(ctrl-a, h)
QEMU: Terminated
```

QEMU gdbstub:
```
$ qemu-system-x86_64 -nographic -s -S
(another terminal)
$ gdb
(gdb) target remote :1234
```
