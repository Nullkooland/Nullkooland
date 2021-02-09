using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class EmphasisInlineRenderer : ComponentObjectRenderer<EmphasisInline>
    {
        protected override void Write(ComponentRenderer renderer, EmphasisInline emphasisInline)
        {
            bool isBold = emphasisInline.DelimiterChar == '*' && emphasisInline.DelimiterCount > 1;
            if (isBold)
            {
                renderer.Builder.OpenElement(-1, "div");
                renderer.Builder.AddAttribute(-1, "class", "d-inline-flex px-1 py-0 grey lighten-3 rounded");
            }

            renderer.Builder.OpenComponent<MudText>(0);

            renderer.Builder.AddAttribute(1, "Typo", Typo.body1);
            renderer.Builder.AddAttribute(2, "Inline", true);

            string fontStyle = GetFontStyle(emphasisInline.DelimiterChar, emphasisInline.DelimiterCount);
            renderer.Builder.AddAttribute(3, "Style", fontStyle);

            string? text = emphasisInline.FirstChild.ToString();
            renderer.Builder.AddAttribute(5, "ChildContent",
                (RenderFragment) (builder => builder.AddContent(6, text))
            );

            renderer.Builder.CloseComponent();

            if (isBold) renderer.Builder.CloseElement();
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