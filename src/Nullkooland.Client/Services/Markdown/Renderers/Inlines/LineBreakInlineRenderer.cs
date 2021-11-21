using Markdig.Syntax.Inlines;

namespace Nullkooland.Client.Services.Markdown.Renderers.Inlines
{
    public class LineBreakInlineRenderer : RazorComponentObjectRenderer<LineBreakInline>
    {
        protected override void Write(RazorComponentRenderer renderer, LineBreakInline obj)
        {
            var builder = renderer.BuilderStack.Peek();

            builder.OpenElement(renderer.Sequence++, "br");
            builder.CloseElement();
        }
    }
}