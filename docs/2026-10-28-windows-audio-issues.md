# Windows sound issues and verified fixes

Generated 2026-10-28. Every fix has at least two independent sources. `verified` means an official Microsoft or vendor source, or an accepted high-vote answer, backs it; `strong` means independent community confirmation.

Try fixes in the order listed: they go from least to most invasive.

## Bluetooth

### Bluetooth volume is too low or the volume controls do not work

`bt-absolute-volume-low` · win10, win11

**Symptoms**

- Bluetooth speaker or headphones are quiet even at 100% volume in Windows
- The Windows volume slider does not change the Bluetooth device volume
- Volume jumps or resets when the device reconnects

**Causes**

- Windows Absolute Volume links the Windows slider to the device's own volume, and some devices handle it badly
- A Windows update changed how Absolute Volume works with the device

**Fix 1: Remove the device and pair it again** (verified, risk: low)

1. Open Settings > Bluetooth & devices > Devices.
2. Select More options (...) next to the device and choose Remove device.
3. Pair the device again, then set the volume in Windows and on the device.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-bluetooth-problems-in-windows-723e092f-03fa-858b-5c80-131ec3fba75c) (Official Microsoft support article, remove and re-add the device) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/359040/where-can-i-set-the-default-volume-for-bluetooth-d) (Microsoft Q&A volume thread: two commenters report removing and re-adding the device fixed it (one says volume later reverted))

### Bluetooth speaker or headphones connected but no sound or missing from output list

`bt-connected-no-output-endpoint` · win10, win11

**Symptoms**

- Settings shows the Bluetooth speaker or headphones as Connected, but no sound comes out
- Sound keeps playing from the PC speakers instead of the Bluetooth device
- The device is missing from the Sound output list, or shows as Disconnected in the Playback tab
- Settings shows only 'Connected' instead of 'Connected voice, music' / 'Audio, Voice'

**Causes**

- Windows is still sending audio to another output device
- The stereo (A2DP) audio endpoint was not created or is in a disconnected state after pairing
- The pairing record or Bluetooth driver is in a bad state after an update
- The device does not support the A2DP stereo audio profile

**Fix 1: Select the Bluetooth device as the output device** (verified, risk: low)

1. Select the Sound icon in the taskbar and pick the Bluetooth device from the output list.
2. Or open Settings > System > Sound and choose the Bluetooth device under Output.
3. If it is not listed, open Settings > System > Sound > More sound settings > Playback tab.
4. If the Bluetooth device (usually 'Headphones' or 'Speakers', not 'Headset') shows as Disconnected, right-click it and select Connect.
5. Right-click the Bluetooth device and select Set as Default Device.

