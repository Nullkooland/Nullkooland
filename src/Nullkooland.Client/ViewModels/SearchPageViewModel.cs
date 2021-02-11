using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nullkooland.Client.Services.Post;
using Nullkooland.Shared.Models.Post;

namespace Nullkooland.Client.ViewModels
{
    public class SearchPageViewModel
    {
        private const int MATCHED_TEXT_LENGTH = 150;
        private readonly IBlogPostService _blogPostService;

        private readonly Dictionary<BlogPost, string> _postsWithContent;

        private readonly char[] SENTENCE_DELIMITERS = {'.', '?', '!', '\n', '\t', '#', '。', '？', '！'};

        public SearchPageViewModel(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
            _postsWithContent = new Dictionary<BlogPost, string>();
        }

        public bool IsLoading { get; set; } = true;

        public string Keywords { get; set; }

        public IEnumerable<(BlogPost post, string matchedKeyword, string matchedText)> GetPostsWithMatchedKeyword()
        {
            if (string.IsNullOrWhiteSpace(Keywords)) yield break;

            // Extract the keywords
            string[] keywords = Keywords.Split();

            foreach ((var post, string content) in _postsWithContent)
            foreach (string keyword in keywords)
            {
                // First let's see if there's any match in post content
                int posMatch = content.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
                string matchedText = null;

                // Found a match, slice a sentence around it
                if (posMatch != -1)
                {
                    int posBegin = content.LastIndexOfAny(SENTENCE_DELIMITERS, posMatch);
                    posBegin = posBegin != -1 ? posBegin + 1 : 0;

                    int posEnd = Math.Min(posBegin + MATCHED_TEXT_LENGTH, content.Length);
                    matchedText = content[posBegin..posEnd];
                }

                // No match in the content, try the title and brief
                if (post.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    post.Brief.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    // Not really matched text, but hey, we gotta display something or the viewer gets annoyed!
                    int posEnd = Math.Min(MATCHED_TEXT_LENGTH, content.Length);
                    matchedText = content[..posEnd];
                }

                if (matchedText != null)
                {
                    // We're done for this post
                    yield return (post, keyword, matchedText.TrimStart() + "...");

                    // Gracefully skip other keywords
                    // like, "I DON'T GIVE A SHIT"ly gracefully
                    break;
                }
            }
        }

        public async ValueTask LoadPostsWithContentAsync()
        {
            if (_postsWithContent.Any()) return;

            IsLoading = true;

            await _blogPostService.LoadAsync();

            foreach (var post in _blogPostService.GetAll())
            {
                string content = await _blogPostService.GetContentAsync(post);
                _postsWithContent.Add(post, content);
            }

            IsLoading = false;
        }

        private static bool IsMatch(KeyValuePair<BlogPost, string> postWithContent, string keywords)
        {
            (var post, string content) = postWithContent;
            string[] keywordsArray = keywords.Split();

            if (keywordsArray.Any(keyword => post.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                return true;

            if (keywordsArray.Any(keyword => post.Brief.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                return true;

            if (keywordsArray.Any(keyword => content.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                return true;

            return false;
        }
    }
}