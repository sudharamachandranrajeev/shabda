# services-update — source log (2026-10-28)

Existing catalog entries: none. All three issues are new.

## Issues covered

1. `svc-audio-services-not-running` covers several symptoms: Audiosrv or AudioEndpointBuilder stopped, the red X on the speaker icon, "Audio services not responding" and Error 1068.
   - Fixes: troubleshooter, restart services and set them to Automatic, reinstall the driver, DISM + SFC.
2. `svc-no-sound-after-windows-update` covers sound that stops working after an update.
   - Fixes: check release health and install the latest update, reinstall the PC maker's driver, uninstall the update.
3. `svc-usb-audio-code10-sept-2026-update` is the current Microsoft known issue for USB Audio Class 1.0 devices. The originating update is KB5124008, released 2026-09-08.
   - Fixes: install KB5129195 or a later update, switch to 2-channel stereo.

## Sources consulted

| URL | Type | Confirmation signal |
|-----|------|--------------------|
| https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8 | microsoft | Official page. Covers the Get Help troubleshooter, restarting Windows Audio, AudioEndpointBuilder and RPC, driver update/uninstall/rollback, updates, and restore point. |
| https://support.microsoft.com/en-us/windows/deprecation-of-microsoft-support-diagnostic-tool-msdt-and-msdt-troubleshooters-0c5ac9a2-1600-4539-b9d0-069e71f9040a | microsoft | Official page. The Audio troubleshooter redirects to Get Help on Win11 22H2+. |
| https://learn.microsoft.com/en-us/answers/a/2613323 | microsoft | Accepted answer (2009). Covers starting Windows Audio, setting it to Automatic, and starting the dependencies on Error 1068. The asker accepted it but posted no explicit "it worked". |
| https://learn.microsoft.com/en-us/answers/questions/9ebc6690-8c45-44c9-beb1-e3cfc4fcc8e2/... | microsoft | Accepted answer, 10+ votes. Uninstall the driver and install the OEM driver. The asker confirmed: "That did the trick". Restarting the services had not helped. |
| https://learn.microsoft.com/en-us/answers/questions/5834198/... | microsoft | MS External Staff gave the Set-Service/Restart-Service commands. Not accepted and not confirmed. |
| https://learn.microsoft.com/en-us/answers/questions/5558559/... | microsoft | Independent Advisor suggested DISM/SFC plus a service restart. Not accepted. The asker reported no change. |
| https://learn.microsoft.com/en-us/answers/questions/1693134/... | microsoft | MVP answers suggested the troubleshooter, a service restart and SFC. Not accepted. The asker reported none worked. |
| https://support.microsoft.com/en-us/topic/use-the-system-file-checker-tool-to-repair-missing-or-corrupted-system-files-79aa86cb-ca52-166a-92a3-966e85d4094e | microsoft | Official DISM then SFC procedure. |
| https://support.microsoft.com/en-us/windows/windows-update-faq-8a903416-6f45-0718-f5c7-375e92dddeb2 | microsoft | Official uninstall-update steps for Win11 and Win10, with the warning "We don't recommend…". |
| https://learn.microsoft.com/en-us/answers/questions/5834975/no-sound-after-update | microsoft | Accepted answer. In the question, the asker says uninstalling KB5079473 restored the drivers. Resolved by reinstalling drivers. |
| https://learn.microsoft.com/en-us/windows/release-health/status-windows-11-24h2 | microsoft | Fetched 2026-10-28. USB Audio Class 1.0 issue is Mitigated. AC-3 decoding crash is Resolved by KB5127648. |
| https://support.microsoft.com/help/5129195 | microsoft | OOB 2026-09-14. Fixes the multichannel USB audio failure only. |
| https://learn.microsoft.com/en-us/windows/release-health/resolved-issues-windows-10-22h2 | microsoft | Checked. No audio issues are currently listed. |
| https://learn.microsoft.com/en-us/answers/a/6919957 | microsoft | Error 1068 thread. The top answer (10+ votes) says to check the RPC, MMCSS and AudioEndpointBuilder dependencies. Unresolved. |

## Single-source, not included

- **Error 1079: set the Log On account of Windows Audio and AudioEndpointBuilder to `NT AUTHORITY\LocalService`.**
  - Source: https://learn.microsoft.com/en-us/answers/a/4100247, an accepted answer with about 80 helpful votes. The asker did not confirm.
  - No second source was found.
- **Roll back the audio driver after an update (Device Manager > Driver > Roll Back Driver).**
  - Only the Microsoft sound page gives this fix.
  - No community confirmation was found.
- **System Restore to a point before the update.**
  - Only the Microsoft sound page gives this fix.
- **Error 1075/126: restore the AudioEndpointBuilder service or ServiceDll via a registry file, or a missing audiosrv.dll / AudioEndpointBuilder.dll.**
  - Only blogs give this fix: winhelponline (403 when fetched), wintips and pcrisk.
  - It is a high-risk registry edit, so it is excluded.
- **Reset the MMDevices\Audio registry cache and ACLs.**
  - Source: a follow-up comment in https://learn.microsoft.com/en-us/answers/questions/5558559/. Unconfirmed.
  - It is a high-risk change, so it is excluded.
- **Switch to the generic "High Definition Audio Device" driver after an update.**
  - Only AI-generated answers on Microsoft Q&A give this fix.
  - The `driver-realtek` researcher may cover it.
- **Dirac Audio (cridspapo.dll) loss of audio on Win11 24H2.**
  - The issue was resolved and the safeguard lifted around 2025-09.
  - Only news coverage (borncity) was found. It is no longer on the live release health page.

## Gaps

- The DISM/SFC fix (`svc-dism-sfc`) meets the "Microsoft page + one more" rule, but no source confirms it restored audio. In two Q&A threads where it was tried, the asker reported no change. Treat it as a low-risk hygiene step, not a likely fix.
- The restart-services fix leans on an old (2009) accepted answer plus the official Microsoft page. Modern community confirmation was not found:
  - Reddit returned no threads.
  - Super User returned no pages.
  - HP Community, ElevenForum, TenForums-style and TechPowerUp pages returned 403.
- The USB Code 10 issue (`svc-usb-audio-code10-sept-2026-update`) was still Mitigated on 2026-10-28, and the main Code 10 symptom is not fixed yet. Re-check the release health page on the next refresh.
  - Re-check the stereo fix: its steps (Configure > Stereo, Spatial audio Off) are standard Windows UI paths, but Microsoft's text only says "switching to 2-channel mode".
- The troubleshooter command opens the Troubleshoot settings page (`ms-settings:troubleshoot`) rather than launching the audio troubleshooter directly. `msdt.exe -id AudioPlaybackDiagnostic` is deprecated on Win11 22H2+, so it was not used.
- The AC-3 (Dolby Digital) decoding app-crash known issue (KB5124010, resolved by KB5127648) is not included as an issue. It is an app crash rather than lost sound, and it has only one source.
