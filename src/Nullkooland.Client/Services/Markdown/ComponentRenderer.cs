using System.IO;
using Markdig.Extensions.Mathematics;
using Markdig.Extensions.MediaLinks;
using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Syntax;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Nullkooland.Client.Services.Markdown.Renderers;
using Nullkooland.Client.Services.Markdown.Renderers.Inlines;

namespace Nullkooland.Client.Services.Markdown
{
    public sealed class ComponentRenderer : RendererBase
    {
        private readonly HtmlRenderer _htmlRenderer;

        public ComponentRenderer()
        {
            // Block renderers
            ObjectRenderers.Add(new QuoteBlockRenderer());
            ObjectRenderers.Add(new TableRenderer());
            ObjectRenderers.Add(new MathBlockRenderer());
            ObjectRenderers.Add(new CodeBlockRenderer());
            ObjectRenderers.Add(new HeadingsRenderer());
            ObjectRenderers.Add(new ListRenderer());
            ObjectRenderers.Add(new ParagraphRenderer());
            ObjectRenderers.Add(new ThematicBreakRenderer());

            // Inline renderers
            ObjectRenderers.Add(new MathInlineRenderer());
            ObjectRenderers.Add(new EmphasisInlineRenderer());
            ObjectRenderers.Add(new LinkInlineRenderer());
            ObjectRenderers.Add(new AutolinkInlineRenderer());
            ObjectRenderers.Add(new LineBreakInlineRenderer());
            ObjectRenderers.Add(new CodeInlineRenderer());
            ObjectRenderers.Add(new LiteralInlineRenderer());

            _htmlRenderer = new HtmlRenderer(new StringWriter());
            _htmlRenderer.ObjectRenderers.Add(new HtmlMathInlineRenderer());
            _htmlRenderer.ObjectRenderers.Add(new HtmlTableRenderer());

            var mediaExtension = new MediaLinkExtension();
            mediaExtension.Setup(null, _htmlRenderer);
        }

        public RenderTreeBuilder Builder { get; private set; }

        public override RenderFragment Render(MarkdownObject markdownObject)
        {
            return builder =>
            {
                Builder = builder;
                Write(markdownObject);
            };
        }

        public string RenderHtml(MarkdownObject obj)
        {
            (_htmlRenderer.Writer as StringWriter).GetStringBuilder().Clear();
            _htmlRenderer.Render(obj);
            _htmlRenderer.Writer.Flush();
            return _htmlRenderer.Writer.ToString();
        }
    }
}