using System;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using SuperEdit.ViewModels;
using SuperEdit.Views;

namespace SuperEdit;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    [Obsolete("Obsolete")]
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var args = desktop.Args;


            if (args != null && args.Length > 0)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
                string filepath = args[0];
                var viewModel = desktop.MainWindow.DataContext as MainWindowViewModel;
                var mainWindow = desktop.MainWindow as MainWindow;
                // 获取 mainWIndowViewModel  中 Editor 控件
                var editor = mainWindow?.Find<AvaloniaEdit.TextEditor>("Editor");
                viewModel?.OpenSpecificFileContextMenuCommand(editor: editor, file: filepath);
            }
            else
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }
            base.OnFrameworkInitializationCompleted();
        }
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}