using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Nullkooland.Client.Services.Post;
using Nullkooland.Shared.Models.Post;

namespace Nullkooland.Client.ViewModels.Components
{
    public class ArchivesViewerViewModel
    {
        private readonly IBlogPostService _blogPostService;

        private ImmutableList<BlogPost>? _posts;

        public ArchivesViewerViewModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        public BlogPostType Type { get; set; }

        public bool IsLoading { get; set; }

        public IEnumerable<BlogPost>? Posts => _posts?
            .Skip((CurrentPage - 1) * NumBlogsPerPage)
            .Take(NumBlogsPerPage);

        public int CurrentPage { get; set; } = 1;

        public int NumPages => (Count - 1) / NumBlogsPerPage + 1;

        public int NumBlogsPerPage { get; set; } = 8;

        public int Count => _posts?.Count ?? 0;

        public async ValueTask LoadArchivesAsync()
        {
            IsLoading = true;

            await _blogPostService.LoadAsync();
            _posts = _blogPostService.GetAll()
                .Where(post => post.Type == Type)
                .OrderByDescending(post => post.Date)
                .ToImmutableList();

            IsLoading = false;
        }
    }
}