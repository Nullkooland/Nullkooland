using System;
using Markdig.Syntax;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class HeadingsRenderer : ComponentObjectRenderer<HeadingBlock>
    {
        private static readonly Typo[] _headingTypos =
        {
            Typo.h3,
            Typo.h4,
            Typo.h5,
            Typo.h6,
            Typo.subtitle1,
            Typo.subtitle2
        };

        protected override void Write(ComponentRenderer renderer, HeadingBlock heading)
        {
            int level = Math.Min(Math.Max(heading.Level, 1), 6);
            
            // Ignore the level 1 title and let PostPage to render it with greater flexibilities.
            if (level == 1) return;

            renderer.Builder.OpenComponent<MudText>(0);

            renderer.Builder.AddAttribute(1, "Typo", _headingTypos[level - 1]);

            if (level == 2) renderer.Builder.AddAttribute(2, "Color", Color.Secondary);

            if (level < 5) renderer.Builder.AddAttribute(3, "Class", $"mt-{(5 - level) * 2}");

            string title = renderer.RenderHtml(heading.Inline);

            renderer.Builder.AddAttribute(4, "ChildContent",
                (RenderFragment) (builder => builder.AddMarkupContent(5, title))
            );

            renderer.Builder.CloseComponent();
        }
    }
}