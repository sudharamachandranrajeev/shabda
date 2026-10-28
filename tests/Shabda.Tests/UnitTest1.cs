namespace Shabda.Tests;

public class MatcherTests
{
    static readonly Source Ms = new("https://support.microsoft.com/a", "microsoft", "official");

    static Issue MakeIssue(string id, params string[] checkIds) => new(
        id, "bluetooth", "Title " + id, ["symptom"], ["cause"], ["win11"],
        new Detection(["signal"], [.. checkIds]),
        [new Fix("f", "Fix", ["step"], null, false, "low", true, false, "verified", [Ms])],
        "2026-10-28");

    [Fact]
    public void Match_links_findings_to_issues_by_check_id()
    {
        var catalog = new Catalog(1, "2026-10-28", [MakeIssue("a", "bt-endpoint-missing"), MakeIssue("b", "other")]);
        var report = new ScanReport(1, "", "", [new Finding("bt-endpoint-missing", "detail", ["Speaker"])]);

        var match = Assert.Single(Matcher.Match(catalog, report));

        Assert.Equal("a", Assert.Single(match.Issues).Id);
    }

    [Fact]
    public void Match_keeps_findings_without_catalog_entries_and_merges_duplicates()
    {
        var catalog = new Catalog(1, "2026-10-28", []);
        var report = new ScanReport(1, "", "", [
            new Finding("svc-audio-not-running", "Audiosrv stopped", []),
            new Finding("svc-audio-not-running", "AudioEndpointBuilder stopped", ["x"]),
        ]);

        var match = Assert.Single(Matcher.Match(catalog, report));

        Assert.Empty(match.Issues);
        Assert.Equal(["x"], match.Finding.Devices);
    }

    [Fact]
    public void Search_requires_every_word()
    {
        var catalog = new Catalog(1, "", [MakeIssue("a") with { Title = "Bluetooth speaker no sound" }, MakeIssue("b")]);

        Assert.Equal("a", Assert.Single(Matcher.Search(catalog, "speaker SOUND")).Id);
        Assert.Equal(2, Matcher.Search(catalog, " ").Count());
    }
}

public class FixActionTests
{
    [Theory]
    [InlineData("script:restart-audio-services", "script")]
    [InlineData("open:ms-settings:sound", "open")]
    [InlineData("open:mmsys.cpl", "open")]
    public void Parse_accepts_known_forms(string action, string kind) =>
        Assert.Equal(kind, FixAction.Parse(action)?.Kind);

    [Theory]
    [InlineData(null)]
    [InlineData("script:../evil")]
    [InlineData("script:Restart")]
    [InlineData("open:https://example.com")]
    [InlineData("open:ms-settings:sound&calc")]
    [InlineData("open:cmd.exe")]
    [InlineData("run:anything")]
    public void Parse_rejects_anything_else(string? action) => Assert.Null(FixAction.Parse(action));

    [Fact]
    public void Source_only_exposes_https_links()
    {
        Assert.NotNull(new Source("https://superuser.com/a/1", "stackexchange", "x").SafeUri);
        Assert.Null(new Source("javascript:alert(1)", "forum", "x").SafeUri);
        Assert.Null(new Source("http://example.com", "forum", "x").SafeUri);
    }
}

public class EmbeddedTests
{
    [Fact]
    public void Embedded_catalog_parses_and_every_action_resolves()
    {
        var catalog = Scripts.LoadCatalog();

        Assert.NotEmpty(catalog.Issues);
        foreach (var fix in catalog.Issues.SelectMany(i => i.Fixes).Where(f => f.Action is not null))
        {
            var action = FixAction.Parse(fix.Action);
            Assert.True(action is not null, $"{fix.Id}: invalid action {fix.Action}");
            if (action.Kind == "script") Assert.NotNull(Scripts.FixScript(action.Target));
        }
    }

    [Fact]
    public void Every_diagnose_check_is_explained_by_a_catalog_issue()
    {
        var checkIds = System.Text.RegularExpressions.Regex
            .Matches(Scripts.ReadResource("Diagnose.ps1"), @"Add-Finding '([a-z0-9-]+)'")
            .Select(m => m.Groups[1].Value)
            .ToHashSet();
        var covered = Scripts.LoadCatalog().Issues.SelectMany(i => i.Detection.CheckIds).ToHashSet();

        Assert.NotEmpty(checkIds);
        Assert.Empty(checkIds.Except(covered));
    }

    [Fact]
    public async Task Diagnose_runs_through_the_app_runner_and_parses()
    {
        var report = ScanReport.Parse(await Scripts.RunAsync(Scripts.ReadResource("Diagnose.ps1"), "-AsJson"));

        Assert.Equal(1, report.SchemaVersion);
        Assert.NotNull(report.Findings);
    }

    [Fact]
    public void Encoded_diagnose_fits_the_windows_command_line_limit() =>
        Assert.True(Scripts.Encode(Scripts.ReadResource("Diagnose.ps1"), "-AsJson").Length < 30000);
}
