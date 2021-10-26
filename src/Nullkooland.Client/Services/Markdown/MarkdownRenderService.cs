using Markdig;
using Microsoft.AspNetCore.Components;
using Nullkooland.Client.Services.Markdown.Renderers.Inlines;

namespace Nullkooland.Client.Services.Markdown
{
    public class MarkdownRenderService
    {
        private readonly MarkdownPipeline _pipeline;

        private readonly ComponentRenderer _renderer;

        public MarkdownRenderService()
        {
            _pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseAutoLinks()
                .UseEmojiAndSmiley()
                .UseFigures()
                .UseMediaLinks()
                .UseMathematics()
                .Build();

            _renderer = new ComponentRenderer();
        }

        public string RenderHtml(string markdown)
        {
            return Markdig.Markdown.ToHtml(markdown, _pipeline);
        }

        public RenderFragment Render(string markdown, string? baseUrl = null)
        {
            if (baseUrl != null)
            {
                var linkRenderer = _renderer.ObjectRenderers.FindExact<LinkInlineRenderer>();
                if (linkRenderer != null) linkRenderer.BaseUrl = baseUrl;
            }

            return (RenderFragment)Markdig.Markdown.Convert(markdown, _renderer, _pipeline);
        }
    }
}