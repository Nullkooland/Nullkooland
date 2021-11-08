using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers.Inlines
{
    public class LiteralInlineRenderer : ComponentObjectRenderer<LiteralInline>
    {
        protected override void Write(ComponentRenderer renderer, LiteralInline literalInline)
        {
            renderer.Builder.OpenComponent<MudText>(0);

            renderer.Builder.AddAttribute(1, "Typo", Typo.body1);
            renderer.Builder.AddAttribute(2, "Inline", true);

            string text = literalInline.Content.ToString();
            renderer.Builder.AddAttribute(3, "ChildContent",
                (RenderFragment)(builder => builder.AddContent(4, text))
            );

            renderer.Builder.CloseComponent();
        }
    }
}