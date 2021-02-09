using System;
using System.Text.Json.Serialization;

namespace Nullkooland.Shared.Models.Post
{
    public record BlogPost
    {
        public string Id { get; init; }

        public BlogPostType Type { get; init; }

        public DateTime Date { get; init; }

        [JsonIgnore]
        public string Url => $"posts/{Date.Year}/{Id}";

        public string Title { get; init; }

        public string Brief { get; init; }

        public string HeaderImage { get; init; }

        [JsonIgnore]
        public string HeaderImagePath => $"{Url}/images/{HeaderImage}";

        public string[] Tags { get; init; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}