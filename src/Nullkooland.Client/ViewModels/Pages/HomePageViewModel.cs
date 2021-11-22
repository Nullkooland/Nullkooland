using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Nullkooland.Client.Services.Post;
using Nullkooland.Client.Services.Theme;
using Nullkooland.Shared.Models.Post;

namespace Nullkooland.Client.ViewModels.Pages
{
    public class HomePageViewModel
    {
        private const int NUM_RECENT_POSTS = 8;

        private readonly IThemeService _themeService;

        private readonly IBlogPostService _blogPostService;

        public HomePageViewModel(
            IThemeService themeService,
            IBlogPostService blogPostService
        )
        {
            _themeService = themeService;
            _blogPostService = blogPostService;
        }

        public bool IsLoading { get; set; } = true;

        public string PageTitle => _themeService.SiteTitle;

        public string AvatarImage => _themeService.AvatarImage;

        public string BackgroundPattern => _themeService.BackgroundPattern;

        public string GreetingsTitle => _themeService.GreetingsTitle;

        public string GreetingsContent => _themeService.GreetingsContent;


        public ImmutableList<BlogPost> RecentPosts { get; set; }

        public async ValueTask LoadRecentPostsAsync()
        {
            IsLoading = true;

            await _blogPostService.LoadAsync();
            RecentPosts = _blogPostService.GetAll()
                .OrderByDescending(post => post.Date)
                .Take(NUM_RECENT_POSTS)
                .ToImmutableList();

            IsLoading = false;
        }
    }
}