using Avalonia.Controls;
using Avalonia.Input;
using AvaloniaEdit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using AvaloniaEdit.CodeCompletion;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using AvaloniaEdit.Folding;
using AvaloniaEdit.Rendering;
using AvaloniaEdit.TextMate; // 这是 TextMate 的库
using TextMateSharp.Grammars;
using Avalonia.Diagnostics;
using AvaloniaEdit.Snippets;
using ReactiveUI;
using Snippet = AvaloniaEdit.Snippets.Snippet;
using SuperEdit.ViewModels;
using SuperEdit.Resources;

namespace SuperEdit.Views;

using System.Windows;
using SuperEdit.ViewModels;

public partial class MainWindow : Window

{
    public MainWindow()
    {
        InitializeComponent();
        var editor = this.FindControl<TextEditor>("Editor");
        SyntaxHighlightingGrammar(editor);
        mainWindowView = new MainWindowViewModel();
        HightLightCurrentLine(editor);
    }

    public MainWindowViewModel? mainWindowView;
    private void SyntaxHighlightingGrammar(TextEditor? editor)
    {
        var registryOptions = new RegistryOptions(ThemeName.DarkPlus);

//Initial setup of TextMate.
        var textMateInstallation = editor.InstallTextMate(registryOptions) ??
                                   throw new ArgumentNullException("editor is  not InstallTextMate(_registryOptions)");

//Here we are getting the language by the extension and right after that we are initializing grammar with this language.
//And that's all 😀, you are ready to use AvaloniaEdit with syntax highlighting!
        textMateInstallation.SetGrammar(
            registryOptions.GetScopeByLanguageId(registryOptions.GetLanguageByExtension(".cs").Id));
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            this.BeginMoveDrag(e);
        }
    }

    private void HightLightCurrentLine(TextEditor? editor)
    {
        // 添加自定义行高亮渲染器
        if (editor?.TextArea != null)
        {
            var highlighter = new CurrentLineHighLight(editor.TextArea);
            editor.TextArea.TextView.BackgroundRenderers.Add(highlighter);
        }

        // 确保光标移动时触发重绘
        if (editor != null)
            editor.TextArea.Caret.PositionChanged += (_, __) =>
            {
                editor.TextArea.TextView.InvalidateVisual(); // 重绘文本视图
            };
    }


    private void TextEditor_PointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        var textEditor = sender as TextEditor;
        if (textEditor != null && e.KeyModifiers == KeyModifiers.Control)
        {
            // 获取当前字体大小
            var currentFontSize = textEditor.FontSize;

            // 根据滚轮的方向来增加或减少字体大小
            if (e.Delta.Y > 0) // 向上滚动
            {
                textEditor.FontSize = currentFontSize + 1; // 增加大小
            }
            else // 向下滚动
            {
                textEditor.FontSize = currentFontSize - 1; // 减小大小
            }

            // 防止默认的滚动事件继续传播
            e.Handled = true;
        }
    }

    private void AddFeatureToContextMenu()
    {
        var editor = this.FindControl<TextEditor>("Editor");
        if (editor != null) editor.FontSize = 22;
        if (editor != null) editor.Text = "Hello SuperEdit!";

        var contextMenu = editor?.ContextMenu;

        if (contextMenu != null)
        {
            var menuItem1 = new MenuItem() { Header = "菜单项 1" };
            var menuItem2 = new MenuItem() { Header = "菜单项 2" };

            // 为菜单项添加事件处理程序
            menuItem1.Click += (sender, e) =>
            {
                // 菜单项 1 被点击时的处理逻辑
            };
            menuItem2.Click += (sender, e) => { };

            // 将菜单项添加到上下文菜单中
            contextMenu.Items.Add(menuItem1);
            contextMenu.Items.Add(menuItem2);

            // 为窗口启用右键点击打开菜单
            this.AddHandler(Control.PointerPressedEvent, (sender, e) =>
            {
                if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
                {
                    contextMenu.Open(this);
                }
            });
        }
    }

    [Obsolete("Obsolete")]
    private void BindingKeyEvent(object? sender, KeyEventArgs e)
    {
        var editor = sender as TextEditor;
        if (editor == null) return;

        switch (e)
        {
            case { Key: Key.Z, KeyModifiers: KeyModifiers.Alt }:
                editor.WordWrap = !editor.WordWrap;
                break;
            case { Key: Key.D, KeyModifiers: KeyModifiers.Alt }:
                editor.FontSize += 2;
                break;
            case { Key: Key.S, KeyModifiers: KeyModifiers.Alt }:
                editor.FontSize -= 2;
                break;
            case { Key: Key.S, KeyModifiers: KeyModifiers.Control }:
            {
                mainWindowView?.SaveCurrentFile(editor);
                break;
            }
            case { Key: Key.O, KeyModifiers: KeyModifiers.Control }:
            {
                mainWindowView?.OpenSpecificFileContextMenuCommand(editor , file: null );
                break;
            }
            case { Key: Key.S, KeyModifiers: KeyModifiers.Control | KeyModifiers.Shift }:
            {
                mainWindowView?.SaveCurrentTemplateFileToDisk(editor );
                break;
            }
            case { Key : Key.W, KeyModifiers: KeyModifiers.Control }:
            {
                mainWindowView?.CloseTabOrWindowCommand();
                break;
            }
        }
    }

    private void MonitoringFileContentsChanged(object? sender, EventArgs e)
    {
        var editor = sender as TextEditor;
        if (editor == null) return;
        IsContentModifiedText = editor.IsModified ? "内容已修改" : "";
    }
    private string? _changedText = "";

    public string? IsContentModifiedText
    {
        get => _changedText;
        set
        {
            if (_changedText != value)
            {
                _changedText = value;
                OnPropertyChanged(nameof(IsContentModifiedText)); // 通知更新
            }
        }
    }
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

}