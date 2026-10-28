# no-output research log (2026-10-28)

Category: `no-output`. Existing entries: none (new category).

## Issues covered

| id | summary | fixes |
|----|---------|-------|
| `noout-no-device-installed` | Red X / "No audio output device is installed" / "No output devices found" | troubleshooter, enable disabled device, restart services, reinstall device, generic HD Audio driver, roll back driver |
| `noout-intel-sst-code10` | Intel SST Audio Controller Code 10, no output after update | switch to High Definition Audio Controller, install OEM audio driver |
| `noout-silent-device-present` | Icon normal, meters move, nothing heard | pick correct output / mute / app volume, disable enhancements, restart services |

## Sources consulted (fetched)

| url | type | signal |
|-----|------|--------|
| https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8 | microsoft | Official page, Win10/11; troubleshooter, output device, enhancements, enable device, drivers (update/uninstall/generic/rollback), default format, services |
| https://support.microsoft.com/en-us/windows/fix-missing-or-undetected-audio-output-device-in-windows-5504aed3-2c01-4214-89d1-9e8dbe6828e8 | microsoft | Official page: enable device, show hidden devices, scan for hardware changes, OEM driver, troubleshooter |
| https://learn.microsoft.com/en-us/answers/a/6690896 | microsoft | Community Support Specialist answer, 100+ helpful votes, "asked 800+ times"; not marked accepted; mmsys.cpl > Show Disabled Devices > Enable |
| https://techcommunity.microsoft.com/t5/windows-10/reinstate-audio-function-without-reinstalling-windows/m-p/2445398 | microsoft | Reply marked as solution (8-step guide incl. services Automatic, uninstall devices); no OP confirmation visible |
| https://techcommunity.microsoft.com/discussions/windowsinsiderprogram/windows-11-sound-problem/2539526 | microsoft | Thread marked Solved (best answer: KB5005188, 2021, obsolete). Individual user replies, 0 likes each: generic driver, service reset, Realtek rollback, disable enhancements |
| https://www.intel.com/content/www/us/en/support/articles/000058422.html | vendor | Intel: SST Code 10 no audio -> switch to High Definition Audio Controller |
| https://www.intel.com/content/www/us/en/support/articles/000030990.html | vendor | Intel: "no audio output device is installed" after Win10 update -> uninstall, pick audio driver from list; get Realtek driver from OEM |
| https://www.dell.com/community/Latitude/Intel-R-Smart-Sound-Technology-Intel-R-SST-OED-error-Code-10-on/td-p/7709610 | forum | Dell Community, Solved; OP: "This solved the issue on both devices" (HD Audio Controller); accepted solution + 2 users confirm Dell Realtek driver |
| https://www.dell.com/support/kbdoc/en-in/000179019/how-to-fix-persistent-audio-issues-on-your-dell-computer | vendor | Dell KB: restart, power drain, drivers, BIOS/chipset, output device/app volume, Waves antivirus exclusion, troubleshooter, restore |
| https://learn.microsoft.com/en-us/answers/questions/5671043/no-audio-output-device-is-installed | microsoft | No accepted answer; top answer 3 helpful votes (BIOS onboard audio, show disabled devices) - used for discovery only |
| https://learn.microsoft.com/en-us/answers/questions/5915521/no-sound-settings-say-no-output-devices-found | microsoft | No accepted answer; follow-up "tried all of the above. No joy" - discovery only |

Weak-confirmation note: `disable-enhancements`, `roll-back-driver`, and the second source of `restart-audio-services` (silent issue) rest on the Microsoft official page plus individual Tech Community user reports with 0 likes. They meet the "microsoft + one more" rule but a refresh should look for stronger second sources (TenForums/ElevenForums threads were found but could not be fetched).

## Single-source, not included

- Change Default Format (sample rate/bit depth) in device Properties > Advanced - Microsoft support page only.
- Power drain / hard reset (unplug, hold power 15-20 s) - Dell KB only (HP forum and blog mentions not fetched).
- Enable onboard audio in BIOS/UEFI - Microsoft Q&A answer with 3 votes, not accepted.
- Antivirus blocking Waves MaxxAudio service (WavesSysSvc64.exe) - Dell KB only.
- Reset/check Realtek Audio Control "sound effects" (Asus Sonic) - single Tech Community reply.
- HDAudBus registry Start value edit (Dell Inspiron thread) - not fetched, registry edit, excluded.
- Add Legacy Hardware generic audio driver - blog-only.
- SFC /scannow - TenForums search snippet only.

## Gaps

- superuser.com is blocked for the search/fetch tools; no Stack Exchange answers verified.
- tenforums.com, elevenforum.com, h30434.www3.hp.com (HP Community), support.lenovo.com returned HTTP 403; their threads were seen only as search snippets and are not cited. Lenovo HT501860 ("Fix audio or sound problems in Windows 10 and 11") should be re-checked as a vendor source.
- No Reddit thread with visible upvotes/confirmations found for this category.
- KB4468550 (2018 Intel SST 9.21.0.3755 driver on Win10 1709/1803/1809) is historical; not added as an issue.
- `enable-disabled-device` command enables only Device Manager devices with problem code 22. Endpoints disabled in mmsys.cpl are not PnP-disabled and need the manual steps; app maintainer should confirm detection of disabled endpoints.
- Overlap: audio-service restarts also belong to `services-update`; driver rollback/generic driver overlap `driver-realtek`. Merge step should dedupe.
- The MSDT-based Playing Audio troubleshooter is being retired on newer Windows 11 builds; Get Help path used instead, command left null.
