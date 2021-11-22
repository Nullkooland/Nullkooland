using Nullkooland.Client.Services.Theme;

namespace Nullkooland.Client.ViewModels.Pages
{
    public class AboutPageViewModel
    {
        private readonly IThemeService _themeService;

        public AboutPageViewModel(IThemeService themeService)
        {
            _themeService = themeService;
        }

        public string PageTitle => $"About - {_themeService.SiteTitle}";
    }
}