using Markdig.Syntax.Inlines;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class LineBreakInlineRenderer : ComponentObjectRenderer<LineBreakInline>
    {
        protected override void Write(ComponentRenderer renderer, LineBreakInline obj)
        {
            renderer.Builder.OpenElement(0, "br");
            renderer.Builder.CloseElement();
        }
    }
}