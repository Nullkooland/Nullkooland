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

        public LocalBlogPostService(HttpClient client)
        {
            _client = client;
        }

        public async ValueTask<int> LoadAsync()
        {
            // Metadata already loaded, return immediately
            if (_posts?.Any() ?? false) return _posts.Count;

            var metasRaw = await _client.GetFromJsonAsync<BlogPost[]>("posts/metadata.json");
            if (metasRaw == null || metasRaw.Length == 0) return 0;

            _posts = metasRaw.ToDictionary(post => post.Id, post => post);
            return _posts.Count;
        }

        public int Count => _posts.Count;

        public IEnumerable<BlogPost> GetAll()
        {
            return _posts.Values;
        }

        public IDictionary<string, int> GetAllTags()
        {
            return _posts.Values
                .SelectMany(post => post.Tags)
                .GroupBy(tag => tag)
                .ToDictionary(group => group.Key, group => group.Count());
        }


        public BlogPost GetById(string id)
        {
            if (_posts.TryGetValue(id, out var post)) return post;

            return null;
        }

        public IEnumerable<BlogPost> Query(Func<BlogPost, bool> filter)
        {
            return _posts.Values.Where(post => filter(post));
        }

        public Task<string> GetContentAsync(BlogPost post)
        {
            return _client.GetStringAsync($"{post.Url}/{post.Id}.md");
        }
    }
}