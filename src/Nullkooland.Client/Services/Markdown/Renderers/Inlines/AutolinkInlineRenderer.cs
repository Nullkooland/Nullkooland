using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers.Inlines
{
    public class AutolinkInlineRenderer : RazorComponentObjectRenderer<AutolinkInline>
    {
        public string FontFamily { get; set; } = "sans-serif";

        protected override void Write(RazorComponentRenderer renderer, AutolinkInline linkInline)
        {
            var builder = renderer.BuilderStack.Peek();

            builder.OpenComponent<MudLink>(renderer.Sequence++);
            builder.AddAttribute(renderer.Sequence++, "Href", linkInline.Url);
            builder.AddAttribute(renderer.Sequence++, "Style", $"font-family: {FontFamily}");

            builder.AddAttribute(renderer.Sequence++, "ChildContent",
                (RenderFragment)(inlineBuilder => inlineBuilder.AddContent(renderer.Sequence++, linkInline.Url))
            );

            builder.CloseComponent();
        }
    }
}