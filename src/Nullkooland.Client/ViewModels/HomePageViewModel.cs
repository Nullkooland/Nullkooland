using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Nullkooland.Client.Services.Post;
using Nullkooland.Shared.Models.Post;

namespace Nullkooland.Client.ViewModels
{
    public class HomePageViewModel
    {
        private const int NUM_RECENT_POSTS = 8;

        private readonly IBlogPostService _blogPostService;

        public HomePageViewModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
            // Hardcode for now, we'll tweak this later
            AvatarImage = "images/nullko.png";
            GreetingsTitle = "Hey, you've found my ooland";
            GreetingsText = "I've been hibernating in my cave, waiting for spring flowers to blossom.";
        }

        public bool IsLoading { get; set; } = true;

        public string GreetingsTitle { get; set; }

        public string GreetingsText { get; set; }

        public string AvatarImage { get; set; }

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