# default-device research log (2026-10-28)

## Issues covered
- `default-output-switches-on-device-change`: wrong default output, or output switches by itself (monitor sleep/wake, HDMI/DP/dock connect, restart). Fixes: set preferred default; disable unused monitor audio endpoint.
- `default-device-disabled-hidden`: device disabled / Don't allow / hidden in the Sound control panel or Device Manager. Fixes: Allow in Settings; Show Disabled Devices + Enable in mmsys.cpl; Device Manager enable + scan for hardware changes.
- `default-app-routing-stuck`: per-app output (Volume mixer / App volume and device preferences) stuck on the wrong device. Fixes: set the app back to Default; Reset all apps to recommended defaults.

## Sources consulted
| URL | Type | Signal |
|-----|------|--------|
| https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8 | microsoft | Official page. Step 5 covers disabled devices and Step 6 covers the default device (Win10 and Win11). |
| https://support.microsoft.com/en-au/windows/fix-missing-or-undetected-audio-output-device-in-windows-5504aed3-2c01-4214-89d1-9e8dbe6828e8 | microsoft | Official page. Device Manager steps: Enable device, Show hidden devices, Scan for hardware changes. |
| https://support.microsoft.com/topic/c2ebf6ed-4768-4ce5-8cb0-4cf195f5c431 | microsoft | Official page "Fix app audio not working while system sounds work". Covers the per-app Output device in Volume mixer. |
| https://learn.microsoft.com/en-us/answers/questions/4321330/re-enable-audio-device | microsoft | Answer from MSFT staff with 100+ helpful votes. Not marked accepted. |
| https://learn.microsoft.com/en-us/answers/questions/3865004/how-do-i-stop-windows-from-automatically-switching | microsoft | Top answer with 3 helpful votes. Not accepted, thread locked, no OP confirmation. |
| https://learn.microsoft.com/en-us/answers/questions/2387521/edge-switching-from-default-sound-output-device-to | microsoft | Moderator answer. The OP confirms that setting the app to Default and using Reset both work, but only temporarily. |
| https://learn.microsoft.com/en-us/answers/questions/1337541/ap-volume-and-device-properties | microsoft | Answer gives the Reset path. The OP says it did not fix his case. Weak source. |
| https://techcommunity.microsoft.com/discussions/windows11/windows-11-stops-recognizing-monitors-as-audio-output-devices-when-idle/3587354 | forum | Several users each report a different fix, and each fix has about one confirmation. No accepted answer and no like counts. |
| https://dell.com/support/kbdoc/tr-tr/000125484/no-sound-with-displayport-or-hdmi-video-cable-connection?lang=en | vendor | Dell KB 000125484 says Windows switches the default to DP/HDMI when the cable is connected or the PC resumes from sleep. It advises setting the default or disabling that device. Read only from the search excerpt because every fetch variant returned 404. |
| https://support.lenovo.com/au/en/solutions/ht514384 | vendor | Lenovo article on resetting sound to the Microsoft recommended defaults. Read only from the search excerpt because the fetch returned 403. |
| https://learn.microsoft.com/en-us/answers/questions/5628614/default-sound-device-changed-unexpectedly | microsoft | Suggests unchecking exclusive mode and reinstalling the Realtek driver. The OP confirmed that neither fixed it. Used only for context. |
| https://learn.microsoft.com/en-us/answers/a/6538017 | microsoft | 121 answers, many confirming the problem (HDMI/DP auto-switch). Shows no confirmed fix. Context only. |

Blocked from fetching: elevenforum.com, tenforums.com and techpowerup.com (all 403). Super User could not be searched (domain not accessible).

## Single-source, not included
- Uncheck "Allow applications to take exclusive control of this device" and "Give exclusive mode applications priority" to stop default switching. Sources: MS Q&A 5628614 and 2387521, both moderator suggestions with no confirmation.
- Switch NVIDIA HDMI audio to the Microsoft High Definition Audio driver, or update the NVIDIA driver, so monitor audio survives idle. Source: one confirmation each in the Tech Community thread. This may belong to the driver or HDMI category.
- Sleep and wake the PC to bring back a missing monitor output. Source: Tech Community, 2 users. Workaround only.
- Never sleep displays and use a blank screensaver instead. Source: Tech Community, 1 user. Workaround only.
- Uncheck "Allow the computer to turn off this device" on display audio devices in Device Manager. Source: Tech Community. Suggested, not tested.
- Clean boot to stop per-app routing resetting after reboot. Source: MS Q&A answer 5587745 (3 helpful votes, no confirmation).
- Registry `Role` DWORD, or a Task Scheduler task plus the third-party SoundVolumeView to lock the default. Sources: allthings.how (blog) and MS Q&A 5628614. Not included: it uses a third-party exe and a registry edit with no high-signal source.
- Disable HDMI audio in Device Manager instead of in the Sound panel. Sources: Dell community post (one user says it did not help) and an Acer Corner blog (could not read).

## Gaps
- I could not read the Dell and Lenovo vendor pages directly; I only saw search excerpts. The two fixes that rely on them are marked `strong`, not `verified`. A later refresh should re-fetch these pages.
- No Super User, Reddit or ElevenForum/TenForums answers could be read. A refresh with access should look for high-vote answers on "Windows switches audio to monitor" and on the per-app routing reset.
- Many users report that Windows keeps switching the default even after the device is disabled (MS Q&A 6538017, 121 answers). No confirmed permanent fix exists, and the catalog fixes only reduce how often it happens.
- The wrong default recording device (microphone) is not covered as its own issue. The microphone category probably covers it.
- `enable-in-device-manager` command: `Get-PnpDevice -Status ERROR` also catches MEDIA devices that are in error for reasons other than being disabled. Enable-PnpDevice should do nothing to those, but this was not tested. `pnputil /scan-devices` needs Windows 10 2004 or later.
- None of the sources give a built-in cmdlet that sets the default endpoint or enables/disables an endpoint, so those fixes have `command: null`.
