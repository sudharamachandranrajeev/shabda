# Bluetooth audio research log (2026-10-28)

Category: `bluetooth`. Existing catalog entries: none (this is a new category).

## Issues covered

| id | fixes |
|----|-------|
| bt-connected-no-output-endpoint | bt-select-output-device, bt-remove-repair, bt-run-troubleshooter, bt-update-adapter-driver |
| bt-hfp-low-quality | bt-disable-handsfree-telephony (strong) |
| bt-stutter-wifi-interference | bt-use-5ghz-wifi |
| bt-absolute-volume-low | bt-repair-volume, bt-disable-absolute-volume (high risk, registry) |
| bt-multipoint-phone-holds-audio | bt-stop-phone-playback |

"Pairing OK but no sound" is folded into bt-connected-no-output-endpoint, because the sources treat them as one problem.

## Sources consulted

| URL | type | signal |
|-----|------|--------|
| https://support.microsoft.com/windows/fix-bluetooth-connected-but-no-sound-issue-on-windows-90cf598f-ffdb-426f-b253-bc5cc98a30ea | microsoft | Official article. Steps: troubleshooter, default output, 2ch/16-bit/48 kHz format, re-pair, toggle BT, update driver, confirm A2DP, Windows Update, restart. |
| https://support.microsoft.com/en-us/windows/fix-bluetooth-problems-in-windows-723e092f-03fa-858b-5c80-131ec3fba75c | microsoft | Official article. Steps: troubleshooter, remove and re-add, update or uninstall adapter driver. |
| https://support.microsoft.com/en-us/windows/bluetooth-volume-controls-no-longer-work-753b971f-2ba3-4149-94a6-65a2f47bf87c | microsoft | Official article. The search index shows the `reg add ... DisableAbsoluteVolume /d 1` command. **Direct fetch redirected to the Windows help landing page.** The article may have been retired; recheck on the next refresh. |
| https://learn.microsoft.com/en-us/answers/a/6541385 | microsoft (Q&A) | Re-pair, then Playback tab Connect / Set default: 100+ helpful. Troubleshooter, Bluetooth Support Service, driver: 70+ helpful. Asker said neither worked. Not accepted. |
| https://learn.microsoft.com/en-us/answers/questions/359040/... | microsoft (Q&A) | MS staff gave DisableAbsoluteVolume, 20+ helpful. Comments are mixed: 2 fixed by re-pair, 1 says the registry change broke the headphones, 1 says it had no effect. |
| https://learn.microsoft.com/en-us/answers/questions/98578b41-.../audio-goes-bad-whenever-i-use-the-microphone | microsoft (Q&A) | Asker: unchecking Hands-free Telephony fixes quality but removes the mic. Advisor (3 helpful): this is a protocol limitation; use another mic. |
| https://learn.microsoft.com/en-us/answers/a/8079406 | microsoft (Q&A) | Advisor answer, 5 helpful: turn off Hands-Free Telephony. |
| https://learn.microsoft.com/en-us/answers/questions/4012096/... | microsoft (Q&A) | 20+ helpful: disable the Bluetooth Audio Gateway Service (system-wide, loses battery level). Not included; see below. |
| https://forums.flightsimulator.com/t/bluetooth-headphone-audio-is-distorted-unless-handsfree-telephony-is-disabled/684298 | forum | 3 users confirm Handsfree Telephony off fixes distortion. It resets on reboot or reconnect. 2 users say it did not help. |
| https://learn.microsoft.com/en-us/answers/a/2013066 | microsoft (Q&A) | Stutter thread, asker never fixed. Comment (10 votes, 1 confirmation): uninstall device + rescan + uncheck Handsfree Telephony and Remotely Controllable Device. MS answer (1 vote, disputed): power management + restart Bluetooth Support Service. Motherboard antenna (2 votes, 1 confirmation). |
| https://www.intel.com/content/www/us/en/support/articles/000005821/wireless/legacy-intel-wireless-products.html | vendor | Intel recommends 5 GHz Wi-Fi to avoid Bluetooth interference. Audio is not mentioned specifically. |
| https://community.intel.com/t5/Wireless/WIfi-Stutters-when-using-bluetooth-audio-cannot-find-bluetooth/m-p/648160 | vendor | Intel support technician: switch to 5 GHz, clean-install OEM drivers. No asker confirmation. |
| https://techcommunity.microsoft.com/t5/windows-11/win11-bluetooth-audio-stutters-when-wi-fi-is-active-on-2-4-ghz/m-p/4547692 | (not used) | 0 kudos, no best answer. One user said disabling AAC fixed it (single source). |
| https://techcommunity.microsoft.com/discussions/windows10space/...4490401 | (not used) | Only confirms the A2DP/HFP limitation. No fix. |
| https://helpguide.sony.net/ht/2078/v1/en/contents/TP1000954309.html | vendor | Sony: multipoint plays the last-used device; stop playback on the first device. |
| https://learn.microsoft.com/en-us/answers/a/2108904 | microsoft (Q&A) | Multipoint: asker reports PC audio returns only after pause/restart. MVP: no Windows setting, so it is headset firmware. 2 helpful. |
| https://forums.windowscentral.com/threads/my-bluetooth-speaker-pairs-and-connects-but-makes-not-a-sound.430234/ | (not used) | No resolution. |

