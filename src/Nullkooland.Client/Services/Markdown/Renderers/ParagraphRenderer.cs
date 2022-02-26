using Markdig.Syntax;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class ParagraphRenderer : RazorComponentObjectRenderer<ParagraphBlock>
    {
        protected override void Write(RazorComponentRenderer renderer, ParagraphBlock paragraphBlock)
        {
            if (paragraphBlock.Inline == null)
            {
                return;
            }

            var builder = renderer.BuilderStack.Peek();

            builder.OpenComponent<MudText>(renderer.Sequence++);

            if (paragraphBlock.Parent is MarkdownDocument)
            {
                // Only add vertical margin for non-inlined paragraph blocks.
                builder.AddAttribute(renderer.Sequence++, "Class", "my-4");
            }

            builder.AddAttribute(renderer.Sequence++, "ChildContent", (RenderFragment)(inlineBuilder =>
            {
                renderer.BuilderStack.Push(inlineBuilder);
                renderer.WriteChildren(paragraphBlock.Inline);
                renderer.BuilderStack.Pop();
            }));

            builder.CloseElement();
        }
    }
}