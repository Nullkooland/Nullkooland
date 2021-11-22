using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nullkooland.Client.Services.Post;
using Nullkooland.Client.Services.Theme;
using Nullkooland.Shared.Models.Post;

namespace Nullkooland.Client.ViewModels.Pages
{
    public class TagsPageViewModel
    {
        private readonly IThemeService _themeService;

        private readonly IBlogPostService _blogPostService;
        private IDictionary<string, string[]> _tagToPosts;

        public TagsPageViewModel(IThemeService themeService, IBlogPostService blogPostService)
        {
            _themeService = themeService;
            _blogPostService = blogPostService;
        }

        public bool IsLoading { get; set; } = true;

        public string PageTitle => $"Tags - {_themeService.SiteTitle}";

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