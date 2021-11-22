using Markdig.Syntax;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class QuoteBlockRenderer : RazorComponentObjectRenderer<QuoteBlock>
    {
        protected override void Write(RazorComponentRenderer renderer, QuoteBlock quoteBlock)
        {
            var builder = renderer.BuilderStack.Peek();

            builder.OpenElement(renderer.Sequence++, "blockquote");
            builder.AddAttribute(renderer.Sequence++, "class", "my-2 py-2 px-4 mud-elevation-1 rounded");
            builder.AddAttribute(renderer.Sequence++, "style",
                "border-left: 0.4em solid var(--mud-palette-primary-lighten);" +
                "background: var(--mud-palette-drawer-background);"
            );
            renderer.WriteChildren(quoteBlock);
            builder.CloseElement();
        }
    }
}