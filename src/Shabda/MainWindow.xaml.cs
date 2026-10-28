using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Shabda;

public sealed record ListItem(string Title, string Subtitle, Finding? Finding, IReadOnlyList<Issue> Issues);

public partial class MainWindow : Window
{
    readonly Catalog _catalog;
    IReadOnlyList<DetectedIssue>? _detected;

    public MainWindow()
    {
        InitializeComponent();
        _catalog = Scripts.LoadCatalog();
        Title += $"  ·  catalog {_catalog.Generated}, {_catalog.Issues.Count} issues";
    }

    async void Scan_Click(object sender, RoutedEventArgs e)
    {
        ScanButton.IsEnabled = false;
        StatusText.Text = "Scanning…";
        try
        {
            var json = await Scripts.RunAsync(Scripts.ReadResource("Diagnose.ps1"), "-AsJson");
            _detected = Matcher.Match(_catalog, ScanReport.Parse(json));
            StatusText.Text = _detected.Count == 0
                ? "No known problems found. If sound still fails, search All known issues for your symptom."
                : $"Found {_detected.Count} problem(s). Select one to see causes and fixes.";
            DetectedMode.IsChecked = true;
            Refresh();
        }
        catch (Exception ex)
        {
            StatusText.Text = "Scan failed: " + ex.Message;
        }
        finally
        {
            ScanButton.IsEnabled = true;
        }
    }

    void Mode_Changed(object sender, RoutedEventArgs e) => Refresh();
    void Search_Changed(object sender, TextChangedEventArgs e) => Refresh();

    void Refresh()
    {
        if (IssueList is null) return; // Fires during InitializeComponent.
        IEnumerable<ListItem> items;
        if (AllMode.IsChecked == true || !string.IsNullOrWhiteSpace(SearchBox.Text))
        {
            items = Matcher.Search(_catalog, SearchBox.Text).Select(i => new ListItem(
                i.Title, Catalog.CategoryNames.GetValueOrDefault(i.Category, i.Category), null, [i]));
        }
        else
        {
            items = (_detected ?? []).Select(d => new ListItem(
                d.Issues.FirstOrDefault()?.Title ?? d.Finding.Detail,
                d.Finding.Devices.Count > 0 ? string.Join(", ", d.Finding.Devices) : d.Finding.Detail,
                d.Finding, d.Issues));
        }
        IssueList.ItemsSource = items.ToList();
        Detail.Children.Clear();
    }

    void IssueList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Detail.Children.Clear();
        if (IssueList.SelectedItem is not ListItem item) return;

        if (item.Finding is { } finding)
        {
            Add(Heading("What the scan found", 20));
            Add(Text(finding.Detail));
            if (finding.Devices.Count > 0) Add(Text("Devices: " + string.Join(", ", finding.Devices)));
            if (item.Issues.Count == 0)
                Add(Text("No catalog entry covers this yet. Search All known issues for your symptom."));
        }
        foreach (var issue in item.Issues) ShowIssue(issue);
    }

    void ShowIssue(Issue issue)
    {
        Add(Heading(issue.Title, 20));
        Add(Text($"{Catalog.CategoryNames.GetValueOrDefault(issue.Category, issue.Category)} · {string.Join(", ", issue.Os)} · sources checked {issue.LastVerified}", 0.7));
        Add(Heading("Symptoms", 15));
        foreach (var s in issue.Symptoms) Add(Text("• " + s));
        Add(Heading("Likely causes", 15));
        foreach (var c in issue.Causes) Add(Text("• " + c));
        Add(Heading("Fixes — try in this order", 15));
        for (var n = 0; n < issue.Fixes.Count; n++) Add(FixCard(n + 1, issue.Fixes[n]));
    }

    UIElement FixCard(int number, Fix fix)
    {
        var panel = new StackPanel();
        var flags = new List<string> { fix.Confidence, "risk " + fix.Risk };
        if (fix.RequiresAdmin) flags.Add("needs admin");
        if (fix.RequiresRestorePoint) flags.Add("restore point recommended");
        if (!fix.Reversible) flags.Add("hard to undo");
        panel.Children.Add(Heading($"{number}. {fix.Title}", 14));
        panel.Children.Add(Text(string.Join(" · ", flags), 0.75));
        for (var i = 0; i < fix.Steps.Count; i++) panel.Children.Add(Text($"{i + 1}. {fix.Steps[i]}"));

        var sources = new TextBlock { TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 4, 0, 0) };
        sources.Inlines.Add("Sources: ");
        foreach (var source in fix.Sources)
        {
            if (source.SafeUri is not { } uri) continue;
            var link = new Hyperlink(new Run(uri.Host)) { NavigateUri = uri, ToolTip = source.Signal };
            link.RequestNavigate += (_, e) => Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            sources.Inlines.Add(link);
            sources.Inlines.Add($" ({source.Signal})  ");
        }
        panel.Children.Add(sources);

        if (FixAction.Parse(fix.Action) is { } action)
        {
            var button = new Button
            {
                Content = action.Kind == "open" ? "Open the setting" : "Preview and run this fix…",
                Padding = new Thickness(12, 4, 12, 4),
                Margin = new Thickness(0, 8, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            button.Click += async (_, _) => await RunFix(fix, action);
            panel.Children.Add(button);
        }

        return new Border
        {
            Child = panel,
            Padding = new Thickness(12),
            Margin = new Thickness(0, 6, 0, 6),
            BorderThickness = new Thickness(1),
            BorderBrush = SystemColors.ActiveBorderBrush,
            CornerRadius = new CornerRadius(6),
        };
    }

    async Task RunFix(Fix fix, FixAction action)
    {
        if (action.Kind == "open")
        {
            action.Open();
            FixLog.Write(fix.Id, "opened " + action.Target);
            return;
        }

        var script = Scripts.FixScript(action.Target);
        if (script is null) return;
        var dialog = new FixPreviewWindow(fix, script) { Owner = this };
        if (dialog.ShowDialog() != true) return;

        var toRun = dialog.CreateRestorePoint
            ? $"Checkpoint-Computer -Description 'Shabda: {fix.Id}' -RestorePointType MODIFY_SETTINGS\n{script}"
            : script;
        StatusText.Text = $"Running: {fix.Title}…";
        try
        {
            if (fix.RequiresAdmin)
            {
                var code = await Scripts.RunElevatedAsync(toRun);
                FixLog.Write(fix.Id, $"exit code {code}");
                StatusText.Text = code == 0 ? $"Done: {fix.Title}. Scanning again…" : $"The fix reported an error (exit code {code}).";
            }
            else
            {
                await Scripts.RunAsync(toRun);
                FixLog.Write(fix.Id, "exit code 0");
                StatusText.Text = $"Done: {fix.Title}. Scanning again…";
            }
            Scan_Click(this, new RoutedEventArgs());
        }
        catch (Win32Exception ex) when (ex.NativeErrorCode == 1223)
        {
            StatusText.Text = "Cancelled at the administrator prompt. Nothing was changed.";
        }
        catch (Exception ex)
        {
            FixLog.Write(fix.Id, "failed: " + ex.Message);
            StatusText.Text = "The fix failed: " + ex.Message;
        }
    }

    void Add(UIElement element) => Detail.Children.Add(element);

    static TextBlock Heading(string text, double size) => new()
    {
        Text = text, FontSize = size, FontWeight = FontWeights.SemiBold,
        TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 12, 0, 4),
    };

    static TextBlock Text(string text, double opacity = 1) => new()
    {
        Text = text, TextWrapping = TextWrapping.Wrap, Opacity = opacity, Margin = new Thickness(0, 2, 0, 2),
    };
}
