# Shabda

**Shabda** (Sanskrit: *sound*) finds common Windows 10/11 sound problems on your PC and shows fixes that have been confirmed by more than one trusted source.

- **Scan** checks audio services, playback and recording devices, Bluetooth audio, driver errors and microphone privacy settings. Scanning only reads; it changes nothing.
- **Fixes** come from a researched catalog (`data/issues.json`). Every fix has at least two independent sources, a confidence tier and a risk label.
- **Safe to run.** Before a fix runs, Shabda shows you the exact PowerShell and asks to confirm. Fixes that touch drivers offer a System Restore point first. See [SECURITY.md](SECURITY.md).

Browse the full catalog without installing anything: [docs/2026-10-28-windows-audio-issues.md](docs/2026-10-28-windows-audio-issues.md).

## Use it

1. Download `Shabda-<version>-win-x64.exe` from [Releases](../../releases) and check it against `SHA256SUMS.txt`.
2. Run it and press **Scan this PC**.
3. Select a problem to see its causes and fixes, ordered from least to most invasive.
4. If the scan finds nothing but sound still fails, switch to **All known issues** and search your symptom.

The exe is not code-signed yet, so Windows SmartScreen may show "Windows protected your PC". Choose **More info > Run anyway** only if the hash matches.

### Without the app

```powershell
# Read-only scan (Windows PowerShell 5.1 or PowerShell 7)
powershell -ExecutionPolicy Bypass -File .\scripts\Diagnose.ps1
```

The fix scripts are in `scripts/fixes/`. Read one before running it from an elevated PowerShell.

## How fixes are chosen

The catalog is built by the `windows-audio-research` method in `research/`:

| Tier | Meaning |
|------|---------|
| `verified` | A Microsoft or hardware-vendor source, or an accepted high-vote answer, plus one more independent source |
| `strong` | At least two independent community sources where users confirm it worked |

Fixes with a single source are left out and listed in the research notes for the next refresh. `tools/merge_issues.py` enforces these rules in CI, and rejects any command that downloads or runs code from the internet. Maintainer edits (dropped entries, which scan check detects which issue, which fixes the app can run) live in `data/curation.json`.

**Known limits of the current catalog:** Super User, Reddit, TenForums and ElevenForum blocked the research tools in this round, so most evidence comes from Microsoft and vendor pages. Some popular community fixes are therefore missing until a refresh can verify them.

## Build

Requires the .NET 10 SDK.

```powershell
dotnet test
dotnet publish src/Shabda/Shabda.csproj -c Release -o publish
```

## Layout

| Path | What |
|------|------|
| `src/Shabda` | WPF app (.NET 10) |
| `tests/Shabda.Tests` | Unit tests |
| `scripts/Diagnose.ps1` | Read-only scan, JSON output with `-AsJson` |
| `scripts/fixes/` | Fix scripts the app can run |
| `data/issues.json` | Merged catalog, embedded in the app |
| `data/curation.json` | Maintainer edits applied during the merge |
| `research/<date>/` | Per-category research notes and source logs |
| `tools/merge_issues.py` | Validates and merges research into the catalog |

## Licence

MIT. This is not a Microsoft product. Fixes are provided as-is: create a restore point before driver or registry changes.
