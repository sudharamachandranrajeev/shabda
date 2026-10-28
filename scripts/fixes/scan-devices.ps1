# Same as Device Manager > Action > Scan for hardware changes. Needs admin.
pnputil.exe /scan-devices
if ($LASTEXITCODE -ne 0) { throw "pnputil exited with $LASTEXITCODE" }
