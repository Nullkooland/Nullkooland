using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nullkooland.Client.Views.Components;

namespace Nullkooland.Client.Services.Markdown.Renderers.Inlines
{
    public class LinkInlineRenderer : RazorComponentObjectRenderer<LinkInline>
    {
        public string? BaseUrl { get; set; }

        public string FontFamily { get; set; } = "sans-serif";

        protected override void Write(RazorComponentRenderer renderer, LinkInline link)
        {
            var builder = renderer.BuilderStack.Peek();

            string url = link.Url!.Contains("://") ? link.Url : $"{BaseUrl}/{link.Url}";
            string? captionText = link.FirstChild?.ToString();

            if (link.IsImage)
            {
                if (TryParseVideoSource(url, out string? playerUrl))
                {
                    builder.OpenComponent<VideoBlock>(renderer.Sequence++);
                    builder.AddAttribute(renderer.Sequence++, "Source", playerUrl);
                    builder.AddAttribute(renderer.Sequence++, "Width", "100%");
                    builder.CloseComponent();
                }
                else
                {
                    builder.OpenComponent<ImageBlock>(renderer.Sequence++);
                    builder.AddAttribute(renderer.Sequence++, "Source", url);
                    builder.AddAttribute(renderer.Sequence++, "Caption", captionText);
                    builder.AddAttribute(renderer.Sequence++, "Description", link.Title);

                    bool isInline = (link.PreviousSibling != null && link.PreviousSibling is not LineBreakInline);
                    builder.AddAttribute(renderer.Sequence++, "Inline", isInline);

                    if (!isInline)
                    {
                        builder.AddAttribute(renderer.Sequence++, "Width", "100%");
                        builder.AddAttribute(renderer.Sequence++, "MaxHeight", "75vh");
                    }

                    builder.CloseComponent();
                }
            }
            else
            {
                builder.OpenComponent<MudLink>(renderer.Sequence++);
                builder.AddAttribute(renderer.Sequence++, "Href", url);
                builder.AddAttribute(renderer.Sequence++, "Style", $"font-family: {FontFamily}");

                if (string.IsNullOrEmpty(captionText))
                {
                    captionText = url;
                }

                builder.AddAttribute(renderer.Sequence++, "ChildContent",
                    (RenderFragment)(builder => builder.AddContent(13, captionText))
                );

                builder.CloseComponent();
            }
        }

        private static bool TryParseVideoSource(string url, out string? playerUrl)
        {
            if (url.StartsWith("https://www.youtube.com/watch"))
            {
                int posVideoIDBegin = url.IndexOf("v=") + 2;
                string videoID = url[posVideoIDBegin..];
                playerUrl = $"https://www.youtube.com/embed/{videoID}";
                return true;
            }

            if (url.StartsWith("https://www.bilibili.com/video/"))
            {
                int posBvBegin = url.IndexOf("video/") + 6;
                string bv = url[posBvBegin..];
                playerUrl = $"https://player.bilibili.com/player.html?bvid={bv}&high_quality=1";
                return true;
            }

            playerUrl = null;
            return false;
        }
    }
}