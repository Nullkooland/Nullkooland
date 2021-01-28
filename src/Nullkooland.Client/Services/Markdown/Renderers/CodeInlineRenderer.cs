using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using CodeBlock = Nullkooland.Client.Controls.CodeBlock;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class CodeInlineRenderer : ComponentObjectRenderer<CodeInline>
    {
        protected override void Write(ComponentRenderer renderer, CodeInline codeInline)
        {
            renderer.Builder.OpenComponent<CodeBlock>(0);
            
            renderer.Builder.AddAttribute(1, "Inline", true);
            renderer.Builder.AddAttribute(2, "Code", codeInline.Content);
            
            renderer.Builder.CloseComponent();
        }
    }
}