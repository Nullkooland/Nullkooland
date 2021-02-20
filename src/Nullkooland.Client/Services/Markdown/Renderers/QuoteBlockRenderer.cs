using Markdig.Syntax;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class QuoteBlockRenderer : ComponentObjectRenderer<QuoteBlock>
    {
        protected override void Write(ComponentRenderer renderer, QuoteBlock quoteBlock)
        {
            renderer.Builder.OpenElement(0, "div");
            renderer.Builder.AddAttribute(1, "Class", 
                "my-2 py-2 px-4 mud-elevation-1 rounded grey lighten-4");
            
            renderer.WriteChildren(quoteBlock);
            
            renderer.Builder.CloseElement();
        }
    }
}