# Quality category: source log (2026-10-28)

## Issues covered
- `qual-crackling-enhancements-format`: crackling or distortion. Fixes: disable enhancements, change default format, update driver.
- `qual-power-saving-crackling`: crackling caused by power saving. Fixes: High performance plan, minimum processor state 100%, USB selective suspend off.
- `qual-dpc-latency`: clicks and dropouts from high DPC latency. Fixes: update system drivers, temporarily disable Wi-Fi to test.
- `qual-sample-rate-mismatch`: Windows sample rate differs from the app or interface. Fix: match the default format.

## Sources consulted
| URL | Type | Signal | Used |
|---|---|---|---|
| https://support.microsoft.com/windows/fix-distorted-or-crackling-audio-in-windows-5304e452-38a6-4f3b-83cd-664beb3e68aa | microsoft | Official page. Lists 4 fixes: enhancements, default format, driver, restart audio services. | yes |
| https://support.native-instruments.com/support/solutions/articles/69000880009-windows-tuning-tips-for-audio-processing | vendor | Official NI tuning guide. Covers power settings, sample-rate matching, LatencyMon, a driver table and network-adapter disabling. | yes |
| https://support.enginedj.com/support/solutions/articles/69000803869-troubleshooting-dpc-latency | vendor | Official inMusic/Engine DJ DPC article. Covers High Performance plan, Wi-Fi disable test and driver updates. | yes |
| https://learn.microsoft.com/en-us/answers/a/7161555 (same thread as /a/6587696) | forum (MS Q&A, not accepted) | Answers marked helpful by 10+ people each. Posters report enhancements/exclusive-mode off and Fast Startup off fixed it. The asker never confirmed. | yes (enhancements, as second source only) |
| https://learn.microsoft.com/en-us/answers/a/4861142 | MS Q&A | 30+ helpful for "disable enhancements, 16-bit 48000 Hz, update driver". The asker said it did NOT work. | no |
| https://learn.microsoft.com/en-us/answers/questions/583991/ndis-sys-cause-high-dpc-latency | MS Q&A | No accepted answer. Network driver reinstall did not work for the OP. | no |
| https://forums.tomshardware.com/threads/dpc-latency-issues-after-fresh-windows-10-install.3754531 | forum | "Solution" tag on speculation. ndis.sys latency remained. | no |
| https://www.dell.com/community/en/conversations/windows-general/xps-15-9510-realtek-sound-echo-solved/647f9adff4ccf8a8dee9fff7 | forum | Community-accepted. Turning ON Windows Sonic fixed echo on a Realtek XPS 15. One user fixed it by turning off "Enhance audio" instead. | no (single source) |
| https://learn.microsoft.com/en-us/answers/a/5760574 | MS Q&A | DTS Sound Unbound distortion. Top answer (20+) is a DTS app workaround. A 1-vote answer says turning off spatial sound fixed it. | no |
| https://support.microsoft.com/en-US/Windows/Hardware/Drivers/how-to-turn-on-spatial-sound-in-windows | microsoft | Shows how to turn spatial sound on. No troubleshooting. | no |
| https://www.dell.com/community/Laptops-General-Read-Only/Inspiron-15-5000-audio-crackling-Netflix-YouTube-only/m-p/5173456 | forum | UNSOLVED, no confirmations. | no |

The following pages returned HTTP 403 and could not be checked: help.ableton.com 115000211304 (High performance plan), support.focusrite.com 208360865 (DPC latency), techpowerup thread 225533, h30434.www3.hp.com threads, ar.msi.com FAQ 11152, and the elevenforum tutorials (audio enhancements 8426, audio format 46412, spatial sound 5837). superuser.com is blocked for search. A later refresh with a different fetcher should retry them, especially Ableton, Focusrite and ElevenForum.

## Single-source, not included
- **Restart the Windows Audio and Audio Endpoint Builder services** (Microsoft crackling page only). The services-update category may cover it.
- **Clear "Allow applications to take exclusive control" and hardware acceleration** (MS Q&A answer, 10+ helpful, not accepted).
- **Disable Fast Startup** (MS Q&A answer, 10+ helpful; another answer in the same thread says it did not help).
- **Turn spatial sound Off, or toggle Windows Sonic, to fix distortion or echo.** Evidence conflicts: Dell thread turned it ON, MS Q&A turned it OFF with 1 vote.
- **Switch Realtek to the generic "High Definition Audio Device" driver.** The asker reported the fix was temporary, and the Dell thread is unconfirmed. This belongs to driver-realtek.
- **Uninstall AMD Audio CoProcessor** (HP forum quoting Reddit, unverified).
- **Uninstall Equalizer APO** (one Spotify community post).
- **Processor performance boost policy 0%** (one Intel community post).
- **Close apps causing hard pagefaults** (Focusrite, page 403).
- **Disable NVIDIA PowerMizer / set "Prefer maximum performance"** (NI guide plus one NI community user; not fetched further).

## Gaps
- No high-signal Reddit or Super User threads could be fetched. Community confirmation rests mainly on Microsoft Q&A helpful counts and vendor pages.
- The spatial sound issue (Windows Sonic, Dolby Atmos, DTS distortion) is not in the catalog for lack of two agreeing sources.
- Latency/delay (audio out of sync, Bluetooth lag) and robotic Bluetooth audio were not researched. Robotic audio is usually HFP/hands-free profile, which belongs to the bluetooth category.
- LatencyMon is third-party, so no fix runs it. The app can only suggest it, and the DPC fixes do not depend on it.
- `powercfg /setactive SCHEME_MIN` fails on Modern Standby laptops where High performance is hidden. The app should check `powercfg /list` first.
- The minimum-processor-state fix has no command. It translates to `powercfg /setacvalueindex SCHEME_CURRENT SUB_PROCESSOR PROCTHROTTLEMIN 100` followed by `powercfg /setactive SCHEME_CURRENT`. That command is not given by a source, so it is not included.
