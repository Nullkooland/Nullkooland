using Markdig.Extensions.Mathematics;
using MathBlock = Nullkooland.Client.Controls.MathBlock;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class MathInlineRenderer : ComponentObjectRenderer<MathInline>
    {
        protected override void Write(ComponentRenderer renderer, MathInline mathInline)
        {
            renderer.Builder.OpenComponent<MathBlock>(0);

            renderer.Builder.AddAttribute(1, "Inline", true);

            string mathText = mathInline.Content.ToString();
            renderer.Builder.AddAttribute(2, "MathText", mathText);

            renderer.Builder.CloseComponent();
        }
    }
}