<#
.SYNOPSIS
    Read-only scan for common Windows sound problems.

.DESCRIPTION
    Inspects audio services, Core Audio endpoints, Bluetooth audio, audio
    drivers and microphone privacy settings. Changes nothing on the system.
    Each check id matches detection.checkIds in data/issues.json.

    Runs on Windows PowerShell 5.1 and PowerShell 7.

.PARAMETER AsJson
    Emit a JSON report (used by the Shabda app) instead of a table.

.EXAMPLE
    .\Diagnose.ps1
.EXAMPLE
    .\Diagnose.ps1 -AsJson
#>
[CmdletBinding()]
param([switch]$AsJson)

Set-StrictMode -Version 2.0
$ErrorActionPreference = 'Stop'

Add-Type -TypeDefinition @'
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace ShabdaDiag {
  [StructLayout(LayoutKind.Sequential)]
  public struct PropertyKey { public Guid fmtid; public int pid; }

  [StructLayout(LayoutKind.Sequential)]
  public struct PropVariant { public ushort vt; ushort r1, r2, r3; public IntPtr p; IntPtr p2; }

  [ComImport, Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  interface IPropertyStore {
    [PreserveSig] int GetCount(out int c); [PreserveSig] int GetAt(int i, out PropertyKey k);
    [PreserveSig] int GetValue(ref PropertyKey k, out PropVariant v);
  }

  [ComImport, Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  interface IMMDeviceEnumerator {
    [PreserveSig] int EnumAudioEndpoints(int flow, int mask, out IMMDeviceCollection c);
    [PreserveSig] int GetDefaultAudioEndpoint(int flow, int role, out IMMDevice d);
  }

  [ComImport, Guid("0BD7A1BE-7A1A-44DB-8397-CC5392387B5E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  interface IMMDeviceCollection { [PreserveSig] int GetCount(out int n); [PreserveSig] int Item(int i, out IMMDevice d); }

  [ComImport, Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  interface IMMDevice {
    [PreserveSig] int Activate(ref Guid iid, int ctx, IntPtr p, [MarshalAs(UnmanagedType.IUnknown)] out object o);
    [PreserveSig] int OpenPropertyStore(int access, out IPropertyStore s);
    [PreserveSig] int GetId([MarshalAs(UnmanagedType.LPWStr)] out string id);
    [PreserveSig] int GetState(out int s);
  }

  [ComImport, Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  interface IAudioEndpointVolume {
    [PreserveSig] int f0(); [PreserveSig] int f1(); [PreserveSig] int f2(); [PreserveSig] int f3(); [PreserveSig] int f4(); [PreserveSig] int f5();
    [PreserveSig] int GetMasterVolumeLevelScalar(out float level);
    [PreserveSig] int f7(); [PreserveSig] int f8(); [PreserveSig] int f9(); [PreserveSig] int f10(); [PreserveSig] int f11();
    [PreserveSig] int GetMute([MarshalAs(UnmanagedType.Bool)] out bool mute);
  }

  [ComImport, Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")] class MMDeviceEnumerator { }

  public class Endpoint {
    public string Id; public string Name; public string Flow; public int State;
    public bool IsDefault; public bool? Muted; public float? Volume;
  }

  public static class CoreAudio {
    static PropertyKey FriendlyName = new PropertyKey {
      fmtid = new Guid("a45c254e-df1c-4efd-8020-67d146a850e0"), pid = 14 };

    static string DefaultId(IMMDeviceEnumerator e, int flow) {
      IMMDevice d; string id = null;
      if (e.GetDefaultAudioEndpoint(flow, 0, out d) == 0) d.GetId(out id);
      return id;
    }

    public static List<Endpoint> List() {
      var result = new List<Endpoint>();
      var e = (IMMDeviceEnumerator)new MMDeviceEnumerator();
      for (int flow = 0; flow < 2; flow++) {
        string def = DefaultId(e, flow);
        IMMDeviceCollection c; int n;
        e.EnumAudioEndpoints(flow, 0xF, out c); c.GetCount(out n);
        for (int i = 0; i < n; i++) {
          IMMDevice d; c.Item(i, out d);
          var ep = new Endpoint { Flow = flow == 0 ? "render" : "capture" };
          d.GetId(out ep.Id); d.GetState(out ep.State);
          ep.IsDefault = ep.Id == def;
          IPropertyStore s; PropVariant v;
          if (d.OpenPropertyStore(0, out s) == 0 && s.GetValue(ref FriendlyName, out v) == 0 && v.vt == 31)
            ep.Name = Marshal.PtrToStringUni(v.p);
          if (ep.State == 1) {
            var iid = typeof(IAudioEndpointVolume).GUID; object o;
            if (d.Activate(ref iid, 1, IntPtr.Zero, out o) == 0) {
              var vol = (IAudioEndpointVolume)o; float lv; bool m;
              if (vol.GetMasterVolumeLevelScalar(out lv) == 0) ep.Volume = lv;
              if (vol.GetMute(out m) == 0) ep.Muted = m;
            }
          }
          result.Add(ep);
        }
      }
      return result;
    }
  }
}
'@

$results = New-Object System.Collections.Generic.List[object]
function Add-Finding([string]$Id, [string]$Detail, [string[]]$Devices = @()) {
    $results.Add([pscustomobject]@{ id = $Id; detail = $Detail; devices = @($Devices) })
}

# Returns a device property's value, or $null if the device or property is
# missing. Plain (...).Data would throw under strict mode when nothing comes back.
function Get-DeviceProperty([string]$InstanceId, [string]$Key) {
    $prop = Get-PnpDeviceProperty -InstanceId $InstanceId -KeyName $Key -ErrorAction SilentlyContinue
    if ($prop) { $prop.Data } else { $null }
}

function Get-EndpointParent([string]$EndpointId) {
    Get-DeviceProperty "SWD\MMDEVAPI\$EndpointId" 'DEVPKEY_Device_Parent'
}

# --- Services -----------------------------------------------------------------
foreach ($name in 'AudioEndpointBuilder', 'Audiosrv') {
    $svc = Get-Service -Name $name -ErrorAction SilentlyContinue
    if (-not $svc) {
        Add-Finding 'svc-audio-not-running' "$name service is missing."
    } elseif ($svc.Status -ne 'Running' -or $svc.StartType -eq 'Disabled') {
        Add-Finding 'svc-audio-not-running' "$name is $($svc.Status) (start type $($svc.StartType))."
    }
}

# --- Endpoints ----------------------------------------------------------------
$endpoints = @()
try { $endpoints = @([ShabdaDiag.CoreAudio]::List()) }
catch { Add-Finding 'svc-audio-not-running' "Core Audio could not be queried: $($_.Exception.Message)" }

$render = @($endpoints | Where-Object Flow -eq 'render')
$capture = @($endpoints | Where-Object Flow -eq 'capture')
$activeRender = @($render | Where-Object State -eq 1)
$activeCapture = @($capture | Where-Object State -eq 1)

if ($endpoints.Count -gt 0 -and $activeRender.Count -eq 0) {
    Add-Finding 'noout-no-active-output' 'No playback device is active.'
}
if ($activeRender.Count -gt 0 -and -not ($render | Where-Object IsDefault)) {
    Add-Finding 'default-no-default-output' 'Playback devices exist but none is set as default.'
}
# Only playback: recording devices such as Stereo Mix are disabled by default.
$disabled = @($render | Where-Object State -eq 2)
if ($disabled.Count -gt 0) {
    Add-Finding 'default-device-disabled' 'Some playback devices are disabled.' ($disabled | ForEach-Object Name)
}
foreach ($ep in @($render | Where-Object { $_.IsDefault -and $_.State -eq 1 })) {
    if ($ep.Muted -or ($null -ne $ep.Volume -and $ep.Volume -lt 0.05)) {
        Add-Finding 'noout-default-muted' "Default playback device is muted or volume is near zero." @($ep.Name)
    }
}

# --- Bluetooth ----------------------------------------------------------------
foreach ($ep in $render) {
    $parent = Get-EndpointParent $ep.Id
    if (-not $parent) { continue }
    if ($parent -like 'BTHENUM\*' -and $ep.State -ne 1) {
        # A paired speaker that is switched off still shows status OK, so also
        # require the Bluetooth link to be up (DEVPKEY_Device_IsConnected).
        $connected = Get-DeviceProperty $parent '{83DA6326-97A6-4088-9453-A1923F573B29} 15'
        if ($connected -eq $true) {
            Add-Finding 'bt-endpoint-missing' 'Bluetooth audio device is paired and present, but its playback endpoint is not active.' @($ep.Name)
        }
    }
    if ($parent -like 'BTHHFENUM\*' -and $ep.IsDefault -and $ep.State -eq 1) {
        Add-Finding 'bt-handsfree-default' 'Default playback is a Bluetooth Hands-Free (call quality) endpoint, not Stereo/A2DP.' @($ep.Name)
    }
}

# --- Drivers ------------------------------------------------------------------
$media = @(Get-PnpDevice -Class MEDIA -PresentOnly -ErrorAction SilentlyContinue)
foreach ($dev in @($media | Where-Object Status -eq 'Error')) {
    $code = Get-DeviceProperty $dev.InstanceId 'DEVPKEY_Device_ProblemCode'
    if ($code -eq 22) {
        Add-Finding 'drv-device-disabled' 'Sound device is disabled in Device Manager.' @($dev.FriendlyName)
    } else {
        Add-Finding 'drv-audio-device-error' "Sound device reports a driver problem (Device Manager code $code)." @($dev.FriendlyName)
    }
}

# --- Microphone ---------------------------------------------------------------
$consent = 'Software\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone'
foreach ($root in 'HKLM:', 'HKCU:') {
    $item = Get-ItemProperty -Path "$root\$consent" -Name Value -ErrorAction SilentlyContinue
    if ($item -and $item.Value -eq 'Deny') {
        $scope = if ($root -eq 'HKLM:') { 'for all users' } else { 'for this user' }
        Add-Finding 'mic-privacy-blocked' "Microphone access is turned off in Privacy settings $scope."
    }
}
if ($endpoints.Count -gt 0 -and $activeCapture.Count -eq 0) {
    Add-Finding 'mic-no-active-input' 'No recording device is active.'
}
foreach ($ep in @($capture | Where-Object { $_.IsDefault -and $_.State -eq 1 -and $_.Muted })) {
    Add-Finding 'mic-default-muted' 'Default microphone is muted.' @($ep.Name)
}

# --- Output -------------------------------------------------------------------
$stateNames = @{ 1 = 'active'; 2 = 'disabled'; 4 = 'not present'; 8 = 'unplugged' }
$report = [pscustomobject]@{
    schemaVersion = 1
    scannedAt     = (Get-Date).ToString('o')
    os            = [System.Environment]::OSVersion.Version.ToString()
    findings      = $results.ToArray()
    endpoints     = @($endpoints | ForEach-Object {
        [pscustomobject]@{
            name = $_.Name; flow = $_.Flow; state = $stateNames[$_.State]
            isDefault = $_.IsDefault; muted = $_.Muted; volume = $_.Volume
        }
    })
}

if ($AsJson) {
    $report | ConvertTo-Json -Depth 5
} elseif ($results.Count -eq 0) {
    Write-Output 'No known sound problems found.'
} else {
    $results | Format-Table id, detail, @{ n = 'devices'; e = { $_.devices -join ', ' } } -AutoSize -Wrap
}
