using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers.Inlines
{
    public class EmphasisInlineRenderer : ComponentObjectRenderer<EmphasisInline>
    {
        protected override void Write(ComponentRenderer renderer, EmphasisInline emphasisInline)
        {
            bool isBold = emphasisInline.DelimiterChar == '*' && emphasisInline.DelimiterCount > 1;
            if (isBold)
            {
                renderer.Builder.OpenElement(0, "div");
                renderer.Builder.AddAttribute(1, "class", "d-inline-flex px-1 py-0 mud-action-default rounded");
                renderer.Builder.AddAttribute(2, "style", "background-color: var(--mud-palette-drawer-background)");
            }

            renderer.Builder.OpenComponent<MudText>(3);

            renderer.Builder.AddAttribute(4, "Typo", Typo.body1);
            renderer.Builder.AddAttribute(5, "Inline", true);

            string fontStyle = GetFontStyle(emphasisInline.DelimiterChar, emphasisInline.DelimiterCount);
            renderer.Builder.AddAttribute(6, "Style", fontStyle);

            string? text = emphasisInline.FirstChild.ToString();
            renderer.Builder.AddAttribute(7, "ChildContent",
                (RenderFragment)(builder => builder.AddContent(8, text))
            );

            renderer.Builder.CloseComponent();

            if (isBold)
            {
                renderer.Builder.CloseElement();
            }
        }

        private static string GetFontStyle(char delimiter, int count)
        {
            return delimiter switch
            {
                '*' => count == 1 ? "font-style: italic" : "font-weight: bold",
                '~' => count == 1 ? "text-decoration: underline" : "text-decoration: line-through",
                _ => "font-weight: bold"
            };
        }
    }
}