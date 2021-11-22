using Markdig.Syntax.Inlines;

namespace Nullkooland.Client.Services.Markdown.Renderers.Inlines
{
    public class LiteralInlineRenderer : RazorComponentObjectRenderer<LiteralInline>
    {
        protected override void Write(RazorComponentRenderer renderer, LiteralInline literalInline)
        {
            var builder = renderer.BuilderStack.Peek();

            builder.AddContent(renderer.Sequence++, literalInline.Content);
        }
    }
}