using MathBlock = Nullkooland.Client.Views.Components.MathBlock;
using MathBlockSyntax = Markdig.Extensions.Mathematics.MathBlock;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class MathBlockRenderer : RazorComponentObjectRenderer<MathBlockSyntax>
    {
        protected override void Write(RazorComponentRenderer renderer, MathBlockSyntax mathBlock)
        {
            string mathText = mathBlock.Lines.ToString();

            var builder = renderer.BuilderStack.Peek();

            builder.OpenComponent<MathBlock>(renderer.Sequence++);
            builder.AddAttribute(renderer.Sequence++, "MathText", mathText);
            builder.AddAttribute(renderer.Sequence++, "Inline", false);
            builder.CloseComponent();
        }
    }
}