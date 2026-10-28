using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shabda;

public sealed record Source(string Url, string Type, string Signal)
{
    public Uri? SafeUri =>
        Uri.TryCreate(Url, UriKind.Absolute, out var uri) && uri.Scheme == Uri.UriSchemeHttps ? uri : null;
}

public sealed record Fix(
    string Id,
    string Title,
    List<string> Steps,
    string? Command,
    bool RequiresAdmin,
    string Risk,
    bool Reversible,
    bool RequiresRestorePoint,
    string Confidence,
    List<Source> Sources,
    string? Action = null);

public sealed record Detection(List<string> Signals, List<string> CheckIds);

public sealed record Issue(
    string Id,
    string Category,
    string Title,
    List<string> Symptoms,
    List<string> Causes,
    List<string> Os,
    Detection Detection,
    List<Fix> Fixes,
    string LastVerified);

public sealed record Catalog(int SchemaVersion, string Generated, List<Issue> Issues)
{
    public static readonly JsonSerializerOptions Json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public static Catalog Parse(string json) =>
        JsonSerializer.Deserialize<Catalog>(json, Json) ?? throw new InvalidDataException("Empty catalog.");

    public static readonly IReadOnlyDictionary<string, string> CategoryNames = new Dictionary<string, string>
    {
        ["bluetooth"] = "Bluetooth",
        ["no-output"] = "No sound",
        ["default-device"] = "Default device",
        ["driver-realtek"] = "Drivers",
        ["hdmi-dp-usb"] = "HDMI, DisplayPort and USB",
        ["quality"] = "Crackling and quality",
        ["microphone"] = "Microphone",
        ["services-update"] = "Services and updates",
    };
}

public sealed record Finding(string Id, string Detail, List<string> Devices);

public sealed record ScanReport(int SchemaVersion, string ScannedAt, string Os, List<Finding> Findings)
{
    public static ScanReport Parse(string json) =>
        JsonSerializer.Deserialize<ScanReport>(json, Catalog.Json) ?? throw new InvalidDataException("Empty scan report.");
}

/// <summary>A scan finding together with the catalog issues that explain it.</summary>
public sealed record DetectedIssue(Finding Finding, IReadOnlyList<Issue> Issues);

public static class Matcher
{
    public static IReadOnlyList<DetectedIssue> Match(Catalog catalog, ScanReport report) =>
        report.Findings
            .GroupBy(f => f.Id)
            .Select(g => new DetectedIssue(
                g.First() with { Devices = g.SelectMany(f => f.Devices).Distinct().ToList() },
                catalog.Issues.Where(i => i.Detection.CheckIds.Contains(g.Key)).ToList()))
            .ToList();

    public static IEnumerable<Issue> Search(Catalog catalog, string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return catalog.Issues;
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return catalog.Issues.Where(i =>
        {
            var hay = string.Join(' ', [i.Title, .. i.Symptoms, .. i.Causes, i.Category]);
            return words.All(w => hay.Contains(w, StringComparison.OrdinalIgnoreCase));
        });
    }
}
