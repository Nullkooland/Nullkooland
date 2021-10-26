using Markdig.Syntax.Inlines;
using Nullkooland.Client.Controls;

namespace Nullkooland.Client.Services.Markdown.Renderers.Inlines
{
    public class CodeInlineRenderer : ComponentObjectRenderer<CodeInline>
    {
        protected override void Write(ComponentRenderer renderer, CodeInline codeInline)
        {
            renderer.Builder.OpenComponent<CodeBlock>(0);

            renderer.Builder.AddAttribute(1, "Code", codeInline.Content);
            renderer.Builder.AddAttribute(2, "Inline", true);

            renderer.Builder.CloseComponent();
        }
    }
}