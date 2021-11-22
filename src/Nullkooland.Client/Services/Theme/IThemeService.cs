using System;
using System.Threading.Tasks;
using MudBlazor;

namespace Nullkooland.Client.Services.Theme
{
    public enum ThemeType
    {
        // Moonbear of the day
        Nullko,
        // Tiger of the night
        Yunshan,
    }

    public interface IThemeService
    {
        ValueTask InitAsync();

        ThemeType Type { get; }

        event EventHandler<ThemeType>? ThemeChanged;

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
