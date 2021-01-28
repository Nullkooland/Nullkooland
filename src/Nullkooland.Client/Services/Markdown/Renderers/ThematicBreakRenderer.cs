using Markdig.Syntax;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class ThematicBreakRenderer : ComponentObjectRenderer<ThematicBreakBlock>
    {
        protected override void Write(ComponentRenderer renderer, ThematicBreakBlock breakBlock)
        {
            if (breakBlock.ThematicChar == '-')
            {
                renderer.Builder.OpenComponent<MudDivider>(0);
                renderer.Builder.AddAttribute(1, "Class", "my-4");
                renderer.Builder.CloseComponent();
            }
        }
    }
}