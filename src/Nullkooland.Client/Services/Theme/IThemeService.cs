using System.Threading.Tasks;
using MudBlazor;
using Nullkooland.Client.Models.Theme;

namespace Nullkooland.Client.Services.Theme
{
    public interface IThemeService
    {
        ValueTask InitAsync();

        bool IsDarkMode { get; set; }

        OolandThemeType ThemeType { get; }

        string SiteTitle { get; }

        string AvatarImage { get; }

        string AppBarIcon { get; }

        string BackgroundPattern { get; }

        string GreetingsTitle { get; }

        string GreetingsContent { get; }

        MudTheme MudTheme { get; }
    }
}
