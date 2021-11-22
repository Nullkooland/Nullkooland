using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Nullkooland.Shared.Models.Post;

namespace Nullkooland.Client.Services.Post
{
    public class LocalBlogPostService : IBlogPostService
    {
        private readonly HttpClient _client;

        private Dictionary<string, BlogPost> _posts;

        private Dictionary<string, string[]> _tags;

        public LocalBlogPostService(HttpClient client)
        {
            _client = client;
        }

        public async ValueTask<int> LoadAsync()
        {
            // Metadata already loaded, return immediately
            if (_posts?.Any() ?? false)
            {
                return _posts.Count;
            }

            _posts = await _client.GetFromJsonAsync<Dictionary<string, BlogPost>>("posts/index.json");
            _tags = await _client.GetFromJsonAsync<Dictionary<string, string[]>>("posts/tags.json");

            return _posts.Count;
        }

        public int Count => _posts.Count;

        public IEnumerable<BlogPost> GetAll()
        {
            return _posts.Values;
        }

        public IDictionary<string, string[]> GetAllTags()
        {
            return _tags;
        }

        public BlogPost GetById(string id)
        {
            if (_posts.TryGetValue(id, out var post))
            {
                return post;
            }

            return null;
        }

        public IEnumerable<BlogPost> Query(Func<BlogPost, bool> filter)
        {
            return _posts.Values.Where(post => filter(post));
        }

        public async Task<string> GetContentAsync(BlogPost post)
        {
            string content = await _client.GetStringAsync($"{post.Url}/{post.Id}.md");

            // Skip metadata section at the beginning of the markdown file
            int offset = content.IndexOf("---");
            offset = content.IndexOf('\n', offset);
            offset = content.IndexOf("---", offset);
            offset = content.IndexOf('\n', offset);

            return content[(offset + 1)..];
        }
    }
}