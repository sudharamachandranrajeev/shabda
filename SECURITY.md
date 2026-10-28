# Security policy

Shabda runs PowerShell on your PC, some of it as administrator, so security reports matter.

## Reporting a vulnerability

Use GitHub's private vulnerability reporting: **Security > Report a vulnerability** on this repository. Please do not open a public issue for security problems.

Expect an acknowledgement within 7 days.

## How Shabda limits risk

- **Scanning is read-only.** `scripts/Diagnose.ps1` only reads services, devices, audio endpoints and privacy settings.
- **Fixes run only after you confirm.** The app shows the exact script before it runs and asks for administrator rights only when the fix needs them.
- **No code from the internet.** The fix catalog and all scripts are compiled into `Shabda.exe`. The app never downloads or runs anything else, and the catalog's `command` text is shown to you, never executed.
- **Allow-listed actions.** A fix can only run an embedded script from `scripts/fixes/` or open a Windows settings page (`ms-settings:` or `mmsys.cpl`). Anything else is rejected, both in CI and in the app.
- **Risky fixes are labelled.** Registry edits and driver removal are marked high risk and offer a System Restore point first.
- **Nothing is sent anywhere.** Scan results stay on your PC. Fix attempts are logged to `%LOCALAPPDATA%\Shabda\logs\fixes.log`.

## Verifying a download

Each release includes `SHA256SUMS.txt`. Check the exe before running it:

```powershell
Get-FileHash .\Shabda-<version>-win-x64.exe -Algorithm SHA256
```

The exe is not code-signed yet, so Windows SmartScreen may warn on first run.
