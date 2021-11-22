using System;
using System.Threading.Tasks;
using MudBlazor;
using Nullkooland.Client.Models.Theme;

namespace Nullkooland.Client.Services.Theme
{
    public interface IThemeService
    {
        ValueTask InitAsync();

        OolandThemeType Type { get; }

        event EventHandler<OolandThemeType>? ThemeChanged;

        bool IsDark { get; }

        string SiteTitle { get; }

        string AvatarImage { get; }

        string BackgroundPattern { get; }

        string GreetingsTitle { get; }

        string GreetingsContent { get; }

        Palette Colors { get; }

        MudTheme MudTheme { get; }
    }
}
