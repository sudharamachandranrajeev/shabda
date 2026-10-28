# Restarts Windows Audio Endpoint Builder and Windows Audio. Needs admin.
# Restarting AudioEndpointBuilder also restarts Audiosrv, which depends on it.
Restart-Service -Name AudioEndpointBuilder -Force
Start-Service -Name Audiosrv
