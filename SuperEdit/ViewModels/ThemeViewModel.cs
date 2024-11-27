namespace SuperEdit.ViewModels;
using  TextMateSharp.Grammars;
public class ThemeViewModel
{
    private readonly ThemeName _themeName;

    public ThemeName ThemeName => _themeName;

    public string DisplayName => _themeName.ToString();
    public ThemeViewModel(ThemeName themeName)
    {
        _themeName = themeName;
    }
}