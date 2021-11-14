using System.Threading.Tasks;
using MudBlazor;
using Nullkooland.Client.Services.Post;
using Nullkooland.Client.Services.Theme;
using Nullkooland.Shared.Models.Post;

namespace Nullkooland.Client.ViewModels.Pages
{
    public class PostPageViewModel
    {
        private readonly IThemeService _themeService;
        private readonly IBlogPostService _blogPostService;

        public PostPageViewModel(
            IThemeService themeService,
            IBlogPostService blogPostService)
        {
            _themeService = themeService;
            _blogPostService = blogPostService;
        }

        public bool IsLoading { get; set; } = true;

        public string PageTitle => $"{(IsLoading ? "Loading" : Post?.Title ?? "(ﾟ∀ﾟ )是想去静观镇吗？")} - {_themeService.SiteTitle}";

        public string TagsIcon => _themeService.Type switch
        {
            ThemeType.Yunshan => Icons.Material.Sharp.LocalOffer,
            ThemeType.Nullko => Icons.Material.Filled.Style,
            _ => string.Empty,
        };

        public BlogPost? Post { get; private set; }

        public Typo TitleTypo => Post?.Title.Length > 12 ? Typo.h3 : Typo.h2;

        public string? TitleFontFamily => Post?.Type switch
        {
            BlogPostType.Technical => "Roboto Slab",
            BlogPostType.Personal => "LXGW WenKai",
            BlogPostType.Ramblings => "LXGW WenKai",
            _ => "Roboto",
        };

        public string? CommentTitle => Post?.Type switch
        {
            BlogPostType.Technical => "Share your thoughts",
            BlogPostType.Personal => "Leave a shout!",
            BlogPostType.Ramblings => "感觉好安静啊，评论区也没有人开腔！",
            _ => string.Empty
        };

        public string Content { get; set; }

        public async ValueTask LoadMarkdownAsync(string id)
        {
            IsLoading = true;

            int blogsCount = await _blogPostService.LoadAsync();
            if (blogsCount == 0)
            {
                IsLoading = false;
                return;
            }

            Post = _blogPostService.GetById(id);
            if (Post == null)
            {
                IsLoading = false;
                return;
            }

            Content = await _blogPostService.GetContentAsync(Post);
            IsLoading = false;
        }
    }
}