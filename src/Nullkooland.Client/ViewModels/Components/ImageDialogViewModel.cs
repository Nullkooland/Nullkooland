using Nullkooland.Client.Services.Theme;

namespace Nullkooland.Client.ViewModels.Components
{
    public class ImageDialogViewModel
    {
        private readonly IThemeService _themeService;

        public ImageDialogViewModel(IThemeService themeService)
        {
            _themeService = themeService;
        }
    }
}
