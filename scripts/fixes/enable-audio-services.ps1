# Sets the two Windows audio services back to their default Automatic start
# type and starts them. Needs admin.
foreach ($name in 'AudioEndpointBuilder', 'Audiosrv') {
    Set-Service -Name $name -StartupType Automatic
    Start-Service -Name $name
}
