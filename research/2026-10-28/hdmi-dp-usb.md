# hdmi-dp-usb source log (2026-10-28)

Existing catalog entries: none. All entries are new.

## Issues covered

1. `hdmi-display-audio-missing-or-silent`: no sound from the HDMI/DP monitor or TV, or it is missing from the output list. 5 fixes.
2. `hdmi-audio-lost-after-display-sleep`: monitor or TV audio is lost after the display sleeps. 3 fixes, all `strong`.
3. `usb-audio-dropouts-power-saving`: a USB headset, DAC or mic cuts out, disconnects or is not detected. 3 fixes, all `verified`.

## Sources consulted

| URL | type | signal |
|-----|------|--------|
| https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8 | microsoft | Official. Covers: set HDMI/USB as default, monitor without speakers, enable a device, update/reinstall/roll back a driver, try another USB port |
| https://www.samsung.com/us/support/troubleshoot/TSG10001504 | vendor | Official Samsung page for no sound from an HDMI device: select output, show disconnected devices and enable, GPU-vendor HDMI audio drivers, monitors without speakers |
| https://learn.microsoft.com/en-us/answers/questions/5849029/missing-hdmi-output | microsoft | Q&A. Independent Advisor answer, NOT accepted, asker did not confirm. Used only as a third, supporting source |
| https://forums.tomsguide.com/threads/connected-my-laptop-to-my-tv-through-hdmi-cable-but-cant-get-any-sounds.373904/post-1642799 | forum | Post marked Solution (scan for hardware changes). Two replies confirm "Setting to default worked" |
| https://techcommunity.microsoft.com/discussions/windows11/windows-11-stops-recognizing-monitors-as-audio-output-devices-when-idle/3587354 | forum | Long Microsoft Tech Community thread with no accepted solution. Single-user confirmations: hardware scan (temporary), NVIDIA driver update, sleep/wake of the PC (2 users) |
| https://learn.microsoft.com/en-us/answers/questions/5865398/lg-tv-not-recognized-as-audio-device-without-resta | microsoft | Q&A about an LG TV lost after power-off. Not accepted, no confirmation. Discovery only |
| https://support.logi.com/hc/en-ca/articles/360023347833 | vendor | Official Logitech page: disable root hub power management and USB selective suspend |
| https://support.m-audio.com/en/support/solutions/articles/69000851050-... | vendor | Official M-Audio page: disable USB selective suspend |
| https://support.rationalacoustics.com/support/solutions/articles/150000091757-... | vendor | Official Rational Acoustics page: root hub and host controller power management; avoid hubs and extenders |
| https://www.pugetsystems.com/labs/support-software/How-to-Disable-USB-Selective-Suspend-1190/ | vendor | Puget Systems support article (system builder, dated 2018): selective suspend and root hub power management |
| https://www.intel.com/content/www/us/en/support/articles/000037869.html | vendor | Intel: HDMI audio missing with UHD 630 in legacy BIOS mode; fix is to enable UEFI |
| https://forums.overclockers.co.uk/threads/windows-10-hdmi-sound-issue-after-waking-from-sleep.18703963 | forum | No replies, no fix. Not used |

Fetch failures: elevenforum.com (403), support.audient.com (403), thewindowsclub.com (403), dell.com KB 000125484 (404 on every URL variant tried).

## Command notes

- `pnputil /scan-devices` is the built-in equivalent of Device Manager's "Scan for hardware changes". It needs Windows 10 2004 or later and runs elevated.
- I translated the powercfg selective-suspend command from the GUI steps. No source gives it. GUIDs: USB settings subgroup `2a737441-1930-4402-8d77-b2bebba308a3`, USB selective suspend setting `48e6b7a6-50f5-4782-a5d4-53bb8f07e226`. Value 0 means Disabled. The maintainer should test it on a real machine.

## Single-source, not included

- Intel: switch the BIOS from legacy/CSM to UEFI when Intel UHD 630 HDMI audio is missing (intel.com only). The fix is also high-risk because it can stop the PC from booting.
- Dell KB 000125484: after a laptop wakes from sleep, Windows switches the default output to the monitor cable; disable the external audio device to stop it happening again. Seen only in search snippets; the page returned 404.
- Put the PC to sleep and wake it to bring back a TV's audio output. Two users confirmed it, but both on the same Tech Community thread.
- Never let the display sleep; use a blank screen saver instead (Tech Community, one user).
- Clear "Allow the computer to turn off this device" on the HD Audio display devices (Tech Community, untested; also suggested in an unfetched HP forum thread).
- Switch the NVIDIA HDMI audio device to Windows' generic High Definition Audio Device driver (Tech Community, one user). The Microsoft support page describes the generic-driver step only in general terms.
- NVIDIA Control Panel > Display > Set up digital audio (SEO blogs only).
- Disable the NVIDIA HD Audio sleep timer and enable MSI through the registry (one r/htpc post, about start-up delay rather than sleep loss).
- EDID override INF files built with MonInfo (Kodi forum only).
- Clean graphics driver removal with DDU. It is third-party software, which the rules exclude.

## Gaps

- No Super User answer was found with ≥ 25 votes or accepted; `site:` searches returned nothing. No high-vote Reddit thread was confirmed either.
- No official NVIDIA or AMD knowledge-base article on HDMI/DP audio was found.
- There is no separate issue for "USB headset connected but not in the output list / not the default". Only the Microsoft support page covered it, so it is folded into `usb-direct-port` and `hdmi-set-default-output`. A later refresh could add a dedicated `usb-` entry.
- Not researched: USB DAC sample-rate or exclusive-mode problems, and AMD-specific HDMI audio driver problems.
- The NVIDIA Linux driver 515.48.07 FRL/high-refresh HDMI audio fix is Linux-only and out of scope.
