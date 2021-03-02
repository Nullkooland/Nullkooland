using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nullkooland.Shared.Models.Post;

namespace Nullkooland.Client.Services.Post
{
    public interface IBlogPostService
    {
        int Count { get; }
        ValueTask<int> LoadAsync();

        IEnumerable<BlogPost> GetAll();

        IDictionary<string, string[]> GetAllTags();

        BlogPost GetById(string id);

        IEnumerable<BlogPost> Query(Func<BlogPost, bool> filter);

        Task<string> GetContentAsync(BlogPost post);
    }
}