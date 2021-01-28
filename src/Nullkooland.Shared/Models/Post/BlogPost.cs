using System;
using System.Text.Json.Serialization;

namespace Nullkooland.Shared.Models.Post
{
    public record BlogPost
    {
        public string Id { get; set; }

        public BlogPostType Type { get; set; }

        public DateTime Date { get; set; }

        [JsonIgnore] public string Url => $"posts/{Date.Year}/{Id}";

        public string Title { get; set; }

        public string Brief { get; set; }

        public string HeaderImage { get; set; }

        [JsonIgnore] public string HeaderImagePath => $"{Url}/images/{HeaderImage}";

        public string[] Tags { get; set; }
    }
}