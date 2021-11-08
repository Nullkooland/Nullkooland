using Markdig.Syntax;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class QuoteBlockRenderer : ComponentObjectRenderer<QuoteBlock>
    {
        protected override void Write(ComponentRenderer renderer, QuoteBlock quoteBlock)
        {
            renderer.Builder.OpenElement(0, "blockquote");
            renderer.Builder.AddAttribute(1, "class", "my-2 py-2 px-4 mud-elevation-1 rounded");
            renderer.Builder.AddAttribute(2, "style",
                "border-left: 0.4em solid var(--mud-palette-primary-lighten);" +
                "background: var(--mud-palette-drawer-background);"
            );
            renderer.WriteChildren(quoteBlock);

            renderer.Builder.CloseElement();
        }
    }
}