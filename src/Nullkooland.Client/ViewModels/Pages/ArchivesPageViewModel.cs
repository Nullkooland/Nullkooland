using Nullkooland.Client.Services.Theme;

namespace Nullkooland.Client.ViewModels.Pages
{
    public class ArchivesPageViewModel
    {
        private readonly IThemeService _themeService;

        public ArchivesPageViewModel(IThemeService themeService)
        {
            _themeService = themeService;
        }

        public string PageTitle => $"Archives - {_themeService.SiteTitle}";

        public int ActiveViewerIndex { get; set; } = 0;
    }
}