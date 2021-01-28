using System.Linq;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Nullkooland.Client.Controls;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class LinkInlineRenderer : ComponentObjectRenderer<LinkInline>
    {
        public string BaseUrl { get; set; }

        protected override void Write(ComponentRenderer renderer, LinkInline link)
        {
            string displayText = renderer.RenderHtml(link.FirstChild);
            string url = link.Url.Contains("://") ? link.Url : $"{BaseUrl}/{link.Url}";

            if (link.IsImage)
            {
                string[] attributes = link.Title.Split();
                bool isInline = attributes.Any(attr => attr == "inline");
                bool isNoClip = attributes.Any(attr => attr == "noclip");

                if (!isInline)
                {
                    renderer.Builder.OpenElement(0, "div");
                    renderer.Builder.AddAttribute(1, "class", "my-4 d-flex flex-row justify-center align-center");
                    renderer.Builder.AddAttribute(2, "style", "width: 100%");
                }

                renderer.Builder.OpenComponent<MediaBlock>(3);

                if (TryParseVideoSource(url, out string playerUrl))
                {
                    renderer.Builder.AddAttribute(4, "Source", playerUrl);
                    renderer.Builder.AddAttribute(5, "IsVideo", true);
                    renderer.Builder.AddAttribute(7, "Width", "800px");
                }
                else
                {
                    renderer.Builder.AddAttribute(8, "Source", url);
                }

                renderer.Builder.AddAttribute(9, "Caption", displayText);

                if (!(isInline || isNoClip))
                {
                    renderer.Builder.AddAttribute(10, "MaxHeight", "75vh");
                    renderer.Builder.AddAttribute(11, "Fit", "cover");
                }

                renderer.Builder.CloseComponent();

                if (!isInline) renderer.Builder.CloseElement();
            }
            else
            {
                renderer.Builder.OpenComponent<MudLink>(12);
                renderer.Builder.AddAttribute(13, "Href", url);

                if (string.IsNullOrEmpty(displayText)) displayText = url;

                renderer.Builder.AddAttribute(14, "ChildContent",
                    (RenderFragment) (builder => builder.AddContent(15, displayText))
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