using Markdig.Syntax;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class ParagraphRenderer : ComponentObjectRenderer<ParagraphBlock>
    {
        protected override void Write(ComponentRenderer renderer, ParagraphBlock paragraphBlock)
        {
            renderer.Builder.OpenElement(0, "p");
            renderer.Builder.AddAttribute(1, "class", "mt-2");

            renderer.WriteChildren(paragraphBlock.Inline);

            renderer.Builder.CloseElement();
        }
    }
}