using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaUi.Views;
using Engine.EventArgs;
using Engine.ViewModels;

namespace AvaloniaUi
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            { 
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new GameSessionViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}