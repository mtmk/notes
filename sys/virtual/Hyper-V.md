### Run pwsh terminal as admin:
```
> Get-VM
```

### Enable for KVM:
```
> Stop-VM -Name ubuntu
> Set-VMProcessor -VMName ubuntu -ExposeVirtualizationExtensions $True
> Start-VM -Name ubuntu
```
