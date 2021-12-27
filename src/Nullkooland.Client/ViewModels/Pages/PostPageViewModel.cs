using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nullkooland.Client.Services.Markdown;
using Nullkooland.Client.Services.Post;
using Nullkooland.Client.Services.Theme;
using Nullkooland.Client.Models.Theme;
using Nullkooland.Shared.Models.Post;

namespace Nullkooland.Client.ViewModels.Pages
{
    public class PostPageViewModel
    {
        private readonly IThemeService _themeService;
        private readonly IBlogPostService _blogPostService;
        private readonly MarkdownRenderService _markdownRenderService;

        public PostPageViewModel(
            IThemeService themeService,
            IBlogPostService blogPostService,
            MarkdownRenderService markdownRenderService)
        {
            _themeService = themeService;
            _blogPostService = blogPostService;
            _markdownRenderService = markdownRenderService;
        }

        public bool IsLoading { get; set; } = true;

        public string PageTitle => $"{(IsLoading ? "Loading" : Post?.Title ?? "(ﾟ∀ﾟ )是想去静观镇吗？")} - {_themeService.SiteTitle}";

        public string TagsIcon => _themeService.Type switch
        {
            OolandThemeType.Yunshan => Icons.Material.Sharp.LocalOffer,
            OolandThemeType.Nullko => Icons.Material.Filled.Style,
            _ => string.Empty,
        };

        public BlogPost? Post { get; private set; }

        public RenderFragment? Markdown { get; set; }

        public Typo TitleTypo => Post?.Title.Length > 12 ? Typo.h3 : Typo.h2;

        public string? TitleFontFamily => Post?.Type switch
        {
            BlogPostType.Technical => "Roboto Slab",
            BlogPostType.Personal => "Noto Serif SC",
            BlogPostType.Ramblings => "Noto Serif SC",
            _ => "sans-serif",
        };

        public string? CommentTitle => Post?.Type switch
        {
            BlogPostType.Technical => "Share your thoughts",
            BlogPostType.Personal => "Leave a shout!",
            BlogPostType.Ramblings => "感觉好安静啊，评论区也没有人开腔！",
            _ => string.Empty
        };

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

            string? content = await _blogPostService.GetContentAsync(Post);

            string? fontFamily = Post?.Type switch
            {
                BlogPostType.Technical => "Roboto, sans-serif",
                BlogPostType.Personal => "Noto Serif SC",
                BlogPostType.Ramblings => "Noto Serif SC",
                _ => "sans-serif",
            };

            Markdown = _markdownRenderService.Render(content, fontFamily, Post.Url);

            IsLoading = false;
        }
    }
}