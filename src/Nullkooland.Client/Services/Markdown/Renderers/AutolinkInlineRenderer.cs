using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class AutolinkInlineRenderer : ComponentObjectRenderer<AutolinkInline>
    {
        protected override void Write(ComponentRenderer renderer, AutolinkInline linkInline)
        {
            renderer.Builder.OpenComponent<MudLink>(0);

            renderer.Builder.AddAttribute(1, "Href", linkInline.Url);

            renderer.Builder.AddAttribute(2, "ChildContent",
                (RenderFragment) (builder => builder.AddContent(4, linkInline.Url))
            );

            renderer.Builder.CloseComponent();
        }
    }
}