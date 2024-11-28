using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AvaloniaEdit;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.VisualTree;

namespace SuperEdit.ViewModels;

using System.Collections.ObjectModel;
using AvaloniaEdit.Editing;
using ReactiveUI;
using SuperEdit.Views;
using TextMateSharp.Grammars;
using Avalonia;
using SuperEdit.Views;
using Avalonia.Controls;
using Avalonia.Threading;
using SuperEdit.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
{
    private string? _tabItemHeader = "新建文本.txt";

    public MainWindowViewModel()
    {
    }


    public void IncreaseFontSizeMouseCommmand(TextEditor editor)
    {
        editor.FontSize += 2;
    }

    public void DecreaseFontSizeMouseCommmand(TextEditor editor)
    {
        editor.FontSize -= 2;
    }

    public void ToggleAutoLineMouseCommmand(TextEditor editor)
    {
        editor.WordWrap = !editor.WordWrap;
    }


    public void CopyMouseCommmand(TextArea textArea)
    {
        ApplicationCommands.Copy.Execute(null, textArea);
    }

    public void CutMouseCommand(TextArea textArea)
    {
        ApplicationCommands.Cut.Execute(null, textArea);
    }

    public void PasteMouseCommmand(TextArea textArea)
    {
        ApplicationCommands.Paste.Execute(null, textArea);
    }

    public void SelectAllMouseCommmand(TextArea textArea)
    {
        ApplicationCommands.SelectAll.Execute(null, textArea);
    }

    // Undo Status is not given back to disable it's item in ContextFlyout; therefore it's not being used yet.
    public void UndoMouseCommmand(TextArea textArea)
    {
        ApplicationCommands.Undo.Execute(null, textArea);
    }

    public void RedoMouseCommmand(TextArea textArea)
    {
        ApplicationCommands.Redo.Execute(null, textArea);
    }

    public void FindMouseCommmand(TextArea textArea)
    {
        ApplicationCommands.Find.Execute(null, textArea);
    }

    public void ReplaceMouseCommmand(TextArea textArea)
    {
        ApplicationCommands.Replace.Execute(null, textArea);
    }

    public void ReloadFileMouseCommmand(Panel? panel)
    {
        var editor = panel?.GetVisualDescendants().OfType<TextEditor>().FirstOrDefault(e => e.Name == "Editor");
        var reloadButton = panel?.GetVisualDescendants().OfType<Button>().FirstOrDefault(e => e.Name == "FileName");

        if (editor == null)
        {
            Console.WriteLine("editor is null");
            return;
        }

        if (reloadButton == null)
        {
            Console.WriteLine("reloadButton is not null");
            return;
        }

        // Reload the file from disk
        try
        {
            var filePath = reloadButton.Content?.ToString();
            if (filePath != null && !filePath.Contains("新建文本.txt"))
            {
                var fileContent = File.ReadAllText(filePath);
                editor.Text = fileContent;
                Console.WriteLine("文件重新加载成功");
                Console.WriteLine("文件路径：" + filePath);
            }
            else
            {
                Console.WriteLine("文件不存在哈哈");
                Console.WriteLine("文件路径：" + filePath);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void CopyFilePathMouseCommmand(Button renameButton)
    {
        try
        {
            var window = new Window();
            var content = renameButton.Content;
            Console.WriteLine(content?.ToString());
            window.Clipboard?.SetTextAsync(content?.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void OpenFileFolderMouseCommmand(Button openFolderButton)
    {
        try
        {
            var filePath = openFolderButton.Content?.ToString();
            Console.WriteLine(filePath);
            if (filePath != null && !filePath.Contains("新建文本.txt"))
            {
                var fileDirectory = System.IO.Path.GetDirectoryName(filePath);
                Console.WriteLine(fileDirectory);
                if (fileDirectory != null)
                {
                    //打开文件夹
                    System.Diagnostics.Process.Start("explorer.exe", fileDirectory);
                }
            }

            else
            {
                Console.WriteLine("文件不存在哈哈");
                Console.WriteLine("文件路径：" + filePath);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void CreateNewTabCommand(TabControl tabControl)
    {
        try
        {
            var tabItem = tabControl.Items.First();

// will search for a ComboBox that's named "userList"
            if (tabItem != null)
            {
                Console.WriteLine("Creating a new tab");
                tabControl.Items.Add(tabItem as TabItem);
            }
            else
            {
                Console.WriteLine("failed to find tabItem or tabControl");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [Obsolete("Obsolete")]
    public async void SaveCurrentFile(TextEditor editor)
    {
        // 保存当前文件
        var filePath = editor.Document.FileName;
        Console.WriteLine(filePath);
        if (filePath != null && !filePath.Contains("新建文本.txt"))
        {
            Console.WriteLine(editor.Text);
            await File.WriteAllTextAsync(filePath, editor.Text); // 保存文件内容 到指定路径
            Console.WriteLine(filePath + " saved");
            // 更新标题栏
            TabItemHeader = filePath.Split('\\').Last();
            StatusBarFilePath = filePath; // 更新状态栏
            editor.Document.FileName = filePath; // 更新文件路径
        }
        else
        {
            Console.WriteLine("文件不存在哈哈");
            Console.WriteLine("文件路径：" + filePath);
        }
    }

    // 打开指定文件
    [Obsolete("Obsolete")]
    public async void OpenSpecificFileContextMenuCommand(TextEditor? editor, string? file)
    {
        //   if (file== null  )  file = editor?.Document.FileName;
        // 使用Avalonia的方法打开对话框
        if (file != null && !file.Contains("新建文本.txt"))
        {
            if (editor != null)
            {
                editor.Text = await File.ReadAllTextAsync(file); // 读取文件内容
                editor.Document.FileName = file; // 更新文件路径
                //  更新标题栏
                TabItemHeader = file.Split('\\').Last();
                // 更新视图是通过设置属性而不是的属性保存的状态, 状态由属性内部更改
                StatusBarFilePath = file; // 更新状态栏
            }
        }
        else
        {
            var dialog = new OpenFileDialog();
            var window = new MainWindow();
            var path = await dialog.ShowAsync(window);
            var filePath = path?.FirstOrDefault() ?? null;
            Console.WriteLine(filePath);
            if (!string.IsNullOrEmpty(filePath))
            {
                //  将文件内容加载到编辑器中
                if (editor != null)
                {
                    editor.Text = await File.ReadAllTextAsync(filePath);
                    //  更新文件路径
                    editor.Document.FileName = filePath;
                    Console.WriteLine(editor.Document.FileName + " loaded");
                    TabItemHeader = filePath.Split('\\').Last();
                    // 更新视图是通过设置属性而不是的属性保存的状态, 状态由属性内部更改
                    StatusBarFilePath = filePath; // 更新状态栏
                    Console.WriteLine("状态栏：" + StatusBarFilePath);
                    Console.WriteLine("标签栏：" + TabItemHeader);
                }

                // //  更新标题栏
                // TabItemHeader = filePath.Split('\\').Last();
                // // 更新视图是通过设置属性而不是的属性保存的状态, 状态由属性内部更改
                // StatusBarFilePath = filePath; // 更新状态栏
            }
        }
    }

    private string? _statusBarText = "新建文本.txt";
    // private IWindowImpl _windowImplImplementation;

    // private App _window;
//    public MainWindowViewModel(App app)
//    {
//        _window = app;
//    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string? TabItemHeader
    {
        get => _tabItemHeader;
        set
        {
            if (_tabItemHeader != value)
            {
                _tabItemHeader = value;
                OnPropertyChanged(nameof(TabItemHeader)); // 通知更新
            }
        }
    }

    public string? StatusBarFilePath
    {
        get => _statusBarText;
        set
        {
            if (_statusBarText != value)
            {
                _statusBarText = value;
                OnPropertyChanged(nameof(StatusBarFilePath)); // 通知更新
            }
        }
    }



    [Obsolete("Obsolete")]
    public void OpenSpecificFileContextMenuCommandByClick(TextEditor editor)
    {
        OpenSpecificFileContextMenuCommand(editor, null);
    }

    // 另存为
    [Obsolete("Obsolete")]
    public async void SaveCurrentTemplateFileToDisk(TextEditor editor)
    {
        var filePath = StatusBarFilePath;
        Console.WriteLine(filePath);
        if (filePath != null)
        {
            var dialog = new SaveFileDialog()
            {
                Title = "另存为", DefaultExtension = "txt", InitialFileName = "输入保存的文件名",
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter() { Name = "文本文件", Extensions = { "txt" } },
                    new FileDialogFilter() { Name = "所有文件", Extensions = { "*" } }
                }
            };
            var window = new MainWindow();
            var path = await dialog.ShowAsync(window);
            if (!string.IsNullOrEmpty(path))
            {
                // 根据用户选择的路径保存文件内容
                await SaveFileContentAsync(path, editor.Text ?? string.Empty);
                Console.WriteLine($"文件已保存至: {path}");
            }

            StatusBarFilePath = path; // 更新状态栏
            TabItemHeader = path?.Split('\\').Last(); // 更新标题栏
            editor.Document.FileName = path; // 更新文件路径
        }
    }

    private async Task SaveFileContentAsync(string filePath, string content)
    {
        // 异步保存文件
        using (var streamWriter = new StreamWriter(filePath))
        {
            await streamWriter.WriteAsync(content);
        }
    }


    public void testOpenFile()
    {
        var statusbar = StatusBarFilePath?.ToString() ?? "不存在statusBarfile";
        ;

        Console.WriteLine("状态栏：" + statusbar);
        var tabBar = TabItemHeader?.ToString() ?? "不存在tabBarfile";
        Console.WriteLine("标签栏bucuo：" + tabBar);
    }

    public void CloseTabOrWindowCommand()
    {
        // Avalonia 强制结束进程或者关闭窗口？

        Environment.Exit(0);
    }
}