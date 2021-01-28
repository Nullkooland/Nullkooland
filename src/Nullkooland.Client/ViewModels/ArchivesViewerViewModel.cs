using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Nullkooland.Client.Services.Post;
using Nullkooland.Shared.Models.Post;

namespace Nullkooland.Client.ViewModels
{
    public class ArchivesViewerViewModel
    {
        private readonly IBlogPostService _blogPostService;

        private int _currentPage = 1;
        private ImmutableList<BlogPost> _posts;

        public ArchivesViewerViewModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        public BlogPostType Type { get; set; }

        public bool IsLoading { get; set; }

        public IEnumerable<BlogPost> Posts => _posts
            .Skip((CurrentPage - 1) * NumBlogsPerPage)
            .Take(NumBlogsPerPage);

        public int CurrentPage
        {
            get => _currentPage;
            set => _currentPage = Math.Min(Math.Max(value, 1), NumPages);
        }

        public bool IsFirstPage => CurrentPage == 1;

        public bool IsLastPage => CurrentPage == NumPages;

        public int NumPages => (Count - 1) / NumBlogsPerPage + 1;

        public int NumBlogsPerPage { get; set; } = 8;

        public int Count => _posts?.Count ?? 0;

        public void OnFirstPageButtonClicked()
        {
            CurrentPage = 1;
        }

        public void OnPreviousPageButtonClicked()
        {
            CurrentPage--;
        }

        public void OnNextPageButtonClicked()
        {
            CurrentPage++;
        }

        public void OnLastPageButtonClicked()
        {
            CurrentPage = NumPages;
        }

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