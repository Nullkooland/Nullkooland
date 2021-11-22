using System.Collections.Generic;
using System.Linq;
using MudBlazor;
using Nullkooland.Client.Services.Theme;

namespace Nullkooland.Client.ViewModels.Shared
{
    public class MainLayoutViewModel
    {
        private readonly IThemeService _themeService;

        private static readonly string[] _navTitles =
        {
            "Home", "Archives", "Tags", "Search", "About"
        };

        private static readonly string[] _navUrls =
        {
            "/", "archives", "tags", "search", "about"
        };

        private static readonly string[] _navNullkoIcons =
        {
            Icons.Material.Rounded.Home,
            Icons.Material.Rounded.AllInbox,
            Icons.Material.Rounded.Style,
            Icons.Material.Rounded.Search,
            Icons.Material.Rounded.EmojiNature,
        };

        private static readonly string[] _navYunshanIcons =
        {
            Icons.Material.Outlined.Terrain,
            Icons.Material.Outlined.Widgets,
            Icons.Material.Outlined.LocalOffer,
            Icons.Material.Outlined.Explore,
            Icons.Custom.Uncategorized.Radioactive,
        };

        public MainLayoutViewModel(IThemeService themeService)
        {
            _themeService = themeService;
        }

        public MudTheme Theme => _themeService.MudTheme;

        public string AppBarTitle => _themeService.SiteTitle;

        public bool IsNavMenuOpened { get; set; }

        public void OnNavMenuButtonClicked()
        {
            IsNavMenuOpened = !IsNavMenuOpened;
        }

        public IEnumerable<(string, string, string)> GetNavItems => _themeService.Type switch
        {
            ThemeType.Nullko => _navUrls.Zip(_navTitles, _navNullkoIcons),
            ThemeType.Yunshan => _navUrls.Zip(_navTitles, _navYunshanIcons),
            _ => Enumerable.Empty<(string, string, string)>()
        };
    }
}