Blocked (HTTP 403 or fetch refused): elevenforum.com (Absolute Volume tutorial; "Solved" threads 47707, 15312, 13487, 31070), tenforums.com (Absolute Volume tutorial 115263), superuser.com (question 1372434), sony.co.in WH-1000XM5 article. The search snippets suggest these confirm the included fixes, but they were not verified, so they are not cited.

## Single-source, not included

- **Set the output format to 2 channel, 16 bit, 48000 Hz** in Advanced sound properties. Source: MS support no-sound article only.
- **Set Bluetooth Support Service to Automatic / restart it.** Sources: learn.microsoft.com only (6541385, 70+ helpful; 2013066, 1 vote, disputed). Second host not found.
- **Disable the Bluetooth Audio Gateway Service** (services.msc) to stop HFP switching for all devices. Source: learn.microsoft.com 4012096 (20+ helpful) only. This is system-wide and loses features.
- **Uncheck "Allow the computer to turn off this device to save power"** on the BT adapter, for stutter. Source: one MS Q&A answer (1 vote, disputed). The rest are SEO blogs.
- **Uninstall the device + Scan for hardware changes + uncheck Handsfree Telephony and Remotely Controllable Device**, for stutter. Source: learn.microsoft.com 2013066 comment (10 votes, 1 confirmation).
- **Attach the motherboard Wi-Fi/BT antennas.** Source: learn 2013066 (2 votes, 1 confirmation).
- **Disable AAC encoding (registry)**, for stutter. Source: techcommunity, 1 user, 0 kudos.
- **Intel April 2026 wireless driver (Wi-Fi 24.40.0 / BT 24.40.0.3) coexistence fix.** Sources: news sites only (windowslatest, hothardware). Check Intel release notes on the next refresh.
- **Set the 'Headphones' (Stereo) endpoint as default and use another mic** without disabling HFP. Sources: learn.microsoft.com answers only.
- **Disconnect the phone to restore PC audio (Samsung community).** The page was not fetched.

## Gaps

- **Stutter coverage is thin.** Only the 5 GHz Wi-Fi fix met the bar. Driver updates, power management and USB 3.0 interference all need better sources.
- **The Microsoft "Bluetooth volume controls no longer work" article could not be fetched.** If it is retired, bt-disable-absolute-volume rests on the MS Q&A answer alone. That answer has mixed comments, and only one source would remain.
- **The bt-disable-handsfree-telephony check box location varies by build.** Windows 11 Settings may show a "Hands-Free Telephony" toggle under the device's properties; the Control Panel Services tab is the documented route. Whether it needs admin is unverified, so `requiresAdmin` is set to true conservatively.
- **No high-signal source for Bluetooth LE Audio / super-wideband (Windows 11 24H2+).** It avoids the HFP quality drop on supported hardware. Worth a dedicated search.
- **Multipoint fix confidence rests on a vendor guide plus one self-report.** No Windows-side setting exists, per an MVP.
- **No Reddit thread surfaced in the searches.**
