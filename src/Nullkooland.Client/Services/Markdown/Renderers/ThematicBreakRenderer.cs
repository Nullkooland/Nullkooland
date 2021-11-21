using Markdig.Syntax;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class ThematicBreakRenderer : RazorComponentObjectRenderer<ThematicBreakBlock>
    {
        protected override void Write(RazorComponentRenderer renderer, ThematicBreakBlock breakBlock)
        {
            if (breakBlock.ThematicChar != '-')
            {
                return;
            }

            var builder = renderer.BuilderStack.Peek();

            builder.OpenComponent<MudDivider>(renderer.Sequence++);
            builder.AddAttribute(renderer.Sequence++, "Class", "my-4");
            builder.CloseComponent();
        }
    }
}