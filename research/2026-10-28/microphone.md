# Microphone research log (2026-10-28)

Category: `microphone`. Existing catalog entries: none (new category).

## Issues covered

| issue id | fixes |
|----------|-------|
| `mic-privacy-access-blocked` | `mic-privacy-enable` (verified) |
| `mic-not-detected` | `mic-select-default-input` (verified), `mic-show-disabled-enable` (strong), `mic-run-troubleshooter` (verified), `mic-reinstall-driver` (verified, medium risk) |
| `mic-level-too-low` | `mic-raise-levels-boost` (verified), `mic-disable-enhancements` (verified) |
| `mic-exclusive-mode-conflict` | `mic-check-desktop-app-access` (verified), `mic-disable-exclusive-mode` (verified) |
| `mic-combo-jack-headset` | `mic-combo-jack-set-headset` (verified) |

## Sources consulted

| source | type | confirmation signal |
|--------|------|---------------------|
| https://support.microsoft.com/en-us/windows/fix-microphone-problems-5f230348-106d-bfa4-1db5-336f35576011 | microsoft | Official page: privacy, default input, troubleshooter, driver reinstall, Levels/Boost. No exclusive-mode section. |
| https://www.dell.com/support/kbdoc/en-us/000133024 | vendor | Dell KB: privacy settings only. |
| https://www.dell.com/support/contents/en-ae/article/product-support/self-support-knowledgebase/audio-and-speakers/microphone | vendor | Dell self-support: privacy, input slider, enhancements, troubleshooter, drivers, rollback. |
| https://www.dell.com/support/kbdoc/en-us/000137121/inspiron-notebooks-using-realtek-audio-driver-do-not-recognize-headset-microphone | vendor | Dell KB: Waves MaxxAudio service and jack prompt. |
| https://learn.microsoft.com/en-us/answers/questions/3848626/why-is-my-mic-sound-so-low | microsoft | Accepted answer, 3 helpful votes. Asker confirmed that doing all steps fixed it, but did not say which step did. |
| https://learn.microsoft.com/en-us/answers/questions/3913917/why-does-my-microphone-show-as-disabled | microsoft | Accepted answer. The steps answer has 6 helpful votes. Asker reported partial success. |
| https://learn.microsoft.com/en-us/answers/questions/4182516/windows-11-input-device-volume-automatically-lower | microsoft | MSFT staff answer with 20+ votes; not accepted. OP said it did not fully fix the issue. Top answer (100+ votes) is the Chrome flag. |
| https://learn.microsoft.com/en-us/answers/questions/4106968/how-to-use-headphones-mic-instead-of-laptop-built | microsoft | Realtek retasking answer, 5 votes, not accepted. |
| https://learn.microsoft.com/en-us/answers/questions/3821340/headset-microphones-not-working-3-5mm-trrs-single | microsoft | Realtek "Headset" answer, 4 votes. Asker said it did NOT work. Not cited. |
| https://learn.microsoft.com/en-us/answers/questions/5597521/headset-microphone-is-not-detected-only-laptop-mic | microsoft | No accepted answer. Asker fixed it only with an in-place Windows reinstall. Not cited. |
| https://www.dell.com/community/en/conversations/inspiron/realtek-audio-headphone-mic-not-detected/647f8fe5f4ccf8a8de1085e8 | forum | Retasking "Headset (with microphone)" confirmed by one reply. Thread marked unsolved. |
| https://support.hp.com/us-en/document/ish_1659562-1439556-16 | vendor | HP Win11 mic troubleshooting. **Every fetch timed out.** Steps (Show Disabled Devices, Enable, exclusive control) were seen only in a search excerpt. |
| https://devforum.zoom.us/t/microphone-volume-reduced-in-chrome-when-using-zoom-videosdk/139011 | forum | Chrome WebRTC flag. Only the OP confirmed; Zoom staff could not reproduce. Not cited. |

## Single-source, not included

- **Chrome flag `chrome://flags` > "Allow WebRTC to adjust the input volume" = Disabled** stops Chrome from lowering the Windows mic level. Source: MS Q&A 4182516, 100+ helpful votes. No second high-signal source: the Zoom devforum thread is unconfirmed, and the Brave community thread was not fetched. Strong candidate for the next refresh.
- **Disable "Automatically adjust microphone settings"** in Zoom/Teams/Discord. Only source: the MSFT staff answer in 4182516.
- **Communications tab "Do nothing"** (mmsys.cpl). Suggested for mic level drops in MS Q&A 4182516. Most other sources say it ducks *playback* volume, not mic input, so it is likely not a real mic fix.
- **Remove leftover virtual audio drivers** (VB-Audio, Voicemeeter) via Device Manager > Show hidden devices. Only source: MS Q&A 3848626, accepted.
- **Restart Windows Audio (`net stop audiosrv` / `net start audiosrv`)**. Only source here: MS Q&A 3848626. Likely covered by the `services-update` category.
- **Roll back the audio driver after a Windows Update.** Only source: Dell self-support.
- **Win11 per-device "Allow apps and Windows to use this device"** (Sound > device properties). Asker in MS Q&A 3913917 reported odd behavior with it. One source only.
- **Combo jack needs a CTIA 4-pole TRRS plug; use a splitter on separate jacks.** Seen in MS Q&A and Dell Community search excerpts, not confirmed on fetched pages.
- **Switch Realtek to the generic "High Definition Audio Device" driver** for a combo-jack mic. Dell Community excerpt only, and other users say the opposite.
- **Select the right mic inside the app's own audio settings.** The MS support page covers this only for Skype, so no generic second source was found.

## Gaps

- superuser.com and reddit.com are blocked for this crawler. No Stack Exchange or Reddit vote counts were collected.
- tenforums.com, elevenforum.com, support.discord.com and support.steelseries.com returned HTTP 403.
- support.hp.com timed out on every fetch. HP is cited on two fixes from a search excerpt only. Re-verify HP when reachable. If HP still fails, `mic-show-disabled-enable` stays "strong" and `mic-disable-exclusive-mode` rests on the learn.microsoft.com sources only.
- The Blizzard exclusive-control article (000197796) redirects to a login wall.
- No `detection.checkIds` were mapped; the app maintainer should assign them.
- The Recording Audio troubleshooter `msdt` command was left `null`. MSDT troubleshooters are being retired on Windows 11, so the Settings path is used instead.
- Bluetooth headset mic (Hands-Free profile) issues were not researched. They likely belong to the `bluetooth` category.
