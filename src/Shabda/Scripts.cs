using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Shabda;

/// <summary>
/// Loads the catalog and PowerShell scripts embedded in the app, and runs them.
/// Scripts are passed with -EncodedCommand, so nothing is written to disk where
/// another process could swap it before it runs elevated.
/// </summary>
public static class Scripts
{
    const string ResourcePrefix = "Shabda.Embedded.";

    public static string ReadResource(string name)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourcePrefix + name)
            ?? throw new FileNotFoundException($"Embedded resource {name} is missing.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static Catalog LoadCatalog() => Catalog.Parse(ReadResource("issues.json"));

    public static string? FixScript(string name) =>
        FixAction.IsValidScriptName(name) ? ReadResource($"fixes.{name}.ps1") : null;

    public static string Encode(string script, string arguments = "")
    {
        // Wrapping in a script block keeps the script's param() block working.
        var command = $"& {{\n{script}\n}} {arguments}";
        return Convert.ToBase64String(Encoding.Unicode.GetBytes(command));
    }

    static string PowerShellPath =>
        Path.Combine(Environment.SystemDirectory, @"WindowsPowerShell\v1.0\powershell.exe");

    static string BaseArguments(string encoded) =>
        $"-NoProfile -NonInteractive -ExecutionPolicy Bypass -EncodedCommand {encoded}";

    /// <summary>Runs a script as the current user and returns its standard output.</summary>
    public static async Task<string> RunAsync(string script, string arguments = "")
    {
        var psi = new ProcessStartInfo(PowerShellPath, BaseArguments(Encode(script, arguments)))
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
        };
        using var process = Process.Start(psi) ?? throw new InvalidOperationException("PowerShell did not start.");
        var stdout = process.StandardOutput.ReadToEndAsync();
        var stderr = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
            throw new InvalidOperationException($"Script failed ({process.ExitCode}): {await stderr}");
        return await stdout;
    }

    /// <summary>Runs a script elevated (UAC prompt) and returns its exit code.</summary>
    public static async Task<int> RunElevatedAsync(string script)
    {
        // Elevated output can't be redirected, so the exe makes PowerShell
        // report failure through its exit code.
        var wrapped = "$ErrorActionPreference = 'Stop'\ntry {\n" + script + "\n} catch { exit 1 }\nexit 0";
        var psi = new ProcessStartInfo(PowerShellPath, BaseArguments(Encode(wrapped)))
        {
            UseShellExecute = true,
            Verb = "runas",
            WindowStyle = ProcessWindowStyle.Hidden,
        };
        using var process = Process.Start(psi) ?? throw new InvalidOperationException("PowerShell did not start.");
        await process.WaitForExitAsync();
        return process.ExitCode;
    }
}

/// <summary>
/// The automation behind a catalog fix: "script:&lt;name&gt;" runs an embedded fix
/// script, "open:&lt;target&gt;" opens a Windows settings page or control panel.
/// </summary>
public sealed record FixAction(string Kind, string Target)
{
    static readonly HashSet<string> ControlPanels = ["mmsys.cpl"];

    public static FixAction? Parse(string? action)
    {
        if (action is null) return null;
        var parts = action.Split(':', 2);
        if (parts.Length != 2) return null;
        return parts[0] switch
        {
            "script" when IsValidScriptName(parts[1]) => new FixAction("script", parts[1]),
            "open" when IsAllowedOpenTarget(parts[1]) => new FixAction("open", parts[1]),
            _ => null,
        };
    }

    public static bool IsValidScriptName(string name) =>
        name.Length is > 0 and <= 64 && name.All(c => char.IsAsciiLetterLower(c) || char.IsAsciiDigit(c) || c == '-');

    static bool IsAllowedOpenTarget(string target) =>
        ControlPanels.Contains(target) ||
        (target.StartsWith("ms-settings:", StringComparison.Ordinal) &&
         target.Skip("ms-settings:".Length).All(c => char.IsAsciiLetterOrDigit(c) || c == '-'));

    public void Open()
    {
        var psi = Kind == "open" && ControlPanels.Contains(Target)
            ? new ProcessStartInfo(Path.Combine(Environment.SystemDirectory, "control.exe"), Target)
            : new ProcessStartInfo(Target) { UseShellExecute = true };
        Process.Start(psi);
    }
}
