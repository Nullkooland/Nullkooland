using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nullkooland.Client.Services.Post;
using Nullkooland.Shared.Models.Post;

namespace Nullkooland.Client.ViewModels
{
    public class TagsPageViewModel
    {
        private readonly IBlogPostService _blogPostService;
        private IDictionary<string, int> _tags;

        public TagsPageViewModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        public bool IsLoading { get; set; } = true;

        public string FilterKeyword { get; set; }

        public IEnumerable<KeyValuePair<string, int>> Tags => string.IsNullOrWhiteSpace(FilterKeyword)
            ? _tags
                .OrderByDescending(pair => pair.Value)
            : _tags
                .Where(tag => tag.Key.Contains(FilterKeyword, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(pair => pair.Value);

        public IEnumerable<BlogPost> PostsWithCurrentTag(string tag)
        {
            return _blogPostService
                .Query(post => post.Tags.Contains(tag));
        }

        public async ValueTask LoadAllTags()
        {
            IsLoading = true;

            await _blogPostService.LoadAsync();
            _tags = _blogPostService.GetAllTags();

            IsLoading = false;
        }
    }
}