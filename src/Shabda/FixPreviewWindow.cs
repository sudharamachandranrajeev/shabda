using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Shabda;

/// <summary>Shows exactly what a fix will run and asks for confirmation.</summary>
public sealed class FixPreviewWindow : Window
{
    readonly CheckBox _restorePoint;

    public bool CreateRestorePoint => _restorePoint.IsChecked == true;

    public FixPreviewWindow(Fix fix, string script)
    {
        Title = "Confirm fix";
        Width = 720;
        Height = 520;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;

        var intro = new TextBlock
        {
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(0, 0, 0, 8),
            Text = $"{fix.Title}\n\nThis PowerShell will run{(fix.RequiresAdmin ? " as administrator (Windows will ask for permission)" : "")}. " +
                   $"Risk: {fix.Risk}. {(fix.Reversible ? "It can be undone." : "It is hard to undo.")}",
        };
        var code = new TextBox
        {
            Text = script,
            IsReadOnly = true,
            FontFamily = new FontFamily("Consolas"),
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
        };
        System.Windows.Automation.AutomationProperties.SetName(code, "Script that will run");
        _restorePoint = new CheckBox
        {
            Content = "Create a System Restore point first",
            IsChecked = fix.RequiresRestorePoint,
            IsEnabled = fix.RequiresAdmin,
            Margin = new Thickness(0, 8, 0, 0),
        };

        var run = new Button { Content = "Run fix", Padding = new Thickness(16, 4, 16, 4), IsDefault = true, Margin = new Thickness(0, 0, 8, 0) };
        run.Click += (_, _) => DialogResult = true;
        var cancel = new Button { Content = "Cancel", Padding = new Thickness(16, 4, 16, 4), IsCancel = true };
        var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 12, 0, 0) };
        buttons.Children.Add(run);
        buttons.Children.Add(cancel);

        var root = new DockPanel { Margin = new Thickness(16) };
        DockPanel.SetDock(intro, Dock.Top);
        DockPanel.SetDock(buttons, Dock.Bottom);
        DockPanel.SetDock(_restorePoint, Dock.Bottom);
        root.Children.Add(intro);
        root.Children.Add(buttons);
        root.Children.Add(_restorePoint);
        root.Children.Add(code);
        Content = root;
    }
}

static class FixLog
{
    static readonly string Folder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Shabda", "logs");

    /// <summary>Appends one line per fix attempt so the user can see what was changed.</summary>
    public static void Write(string fixId, string outcome)
    {
        try
        {
            Directory.CreateDirectory(Folder);
            File.AppendAllText(Path.Combine(Folder, "fixes.log"), $"{DateTime.Now:O}\t{fixId}\t{outcome}{Environment.NewLine}");
        }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
    }
}
