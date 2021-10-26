using Nullkooland.Client.Controls;
using MathBlockSyntax = Markdig.Extensions.Mathematics.MathBlock;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class MathBlockRenderer : ComponentObjectRenderer<MathBlockSyntax>
    {
        protected override void Write(ComponentRenderer renderer, MathBlockSyntax mathBlock)
        {
            renderer.Builder.OpenComponent<MathBlock>(0);

            string mathText = mathBlock.Lines.ToString();
            renderer.Builder.AddAttribute(1, "MathText", mathText);
            renderer.Builder.AddAttribute(2, "Inline", false);

            renderer.Builder.CloseComponent();
        }
    }
}