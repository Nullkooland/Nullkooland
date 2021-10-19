using System.Linq;
using Markdig.Syntax.Inlines;
using Markdig.Helpers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nullkooland.Client.Controls;

namespace Nullkooland.Client.Services.Markdown.Renderers.Inlines
{
    public class LinkInlineRenderer : ComponentObjectRenderer<LinkInline>
    {
        public string BaseUrl { get; set; }

        protected override void Write(ComponentRenderer renderer, LinkInline link)
        {
            string? captionText = link.FirstChild?.ToString();
            string url = link.Url.Contains("://") ? link.Url : $"{BaseUrl}/{link.Url}";

            if (link.IsImage)
            {
                if (TryParseVideoSource(url, out string playerUrl))
                {
                    renderer.Builder.OpenComponent<VideoBlock>(0);
                    renderer.Builder.AddAttribute(1, "Source", playerUrl);
                    renderer.Builder.AddAttribute(2, "Width", "100%");
                    renderer.Builder.CloseComponent();
                }
                else
                {

                    renderer.Builder.OpenComponent<ImageBlock>(3);
                    renderer.Builder.AddAttribute(4, "Source", url);
                    renderer.Builder.AddAttribute(5, "Caption", captionText);
                    renderer.Builder.AddAttribute(6, "Description", link.Title);

                    bool isInline = (link.PreviousSibling != null && link.PreviousSibling is not LineBreakInline);
                    renderer.Builder.AddAttribute(7, "Inlined", isInline);

                    if (!isInline)
                    {
                        renderer.Builder.AddAttribute(8, "Width", "100%");
                        renderer.Builder.AddAttribute(9, "MaxHeight", "75vh");
                    }

                    renderer.Builder.CloseComponent();
                }
            }
            else
            {
                renderer.Builder.OpenComponent<MudLink>(10);
                renderer.Builder.AddAttribute(11, "Href", url);

                if (string.IsNullOrEmpty(captionText)) captionText = url;

                renderer.Builder.AddAttribute(12, "ChildContent",
                    (RenderFragment)(builder => builder.AddContent(13, captionText))
                );

                renderer.Builder.CloseComponent();
            }
        }

        private static bool TryParseVideoSource(string url, out string playerUrl)
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