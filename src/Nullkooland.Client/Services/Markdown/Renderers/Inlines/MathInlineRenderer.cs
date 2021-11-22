using Markdig.Extensions.Mathematics;
using MathBlock = Nullkooland.Client.Views.Components.MathBlock;

namespace Nullkooland.Client.Services.Markdown.Renderers.Inlines
{
    public class MathInlineRenderer : RazorComponentObjectRenderer<MathInline>
    {
        protected override void Write(RazorComponentRenderer renderer, MathInline mathInline)
        {
            var builder = renderer.BuilderStack.Peek();

            builder.OpenComponent<MathBlock>(0);

            string mathText = mathInline.Content.ToString();
            builder.AddAttribute(1, "MathText", mathText);
            builder.AddAttribute(2, "Inline", true);

            builder.CloseComponent();
        }
    }
}