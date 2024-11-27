using AvaloniaEdit.Editing;
using AvaloniaEdit.Highlighting;

namespace SuperEdit.ViewModels;

using Avalonia;
using AvaloniaEdit.Rendering;
using Avalonia.Media;

public class CurrentLineHighLight  : IBackgroundRenderer
{
    private readonly TextArea _textArea;
    private readonly SolidColorBrush _highlightBrush;

    public CurrentLineHighLight(TextArea textArea)
    {
        _textArea = textArea;
        _highlightBrush = new SolidColorBrush(Color.FromRgb(52, 52, 52));
        // _highlightBrush = new SolidColorBrush(Color.FromRgb(34, 34, 34));
    }

    public KnownLayer Layer => KnownLayer.Background; // 背景渲染层

    public void Draw(TextView textView, DrawingContext drawingContext)
    {
        // 确保文本视图的可见性
        if (_textArea.Document == null || !_textArea.IsKeyboardFocusWithin)
            return;

        // 获取光标当前行
        var line = _textArea.Caret.Line;
        var visualLine = textView.GetVisualLine(line);

        if (visualLine != null)
        {
            // 计算当前行的屏幕矩形
            var lineRect = new Rect(
                0,                          // 起始 X 坐标
                visualLine.VisualTop - textView.VerticalOffset, // 行顶部
                textView.Bounds.Width,      // 行宽度（填满窗口）
                visualLine.Height           // 行高度
            );

            // 绘制高亮背景
            drawingContext.FillRectangle(_highlightBrush, lineRect);
        }
    }
}
