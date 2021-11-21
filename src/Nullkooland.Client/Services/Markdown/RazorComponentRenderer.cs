using System.Collections.Generic;
using System.IO;
using Markdig;
using Markdig.Extensions.Mathematics;
using Markdig.Extensions.MediaLinks;
using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Syntax;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Nullkooland.Client.Services.Markdown.Renderers;
using Nullkooland.Client.Services.Markdown.Renderers.Inlines;
using Nullkooland.Client.Services.Theme;

namespace Nullkooland.Client.Services.Markdown
{
    public sealed class RazorComponentRenderer : RendererBase
    {
        private readonly HtmlRenderer _htmlRenderer;
        public RazorComponentRenderer(IThemeService themeService, MarkdownPipeline pipeline)
        {
            ThemeService = themeService;

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
            mediaExtension.Setup(pipeline, _htmlRenderer);

            BuilderStack = new Stack<RenderTreeBuilder>(4);
        }

        public Stack<RenderTreeBuilder> BuilderStack { get; set; }

        public int Sequence { get; set; }

        public IThemeService ThemeService { get; private set; }

        public override RenderFragment Render(MarkdownObject markdownObject)
        {
            return builder =>
            {
                BuilderStack.Push(builder);
                Sequence = 0;

                Write(markdownObject);

                BuilderStack.Clear();
            };
        }

        public string? RenderHtml(MarkdownObject obj)
        {
            (_htmlRenderer.Writer as StringWriter)!.GetStringBuilder().Clear();
            _htmlRenderer.Render(obj);
            _htmlRenderer.Writer.Flush();
            return _htmlRenderer.Writer.ToString();
        }
    }
}