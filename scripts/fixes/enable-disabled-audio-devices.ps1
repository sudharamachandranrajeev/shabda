# Enables sound devices that were disabled in Device Manager (problem code 22).
# Leaves devices with other errors alone. Needs admin.
$disabled = Get-PnpDevice -Class MEDIA -PresentOnly | Where-Object { $_.ConfigManagerErrorCode -eq 22 }
foreach ($device in $disabled) {
    Enable-PnpDevice -InstanceId $device.InstanceId -Confirm:$false
}
