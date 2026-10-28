# driver-realtek: source log (2026-10-28)

## Issues covered

1. `drv-audio-driver-broken-after-update`: Realtek/onboard audio Code 10 or "No audio output device is installed" after an update. Fixes: roll back, uninstall and reinstall, OEM package, generic High Definition Audio Device.
2. `drv-intel-sst-code10`: Intel SST Audio Controller / OED Code 10. Fixes: OEM matching package, switch controller to High Definition Audio Controller.
3. `drv-intel-sst-bsod-11thgen`: IntcAudioBus.sys 10.29.0.5152 / 10.30.0.5152 BSOD and Windows 11 upgrade hold. Fix: update to 5714 or later.

## Sources consulted

| URL | Type | Signal |
|-----|------|--------|
| https://support.microsoft.com/en-gb/windows/fix-audio-stops-working-after-a-windows-update-in-windows-b0dadfe1-051e-4e3f-a971-486ab4f7e6d6 | microsoft | Official: update driver, roll back driver, restart audio services |
| https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8 | microsoft | Official: step 9 uninstall/rollback/generic High Definition Audio Device (Win10 and Win11 wording) |
| https://support.microsoft.com/en-us/topic/error-codes-in-device-manager-in-windows-524e9e89-4dee-8883-0afa-6bca0456324e | microsoft | Official: Code 10 and Code 52 resolutions (update/download vendor driver) |
| https://learn.microsoft.com/en-us/windows/release-health/known-issues/known-issue-2746 | microsoft | Official known issue, Resolved External 2024-04-12; affected and fixed SST versions |
| https://learn.microsoft.com/en-us/answers/questions/2758897/how-to-enable-generic-audio-driver | microsoft | Q&A, not accepted; rollback and uninstall/scan steps. Asker: generic driver gave "No audio devices are installed"; asker's own fix was a registry edit to the Audiosrv ObjectName (not used) |
| https://techcommunity.microsoft.com/t5/windows-10/how-do-i-get-rid-of-realtek-audio-and-go-back-to-high-definition/m-p/4564780 | microsoft | Community reply with generic HDA steps; no confirmation |
| https://www.intel.com/content/www/us/en/support/articles/000058422.html | vendor | Intel: SST Code 10 after update, switch to High Definition Audio Controller |
| https://www.intel.com/content/www/us/en/support/articles/000089706.html | vendor | Intel: Code 10/31 after SST update, use OEM drivers |
| https://www.dell.com/community/Latitude/Intel-R-Smart-Sound-Technology-Intel-R-SST-OED-error-Code-10-on/m-p/7709628 | forum | Marked Solved; accepted reply (2 kudos) uninstall + Dell Realtek driver; OP and others confirm Realtek package and HDA Controller swap |
| https://learn.microsoft.com/en-us/answers/questions/af53cd5c-9b69-4f3d-9bf1-410de2f2e7e1/realtek-audio-console-missing-from-windows | microsoft | Q&A, no accepted answer; top answers 9/6/5 helpful votes |
| https://learn.microsoft.com/en-us/answers/questions/50ccdd96-9657-4e5f-b0f8-320bb6cf36a1/no-audio-from-front-panel-audio-jack | microsoft | Q&A, no accepted answer, no confirmation |
| https://learn.microsoft.com/en-us/answers/questions/5772323/realtek-r-audio-not-recognizing-headphone-jack | microsoft | Q&A, no accepted answer, no confirmation |
| https://forums.tomshardware.com/threads/realtek-front-panel-jack-not-working.245646/ | forum | 4+ users confirm "Disable front panel jack detection" worked; no best-answer marker |
| https://forums.tomshardware.com/threads/front-panel-audio-jack-not-working.2766024/ | forum | OP fixed an earlier PC with jack-detection toggle; option missing in ASUS build; unresolved |
| https://forums.tomshardware.com/threads/sound-and-microphone-no-longer-work-after-realtek-driver-update-dont-appear-in-sound-settings.3602464/ | forum | Solution marked (uninstall + reinstall Realtek) with 0 votes; one user fixed by uninstalling Intel SST controller |

Blocked (403/404/timeout): elevenforum.com threads, h30434.www3.hp.com threads, techpowerup.com, thewindowsclub.com, Dell KB 000197391, HP doc c05519670. superuser.com is not reachable by the search tool.

## Single-source, not included

- **Realtek Audio Console missing / won't install from Store** (`drv-realtek-console-missing`): install OEM DCH driver with its setup.exe (not Device Manager) so the Realtek Hardware Support Application (HSA) component is present, then the Store auto-installs the console; reset Store. Only Microsoft Q&A answers by JosephT71 (9/6/5 helpful votes, not accepted). ElevenForum thread 39380 blocked. Needs a vendor page or ElevenForum confirmation.
- **Front-panel jack not detected**: enable "Disable front panel jack detection" in Realtek Audio Console > Device advanced settings (or HD Audio Manager connector settings); alternatively set front panel to AC97 in BIOS/Realtek if the case uses an AC97 header. Confirmed only on forums.tomshardware.com (single host). Microsoft Q&A replies mention the setting without confirmation. Needs ElevenForum/TenForums or vendor (ASUS/MSI/Gigabyte) source.
- **Code 52 (digital signature)**: Microsoft error-code page says download latest driver from the manufacturer. No second confirmed source fetched. Community workarounds (disable driver signature enforcement, Secure Boot off, delete UpperFilters/LowerFilters) are high risk and unconfirmed; do not add without strong sources.
- **Conexant ISST / HDAudio (uci64a96.dll–uci64a231.dll < 7.231.3.0)**: Microsoft compatibility hold for Win10 2004/20H2, resolved 2021; fix = updated OEM driver. Microsoft release-health page for that issue was not fetched (version retired); HP doc c05519670 timed out. HP forum users report switching to Microsoft HD Audio driver or reinstalling Conexant ISST app (pages 403).
- **Roll back Intel SST Audio Controller**: one HP forum user reported instant fix (page 403, from search snippet only).

## Gaps

- No Reddit or Super User sources could be fetched; confidence rests on Microsoft, Intel and Dell community.
- Generic High Definition Audio Device fix: Microsoft official, but the community replies found were unconfirmed and one asker reported it failed. Marked verified per the evidence bar, but expect mixed results.
- Front-panel jack and Realtek Audio Console are both in scope and common; both need a second host before inclusion.
- No PowerShell commands provided: all fixes are Device Manager GUI flows needing device-specific instance IDs. A detection check could use `Get-PnpDevice -Class MEDIA,System | Where-Object Status -ne 'OK'` and `Get-PnpDeviceProperty` for problem code (not sourced, so not in JSON).
