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
        private IDictionary<string, string[]> _tagToPosts;

        public TagsPageViewModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        public bool IsLoading { get; set; } = true;

        public string FilterKeyword { get; set; }

        public IEnumerable<(string tag, int count)> Tags => string.IsNullOrWhiteSpace(FilterKeyword)
            ? _tagToPosts
                .Select(pair => (pair.Key, pair.Value.Length))
                .OrderByDescending(pair => pair.Length)

            : _tagToPosts
                .Where(tag => tag.Key.Contains(FilterKeyword, StringComparison.OrdinalIgnoreCase))
                .Select(pair => (pair.Key, pair.Value.Length))
                .OrderByDescending(pair => pair.Length);

        public IEnumerable<BlogPost> PostsWithCurrentTag(string tag)
        {
            return _tagToPosts[tag].Select(id => _blogPostService.GetById(id));
        }

        public async ValueTask LoadAllTags()
        {
            IsLoading = true;

            await _blogPostService.LoadAsync();
            _tagToPosts = _blogPostService.GetAllTags();

            IsLoading = false;
        }
    }
}