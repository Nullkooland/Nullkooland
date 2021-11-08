using Markdig.Extensions.Mathematics;
using MathBlock = Nullkooland.Client.Views.Components.MathBlock;

namespace Nullkooland.Client.Services.Markdown.Renderers.Inlines
{
    public class MathInlineRenderer : ComponentObjectRenderer<MathInline>
    {
        protected override void Write(ComponentRenderer renderer, MathInline mathInline)
        {
            renderer.Builder.OpenComponent<MathBlock>(0);

            string mathText = mathInline.Content.ToString();
            renderer.Builder.AddAttribute(1, "MathText", mathText);
            renderer.Builder.AddAttribute(2, "Inline", true);

            renderer.Builder.CloseComponent();
        }
    }
}