Sources: [support.microsoft.com](https://support.microsoft.com/windows/fix-bluetooth-connected-but-no-sound-issue-on-windows-90cf598f-ffdb-426f-b253-bc5cc98a30ea) (Official Microsoft support article, step 'Check the default output device') · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/a/6541385) (Microsoft Q&A answer by Independent Advisor, 100+ people found it helpful (not marked accepted))

**Fix 2: Remove the device and pair it again** (verified, risk: low)

1. Open Settings > Bluetooth & devices > Devices.
2. Select More options (...) next to the speaker or headphones and choose Remove device.
3. Turn Bluetooth off, wait about 10 seconds, then turn it back on.
4. Put the speaker or headphones in pairing mode and select Add device to pair it again.

Sources: [support.microsoft.com](https://support.microsoft.com/windows/fix-bluetooth-connected-but-no-sound-issue-on-windows-90cf598f-ffdb-426f-b253-bc5cc98a30ea) (Official Microsoft support article, steps 'Unpair and re-pair' and 'Toggle Bluetooth') · [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-bluetooth-problems-in-windows-723e092f-03fa-858b-5c80-131ec3fba75c) (Official Microsoft support article, step 5 remove and re-add the device) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/a/6541385) (Microsoft Q&A answer by Independent Advisor, 100+ people found it helpful; first step is remove and pair again)

**Fix 3: Run the Bluetooth troubleshooter** (verified, risk: low)

1. Windows 11: open the Get Help app and run the Bluetooth troubleshooter, or go to Settings > System > Troubleshoot > Other troubleshooters and select Run next to Bluetooth.
2. Windows 10: go to Settings > Update & Security > Troubleshoot > Additional troubleshooters and run Bluetooth.
3. Follow the on-screen instructions.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-bluetooth-problems-in-windows-723e092f-03fa-858b-5c80-131ec3fba75c) (Official Microsoft support article) · [support.microsoft.com](https://support.microsoft.com/windows/fix-bluetooth-connected-but-no-sound-issue-on-windows-90cf598f-ffdb-426f-b253-bc5cc98a30ea) (Official Microsoft support article, first step) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/a/6541385) (Microsoft Q&A community answer, 70+ people found it helpful)

**Fix 4: Update the Bluetooth adapter driver** (verified, risk: medium, admin)

1. Open Device Manager and expand Bluetooth.
2. Right-click the Bluetooth adapter (often named with 'Wireless Bluetooth' or 'radio') and select Update driver > Search automatically for drivers.
3. If Windows finds nothing, install the latest Bluetooth driver from the PC or motherboard manufacturer's support page.
4. Restart the PC.

Sources: [support.microsoft.com](https://support.microsoft.com/windows/fix-bluetooth-connected-but-no-sound-issue-on-windows-90cf598f-ffdb-426f-b253-bc5cc98a30ea) (Official Microsoft support article, step 'Update drivers') · [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-bluetooth-problems-in-windows-723e092f-03fa-858b-5c80-131ec3fba75c) (Official Microsoft support article, steps 7-8) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/a/6541385) (Microsoft Q&A community answer, 70+ people found it helpful (driver update / install from PC maker))

### Bluetooth headphones sound muffled or mono when the microphone is in use

`bt-hfp-low-quality` · win10, win11

**Symptoms**

- Music or game audio becomes muffled, tinny or mono when a call, Discord, Teams or a game with voice chat starts
- Audio sounds fine until an app opens the microphone
- Two entries appear for the headset: 'Headphones' (Stereo) and 'Headset' (Hands-Free)

**Causes**

- When an app uses the headset microphone, Windows switches the headset from the stereo profile (A2DP) to the Hands-Free profile (HFP), which carries mono, low-bitrate audio
- Bluetooth Classic cannot use the headset microphone and the stereo profile at the same time

**Fix 1: Turn off Hands-free Telephony for the headset (headset mic stops working)** (strong, risk: low, admin)

1. Know the trade-off first: the headset microphone will stop working. Use the PC's built-in microphone or another microphone for calls.
2. Open Settings > Bluetooth & devices > Devices and select More devices and printer settings (Windows 10: Control Panel > Devices and Printers).
3. Right-click the headset and select Properties.
4. Open the Services tab and clear the Hands-free Telephony check box.
5. Select Apply, then OK.
6. In Settings > System > Sound, set Input to another microphone.
7. If the setting turns back on after a reboot, reconnect or Windows update, repeat these steps. To undo, select the check box again.

Sources: [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/98578b41-0fd8-465e-b174-b2f48047cf60/audio-goes-bad-whenever-i-use-the-microphone) (Microsoft Q&A: asker confirms unchecking Hands-free Telephony fixes quality but removes the mic; advisor answer (3 helpful) explains the limitation and recommends another mic) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/a/8079406) (Microsoft Q&A advisor answer, 5 people found it helpful; not accepted) · [forums.flightsimulator.com](https://forums.flightsimulator.com/t/bluetooth-headphone-audio-is-distorted-unless-handsfree-telephony-is-disabled/684298) (Three users report that unchecking Handsfree Telephony in the Services tab fixes the audio; two report it resets after a reboot or reconnect)

### No sound on the PC because the speaker or headphones are also connected to a phone

`bt-multipoint-phone-holds-audio` · win10, win11

**Symptoms**

- The speaker or headphones are connected to the PC and a phone at the same time
- The PC shows the device as connected, but sound plays from the phone or nothing plays
- PC audio stops after a phone call or notification and does not come back

**Causes**

- In a multipoint connection, only one device plays audio at a time, usually the one that played last
- The phone keeps the audio link after a call or notification

**Fix 1: Stop playback on the phone, then restart playback on the PC** (verified, risk: low)

1. Pause or stop music, video and calls on the phone, and close its media app.
2. On the PC, pause playback and start it again.
3. If sound still does not play, turn off Bluetooth on the phone or disconnect the speaker from the phone.
4. If the device has a companion app, check that its multipoint setting (for example 'Connect to 2 devices simultaneously') is on.

Sources: [helpguide.sony.net](https://helpguide.sony.net/ht/2078/v1/en/contents/TP1000954309.html) (Official Sony help guide: audio plays from the last-used device; stop playback on the first device to hear the second) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/a/2108904) (Microsoft Q&A (MVP moderator replied): asker reports PC audio stays silent after phone use until playback is paused and restarted)

### Bluetooth audio stutters or drops out while Wi-Fi is in use

`bt-stutter-wifi-interference` · win10, win11

**Symptoms**

- Bluetooth audio crackles, skips or cuts out
- Dropouts are worse during downloads, streaming or online games
- Turning off Wi-Fi makes the Bluetooth audio smooth

**Causes**

- Bluetooth and 2.4 GHz Wi-Fi share the same radio band and, on combo Wi-Fi/Bluetooth cards, the same antenna

**Fix 1: Connect the PC to a 5 GHz Wi-Fi network** (verified, risk: low)

1. Select the Wi-Fi icon in the taskbar and open the Wi-Fi network list.
2. Connect to your router's 5 GHz network if it has one (often named with '5G').
3. To check the band, open Settings > Network & internet > Wi-Fi > the network's properties and look at Network band.
4. If you cannot use 5 GHz, use a wired Ethernet connection or test with Wi-Fi turned off to confirm the cause.

Sources: [intel.com](https://www.intel.com/content/www/us/en/support/articles/000005821/wireless/legacy-intel-wireless-products.html) (Official Intel support article: recommends 5 GHz Wi-Fi to avoid interference between Bluetooth and Wi-Fi) · [community.intel.com](https://community.intel.com/t5/Wireless/WIfi-Stutters-when-using-bluetooth-audio-cannot-find-bluetooth/m-p/648160) (Intel Customer Support technician reply on Bluetooth audio stutter: switch Wi-Fi to 5 GHz (no asker confirmation shown))

## No sound / no output device

### No sound and "No audio output device is installed" (red X on speaker icon)

`noout-no-device-installed` · win10, win11

**Symptoms**

- Speaker icon in the taskbar shows a red X
- Hovering the speaker icon says "No audio output device is installed"
- Settings > System > Sound says "No output devices found"
- No sound from any app

**Causes**

- The playback device was disabled in Sound settings or Device Manager
- The audio driver is missing, corrupted, or not loading
- A driver installed by Windows Update broke the audio device
- The Windows Audio or Windows Audio Endpoint Builder service is stopped

**Fix 1: Run the Windows audio troubleshooter** (verified, risk: low)

1. On Windows 11, open the Get Help app and run the Audio troubleshooter, or go to Settings > System > Sound and select Troubleshoot under the output device.
2. On Windows 10, go to Settings > Update & Security > Troubleshoot > Playing Audio and select Run the troubleshooter.
3. Apply any fixes it suggests, then test the sound.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft troubleshooting page, Windows 10 and 11) · [dell.com](https://www.dell.com/support/kbdoc/en-in/000179019/how-to-fix-persistent-audio-issues-on-your-dell-computer) (Official Dell KB, step 8)

**Fix 2: Re-enable a disabled playback device** (verified, risk: low, admin)

1. Press Win + R, type mmsys.cpl, and press Enter.
2. On the Playback tab, right-click an empty area and select Show Disabled Devices.
3. Right-click your speakers or headphones, select Enable, then select Set as Default Device and OK.
4. If the device is not listed, open Device Manager, expand Sound, video and game controllers, right-click the audio device and select Enable device if that option is shown.

```powershell
Get-PnpDevice -Class MEDIA | Where-Object { $_.ConfigManagerErrorCode -eq 22 } | Enable-PnpDevice -Confirm:$false
```

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-missing-or-undetected-audio-output-device-in-windows-5504aed3-2c01-4214-89d1-9e8dbe6828e8) (Official Microsoft page, Solution 1: Enable device in Device Manager) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/a/6690896) (Microsoft Community Support Specialist answer, 100+ helpful votes; question asked 800+ times)

**Fix 3: Restart the Windows audio services** (verified, risk: low, admin)

1. Press Win + R, type services.msc, and press Enter.
2. Right-click Windows Audio Endpoint Builder and select Restart.
3. Right-click Windows Audio and select Restart, or Start if it is stopped.
4. Make sure both services have Startup type set to Automatic.

```powershell
Restart-Service -Name AudioEndpointBuilder -Force; Start-Service -Name Audiosrv
```

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: restart Windows Audio, Windows Audio Endpoint Builder and RPC) · [techcommunity.microsoft.com](https://techcommunity.microsoft.com/t5/windows-10/reinstate-audio-function-without-reinstalling-windows/m-p/2445398) (Microsoft Tech Community thread, reply marked as solution (step 2: services running and Automatic))

**Fix 4: Uninstall the audio device and let Windows reinstall it** (verified, risk: medium, admin, create a restore point first)

1. Right-click Start and open Device Manager.
2. Expand Sound, video and game controllers.
3. Right-click your audio device and select Uninstall device.
4. Select Attempt to remove the driver for this device (Windows 11) or Delete the driver software for this device (Windows 10), then select Uninstall.
5. Restart the PC so Windows reinstalls the device. If it does not reappear, open Device Manager and select Action > Scan for hardware changes.
6. If Windows does not install a working driver, install the latest audio driver from your PC or motherboard maker's support site.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page, uninstall driver step) · [intel.com](https://www.intel.com/content/www/us/en/support/articles/000030990.html) (Official Intel support article for "no audio output device is installed" after Windows update) · [dell.com](https://www.dell.com/support/kbdoc/en-in/000179019/how-to-fix-persistent-audio-issues-on-your-dell-computer) (Official Dell KB, step 4)

**Fix 5: Switch to the Windows built-in High Definition Audio Device driver** (verified, risk: medium, admin)

1. Open Device Manager and expand Sound, video and game controllers.
2. Right-click the audio device and select Update driver.
3. Select Browse my computer for drivers, then Let me pick from a list of available drivers on my computer.
4. Select High Definition Audio Device and select Next. Confirm the warning if shown.
5. Restart the PC and test the sound. Note that maker-specific features such as audio tuning apps may stop working with this driver.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: use the Windows-supplied generic audio driver / select High Definition Audio Device) · [intel.com](https://www.intel.com/content/www/us/en/support/articles/000030990.html) (Official Intel article: Update driver > Let me pick from a list > choose audio driver) · [techcommunity.microsoft.com](https://techcommunity.microsoft.com/discussions/windowsinsiderprogram/windows-11-sound-problem/2539526) (User reply: "I tried loading the generic Microsoft driver and problem solved" (0 likes))

**Fix 6: Roll back the audio driver after a Windows Update** (verified, risk: medium, admin)

1. Open Device Manager and expand Sound, video and game controllers.
2. Right-click the audio device and select Properties.
3. On the Driver tab, select Roll Back Driver and follow the prompts.
4. Restart the PC. If Roll Back Driver is greyed out, use System Restore to a restore point from before the update.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: roll back driver if audio broke after Windows Update) · [techcommunity.microsoft.com](https://techcommunity.microsoft.com/discussions/windowsinsiderprogram/windows-11-sound-problem/2539526) (User reply (Lenovo IdeaPad 3): rolling back the Realtek driver fixed it)

### Speaker icon looks normal and volume meters move, but nothing is heard

`noout-silent-device-present` · win10, win11

**Symptoms**

- The speaker icon has no red X
- The Volume mixer or Sound control panel shows the level meter moving
- No sound comes out of the speakers or headphones

**Causes**

- Sound is going to a different output, such as a monitor without speakers over HDMI or DisplayPort
- The device, the app, or the physical speakers are muted or turned down
- Audio enhancements from the driver are silencing or breaking the output

**Fix 1: Choose the right output device and check mute and app volume** (verified, risk: low)

1. Select the speaker icon in the taskbar and confirm your speakers or headphones are selected as the output.
2. Open Settings > System > Sound and pick the correct output device. Monitors without built-in speakers can appear as an output device.
3. Make sure the device is not muted and the volume is turned up, including the physical volume on the speakers.
4. Open the Volume mixer and make sure the app you are using is not muted and its slider is up.
5. Check that cables are in the correct jack and that the speakers are powered on. Unplug headphones in case they are taking the sound.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: check output device, hardware and volume, set default device (USB/HDMI/monitors without speakers)) · [dell.com](https://www.dell.com/support/kbdoc/en-in/000179019/how-to-fix-persistent-audio-issues-on-your-dell-computer) (Official Dell KB, step 6: confirm correct output device, review App volume and device preferences)

**Fix 2: Turn off audio enhancements for the output device** (verified, risk: low)

1. On Windows 11, open Settings > System > Sound, select your output device, and set Audio enhancements to Off.
2. On Windows 10, open Control Panel > Hardware and Sound > Sound, select the default playback device, select Properties, and on the Enhancements tab check Disable all enhancements (or Disable all sound effects).
3. Test the sound. If it does not help, turn enhancements back on.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page, step: Turn off audio enhancements) · [techcommunity.microsoft.com](https://techcommunity.microsoft.com/discussions/windowsinsiderprogram/windows-11-sound-problem/2539526) (User replies: disabling all audio enhancements / turning off sound effects fixed no sound (0 likes))

**Fix 3: Restart the Windows audio services** (verified, risk: low, admin)

1. Press Win + R, type services.msc, and press Enter.
2. Right-click Windows Audio Endpoint Builder and select Restart.
3. Right-click Windows Audio and select Restart, or Start if it is stopped.

```powershell
Restart-Service -Name AudioEndpointBuilder -Force; Start-Service -Name Audiosrv
```

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: restart audio services) · [techcommunity.microsoft.com](https://techcommunity.microsoft.com/discussions/windowsinsiderprogram/windows-11-sound-problem/2539526) (User reply: resetting the Windows Audio service worked (0 likes))

## Default device and switching

### An app keeps using the wrong output device

`default-app-routing-stuck` · win10, win11

**Symptoms**

- One app (for example a browser or music player) plays through a different device than the rest of Windows
- An app does not follow when I change the output device from the taskbar
- System sounds work but one app plays on the wrong device or not at all

**Causes**

- The app has its own output device set in Volume mixer (App volume and device preferences) instead of Default
- Windows saved an old per-app device assignment that no longer matches the device you use

**Fix 1: Set the app's output device back to Default in Volume mixer** (verified, risk: low)

1. Start playing sound in the affected app so it appears in the list.
2. On Windows 11, open Start > Settings > System > Sound > Volume mixer. On Windows 10, open Start > Settings > System > Sound > Advanced sound options (App volume and device preferences).
3. Find the app under Apps and set its Output device to Default, or to the device you want.
4. Make sure the app is not muted and its volume slider is raised.
5. Close the app fully and open it again.

Sources: [support.microsoft.com](https://support.microsoft.com/topic/c2ebf6ed-4768-4ce5-8cb0-4cf195f5c431) (Official Microsoft page 'Fix app audio not working while system sounds work in Windows': confirm the correct Output device for the app in Volume mixer) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/2387521/edge-switching-from-default-sound-output-device-to) (Microsoft Q&A moderator answer; OP confirms setting Edge back to Default in App volume and device preferences restores playback (can recur after the next device change))

**Fix 2: Reset sound devices and volumes for all apps** (strong, risk: low)

1. On Windows 11, open Start > Settings > System > Sound > Volume mixer. On Windows 10, open Start > Settings > System > Sound > Advanced sound options.
2. Next to Reset sound devices and volumes for all apps to the recommended defaults, select Reset.
3. Choose your output device again and set any app volumes you had customized.

Sources: [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/1337541/ap-volume-and-device-properties) (Microsoft Q&A answer gives the Reset path; not accepted and OP reported it did not solve his specific case) · [support.lenovo.com](https://support.lenovo.com/au/en/solutions/ht514384) (Lenovo support HT514384 'How to reset the sound to the Microsoft recommended defaults' (System > Sound > Volume mixer). Content read from search excerpt; direct fetch returned 403)

### Audio device is disabled or hidden and does not appear in the sound list

`default-device-disabled-hidden` · win10, win11

**Symptoms**

- My speakers or headphones are missing from the output list in Sound settings
- I clicked Don't allow or Disable on a sound device and now I have no sound
- The output drop-down is empty or cannot be changed

**Causes**

- The audio endpoint was disabled in the Sound control panel or set to Don't allow in Settings
- The audio device was disabled in Device Manager
- The Sound control panel hides disabled devices by default, so they look missing

**Fix 1: Allow the device again in Sound settings** (verified, risk: low)

1. Open Start > Settings > System > Sound.
2. Select the device (under Output, or under All sound devices on Windows 11) to open its properties.
3. On Windows 11, select Allow so the device is enabled. On Windows 10, make sure the Disable check box is cleared.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft troubleshooting page, Step 5: check devices have not been disabled by mistake) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/4321330/re-enable-audio-device) (Microsoft staff answer (Mosken_L - MSFT), 100+ helpful votes, for a device set to Don't Allow on Windows 11)

**Fix 2: Show disabled devices in the Sound control panel and enable the device** (verified, risk: low)

1. Press Windows + R, type mmsys.cpl and press Enter.
2. On the Playback tab (or the Recording tab for a microphone), right-click an empty area and select Show Disabled Devices.
3. Right-click the greyed-out device and select Enable.
4. If it should be your main device, right-click it again and select Set as Default Device.
5. Select OK.

Sources: [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/4321330/re-enable-audio-device) (Microsoft staff answer (Mosken_L - MSFT), 100+ helpful votes; exact mmsys.cpl Show Disabled Devices > Enable steps) · [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft troubleshooting page, Step 5 (enable device) and Step 6 (Set as Default Device in Sound control panel))

**Fix 3: Enable the audio device in Device Manager and scan for hardware changes** (verified, risk: low, admin)

1. Open Device Manager from the Start menu.
2. Expand Sound, video and game controllers.
3. If your audio device is listed, right-click it and select Enable device if that option appears.
4. If it is not listed, open the View menu and select Show hidden devices.
5. Open the Action menu and select Scan for hardware changes.

```powershell
Get-PnpDevice -Class MEDIA -Status ERROR | Enable-PnpDevice -Confirm:$false; pnputil /scan-devices
```

Sources: [support.microsoft.com](https://support.microsoft.com/en-au/windows/fix-missing-or-undetected-audio-output-device-in-windows-5504aed3-2c01-4214-89d1-9e8dbe6828e8) (Official Microsoft page: Enable device, Show hidden devices, Scan for hardware changes) · [techcommunity.microsoft.com](https://techcommunity.microsoft.com/discussions/windows11/windows-11-stops-recognizing-monitors-as-audio-output-devices-when-idle/3587354) (Microsoft Tech Community thread; user reports Scan for hardware changes re-added the missing monitor output (may not persist after sleep))

### Sound plays from the wrong device or the default output switches by itself

`default-output-switches-on-device-change` · win10, win11

**Symptoms**

- Sound comes out of the monitor, TV or dock instead of my speakers or headphones
- The default output changes after the PC wakes from sleep, the monitor turns off and on, or a device is plugged in
- I set my speakers as default but Windows keeps picking another device after a restart

**Causes**

- An HDMI or DisplayPort monitor, TV or dock exposes its own audio device, and Windows makes it the default when it is connected or detected again
- When displays sleep and wake, the monitor audio device disappears and reappears, so Windows falls back to another output
- The preferred device was never set as the default (or default communication) device in the Sound control panel

**Fix 1: Set your speakers or headphones as the default device** (verified, risk: low)

1. Press Windows + R, type mmsys.cpl and press Enter.
2. On the Playback tab, right-click the device you want to use and select Set as Default Device.
3. Right-click the same device again and select Set as Default Communication Device if that option is available.
4. Select Apply, then OK.
5. On Windows 11 you can also go to Start > Settings > System > Sound and choose the device under Output; selecting it also makes it the default.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft troubleshooting page; Step 6 gives Set as Default Device (Win10) and Output selection sets default (Win11)) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/3865004/how-do-i-stop-windows-from-automatically-switching) (Microsoft Q&A, top 'Most helpful' answer (3 helpful votes) gives mmsys.cpl > Set as Default Device; not marked accepted) · [dell.com](https://dell.com/support/kbdoc/tr-tr/000125484/no-sound-with-displayport-or-hdmi-video-cable-connection?lang=en) (Dell KB 000125484: Windows may change the default playback device when a DP/HDMI cable is connected or on resume from sleep; set the wanted device as default. Content read from search excerpt; direct fetch failed)

**Fix 2: Disable the monitor or TV audio device you do not use** (strong, risk: low)

1. Press Windows + R, type mmsys.cpl and press Enter.
2. On the Playback tab, right-click the monitor, TV or display audio device you do not use (often named after the monitor, or NVIDIA/AMD/Intel High Definition Audio) and select Disable.
3. Select OK.
4. To undo, right-click an empty area of the Playback tab, select Show Disabled Devices, right-click the device and select Enable.
5. Do not disable a device that you use for sound, such as TV speakers you listen through.

Sources: [dell.com](https://dell.com/support/kbdoc/tr-tr/000125484/no-sound-with-displayport-or-hdmi-video-cable-connection?lang=en) (Dell KB 000125484: the external audio device causing the switch can be disabled so the issue does not occur again. Content read from search excerpt; direct fetch failed) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/4321330/re-enable-audio-device) (Microsoft staff answer (Mosken_L - MSFT), 100+ helpful votes; documents the mmsys.cpl Show Disabled Devices / Enable path used to reverse a disable)

## Drivers (Realtek, Intel SST, Conexant)

### Realtek or other onboard audio driver stopped working (Code 10 or no output device) after an update

`drv-audio-driver-broken-after-update` · win10, win11

**Symptoms**

- Sound stopped working right after a Windows Update or a driver update
- Device Manager shows a yellow warning on the audio device with "This device cannot start. (Code 10)"
- The sound icon says "No audio output device is installed"

**Causes**

- A newer audio driver delivered by Windows Update or the PC maker is not compatible with this PC
- The audio driver package is damaged or only partly installed
- A generic or other vendor's Realtek driver replaced the PC maker's customized driver

**Fix 1: Roll back the audio driver to the previous version** (verified, risk: medium, admin)

1. Open Device Manager.
2. Expand Sound, video and game controllers.
3. Right-click the audio device (for example Realtek(R) Audio) and select Properties.
4. Select the Driver tab, then select Roll Back Driver. This button is available only if an earlier driver is stored on the PC.
5. Restart the PC and test the sound.

Sources: [support.microsoft.com](https://support.microsoft.com/en-gb/windows/fix-audio-stops-working-after-a-windows-update-in-windows-b0dadfe1-051e-4e3f-a971-486ab4f7e6d6) (Official Microsoft support article; Roll Back Driver listed as fix when audio broke after an update) · [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft troubleshooting page, step 9: roll back if audio worked before Windows Update) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/2758897/how-to-enable-generic-audio-driver) (Microsoft Q&A answers giving the same rollback and uninstall steps; asker reports rollback replaced the driver (not marked accepted))

**Fix 2: Uninstall the audio device and let Windows reinstall the driver** (verified, risk: medium, admin, create a restore point first)

1. Open Device Manager and expand Sound, video and game controllers.
2. Right-click the audio device and select Uninstall device.
3. On Windows 11, select the Attempt to remove the driver for this device check box. On Windows 10, select Delete the driver software for this device. Then select Uninstall.
4. Restart the PC. Windows reinstalls an audio driver automatically.
5. If the device does not come back, open Device Manager and select Action > Scan for hardware changes.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft troubleshooting page, step 9: uninstall with driver removal, restart to reinstall) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/2758897/how-to-enable-generic-audio-driver) (Microsoft Q&A answer: uninstall with delete driver software, then Scan for hardware changes) · [dell.com](https://www.dell.com/community/Latitude/Intel-R-Smart-Sound-Technology-Intel-R-SST-OED-error-Code-10-on/m-p/7709628) (Dell Community thread marked Solved: uninstall with delete driver software, restart, then install Dell's Realtek driver; several users report audio restored)

**Fix 3: Install the audio driver package from the PC or motherboard maker** (verified, risk: medium, admin)

1. Find the exact model of the PC, laptop, or motherboard.
2. Go to the maker's support site (for example Dell, HP, Lenovo, ASUS, MSI) and download the audio driver for that model and Windows version.
3. Run the maker's installer as administrator and follow the instructions.
4. Restart the PC and test the sound.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/topic/error-codes-in-device-manager-in-windows-524e9e89-4dee-8883-0afa-6bca0456324e) (Official Microsoft Device Manager error code reference: for Code 10, update the driver or download the latest driver from the hardware vendor) · [intel.com](https://www.intel.com/content/www/us/en/support/articles/000089706.html) (Intel support article: generic driver versions may be incompatible with OEM-customized systems; use drivers from the OEM) · [dell.com](https://www.dell.com/community/Latitude/Intel-R-Smart-Sound-Technology-Intel-R-SST-OED-error-Code-10-on/m-p/7709628) (Dell Community thread marked Solved; accepted reply and other users restored audio by installing Dell's Realtek driver)

**Fix 4: Switch to the generic Microsoft High Definition Audio Device driver** (verified, risk: medium, admin)

1. Open Device Manager and expand Sound, video and game controllers.
2. Right-click the audio device and select Update driver.
3. Select Browse my computer for drivers, then Let me pick from a list of available drivers on my computer.
4. Select High Definition Audio Device, then select Next. Accept the compatibility warning if one appears.
5. Restart the PC and test the sound. Note: vendor apps such as Realtek Audio Console and OEM sound effects stop working with the generic driver.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft troubleshooting page, step 9: try the generic audio driver that comes with Windows (High Definition Audio Device)) · [techcommunity.microsoft.com](https://techcommunity.microsoft.com/t5/windows-10/how-do-i-get-rid-of-realtek-audio-and-go-back-to-high-definition/m-p/4564780) (Microsoft Tech Community reply giving the same steps and warning not to force an incompatible driver (no user confirmation))

### Blue screen or blocked Windows 11 upgrade caused by Intel SST driver 10.29.0.5152 or 10.30.0.5152

`drv-intel-sst-bsod-11thgen` · win10, win11

**Symptoms**

- A PC with an Intel 11th Gen Core processor shows a blue screen on Windows 11
- Windows Update does not offer the Windows 11 upgrade on a Windows 10 PC with an 11th Gen Intel processor

**Causes**

- Intel Smart Sound Technology Audio Controller driver IntcAudioBus.sys version 10.29.0.5152 or 10.30.0.5152 is incompatible with Windows 11

**Fix 1: Update the Intel SST driver to version 10.29.00.5714 / 10.30.00.5714 or later** (verified, risk: medium, admin)

1. Open Settings > Windows Update and select Check for updates. Install any driver updates offered.
2. If no newer Intel SST driver is offered, download the Intel SST Audio Controller driver for your model from the PC maker's support site and install it.
3. Restart the PC.
4. Check in Device Manager > System devices that the Intel SST Audio Controller driver version is 10.29.00.5714 or later, or 10.30.00.5714 or later.
5. Wait up to 48 hours for the Windows 11 upgrade to be offered. Do not force the upgrade before the driver is updated.

Sources: [learn.microsoft.com](https://learn.microsoft.com/en-us/windows/release-health/known-issues/known-issue-2746) (Official Microsoft release health known issue (Resolved External, 2024-04-12) with affected and fixed versions) · [intel.com](https://www.intel.com/content/www/us/en/support/articles/000089706.html) (Intel support article: get Intel SST driver updates from the OEM because generic versions may be incompatible)

### Intel Smart Sound Technology (SST) audio controller shows Code 10 and there is no sound

`drv-intel-sst-code10` · win10, win11

**Symptoms**

- No sound on a laptop after a system, driver, or BIOS update
- Device Manager > System devices shows Intel(R) Smart Sound Technology (Intel(R) SST) Audio Controller or Intel SST OED with a yellow warning and Code 10
- The sound icon says "No audio output device is installed"

**Causes**

- The Intel SST driver does not match the Realtek or Conexant audio driver and the BIOS shipped by the PC maker
- A generic Intel SST driver was installed over the PC maker's customized driver

**Fix 1: Install the matching Intel SST and audio drivers from the PC maker** (verified, risk: medium, admin)

1. Find the exact model of the laptop.
2. Go to the PC maker's support site and download the audio driver package and, if listed, the Intel SST driver and latest BIOS for that model.
3. Install the BIOS update first if one is offered, then the Intel SST driver, then the audio driver, following the maker's instructions.
4. Restart the PC and test the sound.
5. If no matching driver is available, contact the PC maker's support.

Sources: [intel.com](https://www.intel.com/content/www/us/en/support/articles/000089706.html) (Intel support article for Code 10/Code 31 after SST update: get drivers from the OEM) · [learn.microsoft.com](https://learn.microsoft.com/en-us/windows/release-health/known-issues/known-issue-2746) (Microsoft release health: install newer Intel SST Audio Controller drivers via Windows Update or from the device maker) · [dell.com](https://www.dell.com/community/Latitude/Intel-R-Smart-Sound-Technology-Intel-R-SST-OED-error-Code-10-on/m-p/7709628) (Dell Community thread marked Solved: Dell's Realtek driver package cleared the SST Code 10 for several users)

**Fix 2: Switch the Intel SST Audio Controller to the generic High Definition Audio Controller driver** (verified, risk: medium, admin)

1. Open Device Manager and expand System devices.
2. Right-click Intel(R) Smart Sound Technology (Intel(R) SST) Audio Controller and select Update driver.
3. Select Browse my computer for drivers, then Let me pick from a list of available drivers on my computer.
4. Select High Definition Audio Controller and install it.
5. Restart the PC and test the sound. If the SST OED device still shows Code 10, install the PC maker's driver package instead.

Sources: [intel.com](https://www.intel.com/content/www/us/en/support/articles/000058422.html) (Intel support article: no audio with Code 10 on Intel SST after a system, driver, or BIOS update; fix is selecting High Definition Audio Controller) · [dell.com](https://www.dell.com/community/Latitude/Intel-R-Smart-Sound-Technology-Intel-R-SST-OED-error-Code-10-on/m-p/7709628) (Dell Community thread: original poster reports selecting High Definition Audio Controller fixed both affected Latitude laptops)

## HDMI, DisplayPort and USB audio

### Monitor or TV sound stops after the display sleeps or is turned off

`hdmi-audio-lost-after-display-sleep` · win10, win11

**Symptoms**

- Sound from the monitor or TV stops after the screen turns off and back on
- The monitor or TV disappears from the output list after the display sleeps
- Sound switches to another device after the display wakes up
- Restarting the PC brings the sound back

**Causes**

- When the display turns off, Windows treats its HDMI/DisplayPort audio as unavailable and switches to another output
- The display audio device is not detected again when the display wakes
- Graphics or display audio driver problems with power events

**Fix 1: Select the monitor or TV as the output again** (strong, risk: low)

1. Make sure the monitor or TV is turned on.
2. Open Settings > System > Sound and choose the monitor or TV under Output.
3. If it is not listed, try the next fix.

Sources: [techcommunity.microsoft.com](https://techcommunity.microsoft.com/discussions/windows11/windows-11-stops-recognizing-monitors-as-audio-output-devices-when-idle/3587354) (Microsoft Tech Community thread; users report Windows switches to another output when displays sleep) · [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: choose the HDMI device as the default output)

**Fix 2: Make Windows detect the display audio device again** (strong, risk: low, admin)

1. Make sure the monitor or TV is turned on.
2. Open Device Manager and choose Action > Scan for hardware changes.
3. Select the monitor or TV under Settings > System > Sound > Output.

```powershell
pnputil /scan-devices
```

Sources: [techcommunity.microsoft.com](https://techcommunity.microsoft.com/discussions/windows11/windows-11-stops-recognizing-monitors-as-audio-output-devices-when-idle/3587354) (User reports Scan for hardware changes restores monitor audio after idle; described as a temporary fix) · [forums.tomsguide.com](https://forums.tomsguide.com/threads/connected-my-laptop-to-my-tv-through-hdmi-cable-but-cant-get-any-sounds.373904/post-1642799) (Post marked as the thread Solution: scan for hardware changes revealed NVIDIA High Definition Audio and the TV)

**Fix 3: Update the graphics and display audio driver** (strong, risk: medium, admin)

1. Download the current driver for your graphics card from NVIDIA, AMD, Intel or your PC maker.
2. Install it, including the HD Audio component, and restart the PC with the display on.
3. Test whether sound survives the display turning off and on again.

Sources: [techcommunity.microsoft.com](https://techcommunity.microsoft.com/discussions/windows11/windows-11-stops-recognizing-monitors-as-audio-output-devices-when-idle/3587354) (User reports 'Now its working' after updating NVIDIA graphics and audio drivers) · [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: update the audio driver or get it from the manufacturer)

### No sound from HDMI/DisplayPort monitor or TV, or it is missing from the output list

`hdmi-display-audio-missing-or-silent` · win10, win11

**Symptoms**

- Picture shows on the monitor or TV but no sound comes from it
- The monitor or TV (NVIDIA/AMD High Definition Audio or Intel Display Audio) is not listed under Sound > Output
- The HDMI device shows as Disabled or Not plugged in in the Sound control panel

**Causes**

- Windows is sending sound to a different default output device
- The HDMI/DisplayPort audio endpoint is disabled or hidden
- The monitor has no speakers, or its own volume or input is muted
- The graphics card's display audio device was not detected or its driver is missing or outdated

**Fix 1: Select the monitor or TV as the output device** (verified, risk: low)

1. On Windows 11, open Settings > System > Sound and choose the monitor or TV under Output.
2. On Windows 10, open Control Panel > Hardware and Sound > Sound, right-click the monitor or TV on the Playback tab and choose Set as Default Device.
3. Make sure the volume is not at zero.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: set a USB or HDMI audio device as the default output) · [samsung.com](https://www.samsung.com/us/support/troubleshoot/TSG10001504) (Samsung official troubleshooting page for no sound from HDMI device, step 1) · [forums.tomsguide.com](https://forums.tomsguide.com/threads/connected-my-laptop-to-my-tv-through-hdmi-cable-but-cant-get-any-sounds.373904/post-1642799) (Two later replies confirm 'Setting to default worked for me')

**Fix 2: Check that the monitor or TV has speakers and is not muted** (verified, risk: low)

1. Check that the monitor or TV has built-in speakers and supports audio over HDMI or DisplayPort.
2. Turn up the volume and turn off mute using the monitor's or TV's own buttons or remote.
3. If the monitor has no speakers, choose your speakers or headphones as the output device instead.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: an external monitor without speakers set as default gives no audio) · [samsung.com](https://www.samsung.com/us/support/troubleshoot/TSG10001504) (Samsung official page notes some monitors have no speakers or do not support HDMI audio)

**Fix 3: Show and enable a disabled HDMI/DisplayPort audio device** (verified, risk: low)

1. Open Settings > System > Sound and select More sound settings (Windows 11) or Sound Control Panel (Windows 10).
2. On the Playback tab, right-click an empty area and check Show Disabled Devices and Show Disconnected Devices.
3. If the monitor or TV appears greyed out, right-click it and choose Enable.
4. Right-click it again and choose Set as Default Device.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: check the device is enabled (Use this device / Disable checkbox)) · [samsung.com](https://www.samsung.com/us/support/troubleshoot/TSG10001504) (Samsung official page step 3: show disconnected devices and enable the HDMI device)

**Fix 4: Make Windows detect the display audio device again** (strong, risk: low, admin)

1. Open Device Manager.
2. Select Sound, video and game controllers, then choose Action > Scan for hardware changes.
3. Check that NVIDIA High Definition Audio, AMD High Definition Audio or Intel Display Audio now appears.
4. Set the monitor or TV as the default output device.

```powershell
pnputil /scan-devices
```

Sources: [forums.tomsguide.com](https://forums.tomsguide.com/threads/connected-my-laptop-to-my-tv-through-hdmi-cable-but-cant-get-any-sounds.373904/post-1642799) (Post marked as the thread Solution; poster confirms the TV then played sound) · [techcommunity.microsoft.com](https://techcommunity.microsoft.com/discussions/windows11/windows-11-stops-recognizing-monitors-as-audio-output-devices-when-idle/3587354) (Microsoft Tech Community thread; user reports Scan for hardware changes brings the monitor audio back)

**Fix 5: Update or reinstall the graphics driver that provides HDMI/DisplayPort audio** (verified, risk: medium, admin)

1. Run Windows Update and install any display or audio driver updates.
2. Download the current driver for your graphics card from NVIDIA, AMD, Intel or your PC maker.
3. Run the installer and make sure the HD Audio driver component is included.
4. Restart the PC with the monitor or TV turned on and connected.
5. If sound stopped after a driver update, open Device Manager, right-click the display audio device, choose Properties > Driver > Roll Back Driver instead.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: update, reinstall or roll back the audio driver; check the manufacturer's site) · [samsung.com](https://www.samsung.com/us/support/troubleshoot/TSG10001504) (Samsung official page step 5: get HDMI audio drivers from the graphics card maker (NVIDIA, AMD or Intel)) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/5849029/missing-hdmi-output) (Microsoft Q&A advisor answer (not marked accepted): missing display audio means a graphics audio driver problem; clean-install the graphics driver)

### USB headset, DAC or microphone cuts out, disconnects or is not detected

`usb-audio-dropouts-power-saving` · win10, win11

**Symptoms**

- USB headset or microphone stops working after a few minutes of silence
- USB DAC or audio interface disconnects and reconnects at random
- USB audio device is not recognized after the PC wakes from sleep until a restart
- Sound cuts out when the device is plugged into a USB hub

**Causes**

- USB selective suspend lets Windows power down the USB port to save power
- Power management on the USB Root Hub turns off the hub the device is connected to
- USB hubs or extenders that cannot supply enough power or interfere with the connection

**Fix 1: Plug the device directly into a different USB port** (verified, risk: low)

1. Unplug the USB headset, DAC or microphone from any hub, extender or front-panel port.
2. Plug it directly into a USB port on the PC (on a desktop, a rear port on the motherboard).
3. Select it under Settings > System > Sound and test again.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: try a different USB port for USB speakers or headphones) · [support.rationalacoustics.com](https://support.rationalacoustics.com/support/solutions/articles/150000091757--windows-audio-device-disconnects-or-stops-working-after-inactivity-within-smaart) (Rational Acoustics official support: avoid USB extenders and multi-port hubs for audio interfaces)

**Fix 2: Turn off USB selective suspend** (verified, risk: low)

1. Open Control Panel > Hardware and Sound > Power Options.
2. Select Change plan settings for the active plan, then Change advanced power settings.
3. Expand USB settings > USB selective suspend setting.
4. Set it to Disabled. On a laptop, set both On battery and Plugged in.
5. Select Apply, then OK.

```powershell
powercfg /setacvalueindex SCHEME_CURRENT 2a737441-1930-4402-8d77-b2bebba308a3 48e6b7a6-50f5-4782-a5d4-53bb8f07e226 0; powercfg /setdcvalueindex SCHEME_CURRENT 2a737441-1930-4402-8d77-b2bebba308a3 48e6b7a6-50f5-4782-a5d4-53bb8f07e226 0; powercfg /setactive SCHEME_CURRENT
```

Sources: [support.logi.com](https://support.logi.com/hc/en-ca/articles/360023347833) (Logitech official support: devices such as headsets stop responding; disable USB selective suspend) · [support.m-audio.com](https://support.m-audio.com/en/support/solutions/articles/69000851050-m-audio-my-usb-powered-device-disconnects-randomly-from-my-computer-what-do-i-do-windows-) (M-Audio official support: USB device disconnects randomly; disable USB selective suspend) · [pugetsystems.com](https://www.pugetsystems.com/labs/support-software/How-to-Disable-USB-Selective-Suspend-1190/) (Puget Systems support article (2018): random USB disconnects and devices not waking from sleep)

**Fix 3: Stop Windows from turning off USB Root Hubs to save power** (verified, risk: low, admin)

1. Open Device Manager and expand Universal Serial Bus controllers.
2. Right-click USB Root Hub and choose Properties.
3. On the Power Management tab, clear Allow the computer to turn off this device to save power, then select OK.
4. Repeat for every USB Root Hub, or use View > Devices by connection to find the hub and host controller above the audio device.
5. Restart the PC.

Sources: [support.logi.com](https://support.logi.com/hc/en-ca/articles/360023347833) (Logitech official support: uncheck the USB Root Hub power-saving option) · [support.rationalacoustics.com](https://support.rationalacoustics.com/support/solutions/articles/150000091757--windows-audio-device-disconnects-or-stops-working-after-inactivity-within-smaart) (Rational Acoustics official support: audio device disconnects after inactivity; disable root hub and host controller power saving) · [pugetsystems.com](https://www.pugetsystems.com/labs/support-software/How-to-Disable-USB-Selective-Suspend-1190/) (Puget Systems support article: disable power management on all root hubs if problems persist)

## Crackling, latency and sound quality

### Sound crackles, pops or sounds distorted on speakers or headphones

`qual-crackling-enhancements-format` · win10, win11

**Symptoms**

- Music, videos or system sounds crackle, pop or sound distorted
- Audio sounds harsh or 'fuzzy' even at normal volume
- Crackling started after a driver or Windows update

**Causes**

- Audio enhancements (driver effects such as loudness, bass boost or vendor audio suites) are distorting the signal
- The default audio format (sample rate and bit depth) does not suit the device or driver
- The audio driver is outdated or damaged

**Fix 1: Turn off audio enhancements for the output device** (verified, risk: low)

1. Windows 11: open Settings > System > Sound, select your device under Output, and set Audio enhancements to Off.
2. Windows 10: right-click the volume icon, select Sounds, open the Playback tab, select your device and choose Properties.
3. On the Enhancements tab, select Disable all enhancements, then select OK.
4. If there is no Enhancements tab, open the Advanced tab and clear Enable audio enhancements.
5. Play audio again and check whether the crackling is gone.

Sources: [support.microsoft.com](https://support.microsoft.com/windows/fix-distorted-or-crackling-audio-in-windows-5304e452-38a6-4f3b-83cd-664beb3e68aa) (Official Microsoft 'Fix distorted or crackling audio in Windows' page; first listed fix) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/a/7161555) (Microsoft Q&A community answer (not accepted) marked helpful by 10+ people; poster reports disabling enhancements, exclusive mode and hardware acceleration stopped the crackling)

**Fix 2: Try a different default audio format** (verified, risk: low)

1. Windows 11: open Settings > System > Sound, select your device under Output, and select Advanced.
2. Windows 10: right-click the volume icon, select Sounds, select your device on the Playback tab, choose Properties and open the Advanced tab.
3. Under Default format, choose a different format, such as 24 bit, 48000 Hz or 16 bit, 48000 Hz.
4. Select OK and test your audio. Try one format at a time.

Sources: [support.microsoft.com](https://support.microsoft.com/windows/fix-distorted-or-crackling-audio-in-windows-5304e452-38a6-4f3b-83cd-664beb3e68aa) (Official Microsoft page; 'Change the default audio format' fix) · [support.native-instruments.com](https://support.native-instruments.com/support/solutions/articles/69000880009-windows-tuning-tips-for-audio-processing) (Native Instruments official Windows tuning guide; says mismatched sample rates between Windows and audio software cause serious problems)

**Fix 3: Update or reinstall the audio driver** (verified, risk: medium, admin)

1. Open Device Manager from the Start menu.
2. Expand Sound, video and game controllers, right-click your audio device and select Update driver.
3. Restart the PC and test the audio.
4. If the problem continues, install the latest audio driver from your PC, motherboard or audio interface manufacturer's website.

Sources: [support.microsoft.com](https://support.microsoft.com/windows/fix-distorted-or-crackling-audio-in-windows-5304e452-38a6-4f3b-83cd-664beb3e68aa) (Official Microsoft page; 'Update or reinstall the audio driver' fix) · [support.enginedj.com](https://support.enginedj.com/support/solutions/articles/69000803869-troubleshooting-dpc-latency) (Engine DJ (inMusic) official support article; lists installing the newest audio driver as a quick fix for crackles and dropouts)

### Clicks, pops or dropouts caused by high DPC latency from another driver

`qual-dpc-latency` · win10, win11

**Symptoms**

- Audio clicks, pops or drops out at random, even with enhancements off
- Glitches happen when downloading, using Wi-Fi, or when the GPU is busy
- LatencyMon says the system is not suitable for real-time audio

**Causes**

- A network, Wi-Fi, graphics, storage, USB or ACPI driver holds the CPU too long (high DPC/ISR latency)
- Outdated chipset, BIOS or device drivers

**Fix 1: Install Windows updates and update chipset, network and graphics drivers** (verified, risk: medium, admin)

1. Install all available updates in Windows Update and restart.
2. Install the latest chipset, network (Wi-Fi and Ethernet) and graphics drivers from your PC or motherboard manufacturer's website.
3. Check Device Manager for devices with a warning icon and update their drivers.
4. Restart and test your audio.

Sources: [support.native-instruments.com](https://support.native-instruments.com/support/solutions/articles/69000880009-windows-tuning-tips-for-audio-processing) (Native Instruments official guide; driver-by-driver table (ndis.sys, nvlddmkm.sys, storport.sys etc.) recommending updates) · [support.enginedj.com](https://support.enginedj.com/support/solutions/articles/69000803869-troubleshooting-dpc-latency) (Engine DJ (inMusic) official DPC latency article; update Windows, network and flagged drivers)

**Fix 2: Temporarily disable Wi-Fi or network adapters to test for network-driver latency** (verified, risk: low, admin)

1. Open Device Manager and expand Network adapters.
2. Right-click the Wi-Fi adapter and select Disable device. Do not uninstall it. You will lose internet access while it is disabled.
3. Play audio and check whether the clicks and dropouts stop.
4. If they stop, re-enable the adapter and install the newest network driver from the manufacturer.
5. Right-click the adapter and select Enable device to undo.

```powershell
Get-NetAdapter -Physical | Where-Object { $_.Status -eq 'Up' -and $_.MediaType -eq 'Native 802.11' } | Disable-NetAdapter -Confirm:$false
```

Sources: [support.native-instruments.com](https://support.native-instruments.com/support/solutions/articles/69000880009-windows-tuning-tips-for-audio-processing) (Native Instruments official guide; for ndis.sys/tcpip.sys latency, disable network adapters while using audio and re-enable afterwards) · [support.enginedj.com](https://support.enginedj.com/support/solutions/articles/69000803869-troubleshooting-dpc-latency) (Engine DJ (inMusic) official article; disable Wi-Fi in Device Manager and compare latency with it off and on)

### Crackling or dropouts caused by power-saving settings

`qual-power-saving-crackling` · win10, win11

**Symptoms**

- Audio crackles or drops out, often on a laptop or on battery
- Crackling is worse when the PC is idle or lightly loaded
- Crackling started after switching to a Balanced or Power saver plan

**Causes**

- The CPU drops into low-power states or low clock speeds and the audio buffer runs empty
- USB selective suspend powers down a USB audio device or hub
- Laptop vendor power utilities override Windows power settings

**Fix 1: Switch to the High performance power plan** (verified, risk: low)

1. Open Control Panel > System and Security > Power Options.
2. Select High performance. If it is not shown, select Show additional plans first.
3. Test your audio.
4. To undo, select your previous plan (usually Balanced) again.

```powershell
powercfg /setactive SCHEME_MIN
```

Sources: [support.native-instruments.com](https://support.native-instruments.com/support/solutions/articles/69000880009-windows-tuning-tips-for-audio-processing) (Native Instruments official Windows tuning guide; High Performance plan listed for crackles and dropouts) · [support.enginedj.com](https://support.enginedj.com/support/solutions/articles/69000803869-troubleshooting-dpc-latency) (Engine DJ (inMusic) official support article; recommends High Performance power plan on laptops)

**Fix 2: Raise minimum processor state and turn off USB selective suspend** (verified, risk: low)

1. Open Control Panel > Power Options and select Change plan settings for the active plan.
2. Select Change advanced power settings.
3. Set Processor power management > Minimum processor state to 100%.
4. Set USB settings > USB selective suspend setting to Disabled.
5. Select OK and test your audio. This uses more power and can make a laptop run warmer.

Sources: [support.native-instruments.com](https://support.native-instruments.com/support/solutions/articles/69000880009-windows-tuning-tips-for-audio-processing) (Native Instruments official guide; gives these exact advanced power settings (minimum processor state 100%, USB selective suspend Disabled)) · [support.enginedj.com](https://support.enginedj.com/support/solutions/articles/69000803869-troubleshooting-dpc-latency) (Engine DJ (inMusic) official support article; advises disabling aggressive power-saving features for dropouts)

### Distortion or crackles when the Windows sample rate does not match the app or audio interface

`qual-sample-rate-mismatch` · win10, win11

**Symptoms**

- Audio crackles or sounds wrong only in one music, DAW or recording app
- Sound is distorted after changing the sample rate in an audio app or interface
- Pitch or speed sounds slightly off

**Causes**

- The Windows default format (for example 44100 Hz) differs from the rate used by the app or audio interface (for example 48000 Hz)

**Fix 1: Set the Windows default format to the same sample rate as your app or interface** (verified, risk: low)

1. Find the sample rate your audio app or interface uses (often 44100 Hz or 48000 Hz).
2. Windows 11: open Settings > System > Sound, select your device, and select Advanced. Windows 10: open Sounds > Playback > device Properties > Advanced.
3. Under Default format, select the format with the same sample rate.
4. Select OK, restart the audio app, and test.

Sources: [support.native-instruments.com](https://support.native-instruments.com/support/solutions/articles/69000880009-windows-tuning-tips-for-audio-processing) (Native Instruments official guide; 'Match the sample rate' across Windows and audio software) · [support.microsoft.com](https://support.microsoft.com/windows/fix-distorted-or-crackling-audio-in-windows-5304e452-38a6-4f3b-83cd-664beb3e68aa) (Official Microsoft page; documents changing Default format to fix distorted or crackling audio)

## Microphone

### Headset microphone not working on a laptop's combined headphone/mic jack

`mic-combo-jack-headset` · win10, win11

**Symptoms**

- Headset audio plays, but Windows still uses the laptop's built-in microphone
- The headset microphone never appears as an input device
- No pop-up asks what kind of device was plugged in

**Causes**

- The jack is set to Headphones instead of Headset (with microphone) in the Realtek or Waves MaxxAudio app
- On Dell laptops, the Waves MaxxAudio service that handles jack detection is not running
- The headset uses a plug that is not compatible with the combo jack (needs a 4-pole TRRS plug)

**Fix 1: Set the jack to Headset (with microphone) in the audio app** (verified, risk: low)

1. Plug the headset into the combo jack.
2. If a pop-up asks what device you plugged in, choose Headset (not Headphones) and select OK; do not select 'Never show this dialog again'.
3. If no pop-up appears, open Realtek Audio Console from Start, select Device advanced settings, and under Connector Retasking (Analog) choose Headset or 'Headset (with microphone)'.
4. On Dell laptops with Waves MaxxAudio, check that the Waves MaxxAudio Service Application is enabled, restart with the headset unplugged, then plug it in and choose the headset in the prompt.
5. Open Settings > System > Sound and pick the headset microphone under Input, then test it.

Sources: [dell.com](https://www.dell.com/support/kbdoc/en-us/000137121/inspiron-notebooks-using-realtek-audio-driver-do-not-recognize-headset-microphone) (Dell KB: enable Waves MaxxAudio service, reboot unplugged, select correct device in prompt) · [dell.com](https://www.dell.com/community/en/conversations/inspiron/realtek-audio-headphone-mic-not-detected/647f8fe5f4ccf8a8de1085e8) (Dell Community: Connector Retasking 'Headset (with microphone)' fix confirmed by a reply ('This helped me'); thread not marked solved) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/4106968/how-to-use-headphones-mic-instead-of-laptop-built) (Microsoft Q&A answer with 5 helpful votes (not accepted): Realtek Audio Console > Device advanced settings > Connector Retasking > Headset)

### Microphone works in one app but not another, or its level changes by itself

`mic-exclusive-mode-conflict` · win10, win11

**Symptoms**

- The microphone works in one app but another app hears nothing or says the device is in use
- The microphone stops working in other apps while a call or game is open
- The microphone input level moves up or down on its own

**Causes**

- One app has taken exclusive control of the microphone
- Apps are allowed to change the microphone level through exclusive mode
- Desktop apps are blocked in privacy settings while Store apps are allowed

**Fix 1: Make sure desktop apps are allowed to use the microphone** (verified, risk: low)

1. Open Settings > Privacy & security > Microphone.
2. Turn on Let desktop apps access your microphone.
3. Restart the app that cannot use the microphone.

```powershell
Start-Process 'ms-settings:privacy-microphone'
```

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-microphone-problems-5f230348-106d-bfa4-1db5-336f35576011) (Official Microsoft troubleshooting page) · [dell.com](https://www.dell.com/support/kbdoc/en-us/000133024) (Dell KB on microphone privacy settings)

### Microphone volume too low or others can barely hear me

`mic-level-too-low` · win10, win11

**Symptoms**

- People on calls say my voice is very quiet
- The input level bar barely moves when I speak
- Recordings are much quieter than expected

**Causes**

- Input volume slider or Levels setting is low
- Microphone Boost is set to 0 dB on mics that support it
- Audio enhancements on the input device lower or process the signal

**Fix 1: Raise the input volume and Microphone Boost** (verified, risk: low)

1. Open Settings > System > Sound and select your microphone under Input.
2. Raise the Input volume slider, then use Start test to check the level.
3. For more control, press Win+R, run mmsys.cpl, open the Recording tab, and open the microphone's Properties.
4. On the Levels tab, set Microphone to 100 and, if a Microphone Boost slider exists, raise it in small steps (for example +10 dB).
5. Lower the boost again if you hear hiss or distortion.

```powershell
control mmsys.cpl,,1
```

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-microphone-problems-5f230348-106d-bfa4-1db5-336f35576011) (Official Microsoft page: 'Volume is too low' section, Levels tab Microphone and Microphone Boost sliders) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/3848626/why-is-my-mic-sound-so-low) (Microsoft Q&A, answer accepted by question author; asker confirmed mic returned to usual levels) · [dell.com](https://www.dell.com/support/contents/en-ae/article/product-support/self-support-knowledgebase/audio-and-speakers/microphone) (Dell self-support guide: raise Input slider in small steps to avoid distortion)

**Fix 2: Turn off audio enhancements for the microphone** (verified, risk: low)

1. Open Settings > System > Sound and select the microphone under Input.
2. Turn off Audio enhancements if the option is shown.
3. Or run mmsys.cpl, open the Recording tab, open the microphone's Properties, and on the Enhancements or Advanced tab disable all enhancements.
4. Test the microphone again.

Sources: [dell.com](https://www.dell.com/support/contents/en-ae/article/product-support/self-support-knowledgebase/audio-and-speakers/microphone) (Dell self-support guide: turn off audio enhancements in device properties) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/3848626/why-is-my-mic-sound-so-low) (Microsoft Q&A, answer accepted by question author; asker confirmed)

### Microphone not detected or not listed as an input device

`mic-not-detected` · win10, win11

**Symptoms**

- No microphone appears under Sound > Input
- Windows says no input devices were found
- The microphone is plugged in but the input level bar never moves

**Causes**

- A different input device is set as the default
- The microphone endpoint is disabled and hidden in the classic Sound control panel
- The audio or microphone driver is missing or corrupted

**Fix 1: Select the microphone as the input device and test it** (verified, risk: low)

1. Check that the microphone or headset mute button is off and the cable or Bluetooth connection is in place.
2. Open Settings > System > Sound.
3. Under Input, choose your microphone under 'Choose a device for speaking or recording' (Windows 10: 'Choose your input device').
4. Open the device page and select Start test, speak, then stop the test to check the level.

```powershell
Start-Process 'ms-settings:sound'
```

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-microphone-problems-5f230348-106d-bfa4-1db5-336f35576011) (Official Microsoft troubleshooting page) · [dell.com](https://www.dell.com/support/contents/en-ae/article/product-support/self-support-knowledgebase/audio-and-speakers/microphone) (Dell self-support microphone guide: select input device under Sound > Input)

**Fix 2: Run the recording audio troubleshooter** (verified, risk: low)

1. Windows 11: open Settings > System > Sound, scroll to Advanced, and select Input devices under 'Troubleshoot common sound problems'.
2. Windows 10: open Settings > Update & Security > Troubleshoot > Additional troubleshooters and run Recording Audio.
3. Select the microphone that is not working and follow the prompts.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-microphone-problems-5f230348-106d-bfa4-1db5-336f35576011) (Official Microsoft troubleshooting page) · [dell.com](https://www.dell.com/support/contents/en-ae/article/product-support/self-support-knowledgebase/audio-and-speakers/microphone) (Dell self-support microphone guide: Sound > Advanced > Input devices troubleshooter)

**Fix 3: Uninstall the audio or microphone device and let Windows reinstall it** (verified, risk: medium, admin)

1. Right-click Start and select Device Manager.
2. Expand Sound, video and game controllers (for USB microphones, also Audio inputs and outputs).
3. Right-click the audio device or microphone and select Uninstall device, then select Uninstall.
4. Restart the PC so Windows reinstalls the device.
5. If it does not come back, right-click the category in Device Manager and select Scan for hardware changes.
6. If it still fails, right-click the device and select Update driver, or install the driver from the PC or microphone maker.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-microphone-problems-5f230348-106d-bfa4-1db5-336f35576011) (Official Microsoft troubleshooting page, 'Windows doesn't detect the mic' section) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/3848626/why-is-my-mic-sound-so-low) (Microsoft Q&A, answer accepted by question author; asker confirmed the steps worked) · [dell.com](https://www.dell.com/support/contents/en-ae/article/product-support/self-support-knowledgebase/audio-and-speakers/microphone) (Dell self-support microphone guide: update driver under Audio inputs and outputs)

### Apps cannot use the microphone because Windows privacy settings block access

`mic-privacy-access-blocked` · win10, win11

**Symptoms**

- The microphone works in Sound settings, but apps say no microphone is found or access is denied
- Desktop apps such as Teams, Zoom, Discord or a browser cannot hear me
- The microphone stopped working in apps after an update or a privacy reset

**Causes**

- Microphone access is turned off for the whole device
- 'Let apps access your microphone' is turned off, or the specific Store app is turned off
- 'Let desktop apps access your microphone' is turned off, which blocks all classic desktop apps

**Fix 1: Turn on microphone access for Windows, apps and desktop apps** (verified, risk: low)

1. Open Settings > Privacy & security > Microphone (Windows 10: Settings > Privacy > Microphone).
2. Turn on Microphone access (Windows 10: select Change and turn on access for this device).
3. Turn on Let apps access your microphone, and turn on each app that needs the microphone.
4. Scroll down and turn on Let desktop apps access your microphone.
5. If the microphone is built into a webcam, also check Settings > Privacy & security > Camera.
6. Restart the app that could not use the microphone and test again.

```powershell
Start-Process 'ms-settings:privacy-microphone'
```

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-microphone-problems-5f230348-106d-bfa4-1db5-336f35576011) (Official Microsoft troubleshooting page (Windows 10 and 11 steps)) · [dell.com](https://www.dell.com/support/kbdoc/en-us/000133024) (Dell KB 'Microphone Not Working Due to Privacy Settings in Windows 11 or Windows 10') · [dell.com](https://www.dell.com/support/contents/en-ae/article/product-support/self-support-knowledgebase/audio-and-speakers/microphone) (Dell self-support microphone guide; notes desktop apps cannot be allowed individually)

## Audio services and Windows Update

### Windows Audio services are stopped or "not responding"

`svc-audio-services-not-running` · win10, win11

**Symptoms**

- The speaker icon in the taskbar shows a red X
- A message says "The audio service is not running"
- The sound troubleshooter reports "Audio services not responding"
- Starting Windows Audio in Services fails with "Error 1068: The dependency service or group failed to start"
- No sound from speakers or headphones

**Causes**

- The Windows Audio (Audiosrv) or Windows Audio Endpoint Builder (AudioEndpointBuilder) service stopped or failed to start
- A service that Windows Audio depends on (such as Windows Audio Endpoint Builder or Remote Procedure Call) is not running
- The audio services are set to Manual or Disabled instead of Automatic
- The audio device driver is damaged or does not match the hardware, so the services cannot work with it
- Windows system files used by the audio services are damaged

**Fix 1: Run the built-in audio troubleshooter (Get Help)** (verified, risk: low)

1. Windows 11: open Settings > System > Troubleshoot > Other troubleshooters and select Run next to Audio. On Windows 11 22H2 and later this opens the Get Help audio troubleshooter.
2. Windows 10: open Settings > Update & Security > Troubleshoot > Additional troubleshooters and run Playing Audio.
3. Follow the prompts and apply any fix it offers, then test the sound.

```powershell
Start-Process 'ms-settings:troubleshoot'
```

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft sound troubleshooting page; first step is the Get Help audio troubleshooter, which 'will automatically run diagnostics and attempt to fix most audio problems') · [support.microsoft.com](https://support.microsoft.com/en-us/windows/deprecation-of-microsoft-support-diagnostic-tool-msdt-and-msdt-troubleshooters-0c5ac9a2-1600-4539-b9d0-069e71f9040a) (Official Microsoft page: the legacy Audio troubleshooter is redirected to Get Help for Audio; launch it from Settings > System > Troubleshoot > Other troubleshooters) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/1693134/how-to-fix-audio-service-not-responding-ive-used-a) (Microsoft Q&A MVP answer (S.Sengupta) recommends the audio troubleshooter for 'Audio service not responding'; not accepted)

**Fix 2: Restart the audio services and set them to start automatically** (verified, risk: low, admin)

1. Press Windows + R, type services.msc and press Enter.
2. Double-click Windows Audio Endpoint Builder, set Startup type to Automatic, select Apply, then Start (or right-click it and select Restart if it is already running).
3. Do the same for Windows Audio.
4. If Windows Audio fails with Error 1068, make sure Remote Procedure Call (RPC) and Windows Audio Endpoint Builder are running, then start Windows Audio again.
5. Restart the PC and test the sound.

```powershell
Set-Service -Name AudioEndpointBuilder -StartupType Automatic; Set-Service -Name Audiosrv -StartupType Automatic; Restart-Service -Name AudioEndpointBuilder -Force; Start-Service -Name Audiosrv
```

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: 'Restart Windows audio services' (Windows Audio, Windows Audio Endpoint Builder, Remote Procedure Call)) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/a/2613323) (Microsoft Q&A answer marked accepted by the asker: start Windows Audio, set Startup type to Automatic, and on Error 1068 start its dependency services (Windows Audio Endpoint Builder among them)) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/5834198/audio-services-not-responding-no-recommended-fixes) (Microsoft External Staff moderator answer gives the Set-Service / Restart-Service commands for AudioEndpointBuilder and Audiosrv; not accepted, asker did not confirm)

**Fix 3: Uninstall the audio device and reinstall the PC maker's driver** (verified, risk: medium, admin)

1. Open Device Manager and expand Sound, video and game controllers.
2. Right-click the audio device (for example Realtek Audio) and select Uninstall device.
3. Select Uninstall. Leave 'Attempt to remove the driver for this device' cleared unless you already have the PC maker's driver package ready.
4. Restart the PC so Windows reinstalls the driver.
5. If sound is still missing, download and install the audio driver for your exact model from the PC or motherboard maker's support site, then restart.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: uninstall the device in Device Manager and restart, Windows reinstalls the driver; or get the driver from the device maker) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/9ebc6690-8c45-44c9-beb1-e3cfc4fcc8e2/audio-services-not-responding-windows-11) (Microsoft Q&A accepted answer (10+ helpful votes) for 'Audio services not responding' after restarting services failed: uninstall the audio driver and install the PC maker's driver; asker replied 'That did the trick')

**Fix 4: Repair damaged Windows system files with DISM and SFC** (verified, risk: low, admin)

1. Open Command Prompt or PowerShell as administrator.
2. Run DISM.exe /Online /Cleanup-image /Restorehealth and wait for it to finish.
3. Run sfc /scannow and keep the window open until it reaches 100%.
4. Restart the PC and test the sound.
5. If SFC says it could not fix some files, see the Microsoft System File Checker page for next steps.

```powershell
DISM.exe /Online /Cleanup-image /Restorehealth; sfc /scannow
```

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/topic/use-the-system-file-checker-tool-to-repair-missing-or-corrupted-system-files-79aa86cb-ca52-166a-92a3-966e85d4094e) (Official Microsoft procedure: run DISM RestoreHealth first, then sfc /scannow, from an elevated prompt) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/5558559/audio-services-not-responding) (Microsoft Q&A Independent Advisor answer for 'Audio services not responding' runs DISM and SFC before restarting the audio services; not accepted, and the asker reported no change)

### Sound stopped working after a Windows Update

`svc-no-sound-after-windows-update` · win10, win11

**Symptoms**

- Sound worked before a Windows Update and stopped right after it
- "No audio output device is installed" after an update
- The audio driver changed or disappeared after an update

**Causes**

- The update has a known issue that Microsoft lists on the Windows release health pages
- Windows Update replaced the PC maker's audio driver with one that does not work on this device
- The update removed or broke the audio driver

**Fix 1: Check Microsoft's known issues and install the latest update** (verified, risk: low)

1. Open the Windows release health page for your Windows version (for example 'Windows 11, version 24H2 known issues') and look for an audio or sound issue tied to the update you installed.
2. If the issue is listed as resolved, open Settings > Windows Update, select Check for updates, install everything offered, and restart.
3. If the issue lists a workaround, follow it until a fix is released.

Sources: [learn.microsoft.com](https://learn.microsoft.com/en-us/windows/release-health/status-windows-11-24h2) (Official release health page; lists audio known issues (USB Audio Class 1.0 Code 10, AC-3 decoding) with status, workaround and the update that resolves them) · [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft sound page: check for updates, install, and restart to apply pending updates)

**Fix 2: Reinstall the audio driver from the PC maker** (verified, risk: medium, admin)

1. Download the audio driver for your exact model from the PC or motherboard maker's support site.
2. Open Device Manager, expand Sound, video and game controllers, right-click the audio device and select Uninstall device.
3. Restart the PC, then install the driver you downloaded and restart again.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/fix-sound-or-audio-problems-in-windows-73025246-b61c-40fb-671a-2535c7cd56c8) (Official Microsoft page: uninstall/reinstall the driver or download it from the device maker) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/5834975/no-sound-after-update) (Microsoft Q&A, answer accepted by the asker; sound drivers removed by an update (KB5079473) and the asker resolved it by reinstalling the drivers)

**Fix 3: Uninstall the update that broke sound (temporary)** (verified, risk: medium, admin)

1. Windows 11: open Settings > Windows Update > Update history > Uninstall updates.
2. Windows 10: open Settings > Update & Security > Windows Update > View update history > Uninstall updates.
3. Select the update installed just before sound stopped and select Uninstall, then restart.
4. Pause updates for a short time and reinstall the update once Microsoft lists a fix. Removing a security update leaves the PC less protected, and not all updates can be removed.

Sources: [support.microsoft.com](https://support.microsoft.com/en-us/windows/windows-update-faq-8a903416-6f45-0718-f5c7-375e92dddeb2) (Official Microsoft steps to uninstall an update on Windows 11 and Windows 10; Microsoft notes it does not recommend removing updates and that not all updates can be removed) · [learn.microsoft.com](https://learn.microsoft.com/en-us/answers/questions/5834975/no-sound-after-update) (Asker reports that uninstalling KB5079473 brought the sound drivers back; the accepted answer later resolved it by reinstalling drivers)

### USB headset or DAC shows Code 10 or has no sound after the September 2026 update

`svc-usb-audio-code10-sept-2026-update` · win10, win11

**Symptoms**

- A USB headset, USB speaker or USB DAC stopped working after the September 8, 2026 update
- Device Manager shows "This device cannot start (Code 10)" on the USB audio device
- Volume controls do nothing or stay at zero
- Audio works in stereo but fails in 8-channel or 3D/spatial audio mode

**Causes**

- Known issue in the September 8, 2026 Windows security update (KB5124008 on Windows 11 24H2/25H2) affecting USB Audio Class 1.0 devices

**Fix 1: Install the September 14, 2026 out-of-band update or later** (verified, risk: low)

1. Open Settings > Windows Update and select Check for updates.
2. Install all updates offered (KB5129195 or a later cumulative update on Windows 11 24H2/25H2) and restart.
3. Note: Microsoft says this fixes the 8-channel / 3D audio failure only; Code 10 and no-output symptoms were still under investigation as of 2026-10-28.

Sources: [learn.microsoft.com](https://learn.microsoft.com/en-us/windows/release-health/status-windows-11-24h2) (Official release health entry 'USB audio devices might fail to start or produce no sound', status Mitigated; partially resolved by KB5129195 and later) · [support.microsoft.com](https://support.microsoft.com/help/5129195) (Official KB5129195 release notes: fixes USB Audio Class 1.0 devices failing in multichannel modes; installs automatically via Windows Update)

**Fix 2: Switch the USB audio device to 2-channel stereo** (verified, risk: low)

1. Turn off spatial / 3D audio for the device (Settings > System > Sound > the device > Spatial audio: Off).
2. Open Control Panel > Sound, select the USB device on the Playback tab, select Configure, choose Stereo and finish the wizard.
3. Test the sound.

Sources: [learn.microsoft.com](https://learn.microsoft.com/en-us/windows/release-health/status-windows-11-24h2) (Official release health entry: 'Some customers have reported that they're able to restore audio in these cases by switching to 2-channel mode') · [support.microsoft.com](https://support.microsoft.com/help/5129195) (Official KB page confirms the failure is limited to multichannel modes while stereo works)
