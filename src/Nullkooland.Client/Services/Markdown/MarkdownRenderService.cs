using Markdig;
using Microsoft.AspNetCore.Components;
using Nullkooland.Client.Services.Markdown.Renderers;
using Nullkooland.Client.Services.Markdown.Renderers.Inlines;
using Nullkooland.Client.Services.Theme;

namespace Nullkooland.Client.Services.Markdown
{
    public class MarkdownRenderService
    {
        private readonly MarkdownPipeline _pipeline;

        private readonly RazorComponentRenderer _renderer;

        public MarkdownRenderService(IThemeService themeService)
        {
            _pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseAutoLinks()
                .UseEmojiAndSmiley()
                .UseFigures()
                .UseMediaLinks()
                .UseMathematics()
                .Build();

            _renderer = new RazorComponentRenderer(themeService, _pipeline);
        }

        public string RenderHtml(string markdown)
        {
            return Markdig.Markdown.ToHtml(markdown, _pipeline);
        }

        public RenderFragment Render(string markdown, string fontFamily, string? baseUrl = null)
        {
            var paragraphRenderer = _renderer.ObjectRenderers.FindExact<ParagraphRenderer>();
            var autolinkRenderer = _renderer.ObjectRenderers.FindExact<AutolinkInlineRenderer>();
            var linkRenderer = _renderer.ObjectRenderers.FindExact<LinkInlineRenderer>();

            paragraphRenderer!.FontFamily = fontFamily;
            autolinkRenderer!.FontFamily = fontFamily;
            linkRenderer!.FontFamily = fontFamily;

            linkRenderer!.BaseUrl = baseUrl;

            return (RenderFragment)Markdig.Markdown.Convert(markdown, _renderer, _pipeline);
        }
    }
}