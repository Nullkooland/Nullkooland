using System.Threading.Tasks;
using MudBlazor;
using MudBlazor.Services;
using Nullkooland.Client.Services.Post;
using Nullkooland.Shared.Models.Post;

namespace Nullkooland.Client.ViewModels
{
    public class PostPageViewModel
    {
        private readonly IBlogPostService _blogPostService;
        private readonly IResizeListenerService _resizeListenerService;

        public PostPageViewModel(IBlogPostService blogPostService, IResizeListenerService resizeListenerService)
        {
            _blogPostService = blogPostService;
            _resizeListenerService = resizeListenerService;
        }

        public bool IsLoading { get; set; } = true;

        public Breakpoint Breakpoint { get; set; }

        public BlogPost Post { get; set; }

        public Typo TitleTypo => Post.Title.Length > 12 ? Typo.h3 : Typo.h2;

        public string CommentTitle => Post.Type switch
        {
            BlogPostType.Technical => "Share your thoughts",
            BlogPostType.Personal => "Leave a shout!",
            BlogPostType.Ramblings => "感觉好安静啊，评论区也没有人开腔！"
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

        public async ValueTask GetBreakPointAsync()
        {
            Breakpoint = await _resizeListenerService.GetBreakpoint();
        }
    }
}