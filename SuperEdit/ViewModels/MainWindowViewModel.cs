using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Avalonia.Input;
using AvaloniaEdit;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
namespace SuperEdit.ViewModels;
using System.Collections.ObjectModel;
using AvaloniaEdit.Editing;
using ReactiveUI;
using SuperEdit.Views;
using TextMateSharp.Grammars;
 using Avalonia;
public partial class MainWindowViewModel :  ViewModelBase,INotifyPropertyChanged
{
    private string ? _tabItemHeader="新建文本.txt";


    public void IncreaseFontSizeMouseCommmand(TextEditor editor)
    {

        editor.FontSize += 2;
    }
    public void DecreaseFontSizeMouseCommmand(TextEditor editor)
    {
        editor.FontSize -= 2;
    }

    public void ToggleAutoLineMouseCommmand(    TextEditor editor)
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

    public void ReloadFileMouseCommmand(Button reloadButton)
    {
        // Reload the file from disk
        try
        {
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public   void CopyFilePathMouseCommmand(Button  renameButton )
    {
        try
        {
            var window = new Window();
            var content = renameButton.Content;
            Console.WriteLine(content? .ToString());
            window.Clipboard?.SetTextAsync( content? .ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public void OpenFileFolderMouseCommmand(Button   openFolderButton)
    {
        try
        {
            var filePath = openFolderButton.Content?.ToString();
            Console.WriteLine(filePath);
            var fileDirectory = System.IO.Path.GetDirectoryName(filePath);
            Console.WriteLine(fileDirectory);
            if (fileDirectory != null) System.Diagnostics.Process.Start(fileDirectory);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void CreateNewTabCommand( TabControl tabControl  )
    {
        try
        {

            var tabItem = tabControl.Items.First();

// will search for a ComboBox that's named "userList"
            if (tabItem != null)
            {
                Console.WriteLine("Creating a new tab");
                tabControl.Items.Add(tabItem  as TabItem);
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
    public  async void SaveCurrentFile(TextEditor editor)
    {

    }

    [Obsolete("Obsolete")]
    public async   void OpenSpecificFileContextMenuCommand(TextEditor editor)
    {

        // 使用Avalonia的方法打开对话框
        var dialog = new    OpenFileDialog();
        var window = new MainWindow();
        var path = await dialog.ShowAsync( window);
        var filePath =    path?.FirstOrDefault() ?? null;
        Console.WriteLine(filePath);
        if (!string.IsNullOrEmpty(filePath))
        {
            //  将文件内容加载到编辑器中
            editor.Text = await File.ReadAllTextAsync(filePath);
            //  更新文件路径
            editor.Document.FileName = filePath;
            Console.WriteLine(editor.Document.FileName);
            //  更新标题栏
            _tabItemHeader = filePath.Split('\\').Last();
            _statusBarText = filePath; // 更新状态栏

        }
    }
   private string ? _statusBarText = "新建文本.txt";
    public object? TabItemHeader
    {
        get => _tabItemHeader;
        set => _tabItemHeader = (string?)value;
    }

    public object? StatusBarFilePath { get=> _statusBarText; set => _statusBarText = (string?)value; }
}