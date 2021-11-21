using Markdig.Syntax.Inlines;

namespace Nullkooland.Client.Services.Markdown.Renderers.Inlines
{
    public class HtmlInlineRenderer : RazorComponentObjectRenderer<HtmlInline>
    {
        protected override void Write(RazorComponentRenderer renderer, HtmlInline htmlInline)
        {
            var builder = renderer.BuilderStack.Peek();

            builder.AddMarkupContent(renderer.Sequence++, htmlInline.Tag);
        }
    }
}