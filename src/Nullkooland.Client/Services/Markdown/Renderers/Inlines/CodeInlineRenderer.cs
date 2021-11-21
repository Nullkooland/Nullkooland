using Markdig.Syntax.Inlines;
using Nullkooland.Client.Views.Components;

namespace Nullkooland.Client.Services.Markdown.Renderers.Inlines
{
    public class CodeInlineRenderer : RazorComponentObjectRenderer<CodeInline>
    {
        protected override void Write(RazorComponentRenderer renderer, CodeInline codeInline)
        {
            var builder = renderer.BuilderStack.Peek();

            builder.OpenComponent<CodeBlock>(renderer.Sequence++);
            builder.AddAttribute(renderer.Sequence++, "Code", codeInline.Content);
            builder.AddAttribute(renderer.Sequence++, "Inline", true);
            builder.CloseComponent();
        }
    }
}