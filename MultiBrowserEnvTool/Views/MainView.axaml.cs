using Avalonia.Controls;
using Avalonia.Interactivity;

namespace MultiBrowserEnvTool.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void MenuItem_Options_Click(object sender, RoutedEventArgs e)
    {
        new OptionsWindow().Show();
    }

    private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

}
