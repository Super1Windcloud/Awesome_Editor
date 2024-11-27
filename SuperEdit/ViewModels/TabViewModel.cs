using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ReactiveUI;

namespace SuperEdit.ViewModels;

public   class  TabViewModel : ViewModelBase
{
    public string? Title { get; set; } // 标签标题（文件名）
    public string? Content { get; set; } // 编辑器内容


    // public ObservableCollection<MainWindowViewModel> Tabs { get; } = new();
    // public int SelectedTabIndex { get; set; }
    // public object CreateNewTabCommand { get; }
    //
    // public MainWindowViewModel(Action<MainWindowViewModel> closeAction)
    // {
    //     CloseCommand = ReactiveCommand.Create(() => closeAction(this));
    // }
    //
    //
    // private void AddNewTab()
    // {
    //     Tabs.Add(new MainWindowViewModel(RemoveTab)
    //     {
    //         Title = "新建文本文档.txt",
    //         Content = string.Empty
    //     });
    //     SelectedTabIndex = Tabs.Count - 1;
    // }
    //
    // private void RemoveTab(MainWindowViewModel tab )
    // {
    //     Tabs.Remove(tab);
    // }

}