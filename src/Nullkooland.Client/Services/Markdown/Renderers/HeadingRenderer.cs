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
            Typo.h2,
            Typo.h3,
            Typo.h4,
            Typo.h5,
            Typo.h6,
            Typo.subtitle1
        };

        protected override void Write(ComponentRenderer renderer, HeadingBlock heading)
        {
            int level = Math.Min(Math.Max(heading.Level, 1), 6);

            renderer.Builder.OpenComponent<MudText>(0);

            renderer.Builder.AddAttribute(1, "Typo", _headingTypos[level - 1]);

            if (level == 1) renderer.Builder.AddAttribute(2, "Style", "font-family: Roboto Slab");

            if (level == 2) renderer.Builder.AddAttribute(2, "Color", Color.Secondary);

            if (level < 4) renderer.Builder.AddAttribute(3, "Class", $"mt-{(4 - level) * 3} mb-{(4 - level) * 1}");

            string title = renderer.RenderHtml(heading.Inline);

            renderer.Builder.AddAttribute(4, "ChildContent",
                (RenderFragment) (builder => builder.AddMarkupContent(5, title))
            );

            renderer.Builder.CloseComponent();

            if (level == 1)
            {
                renderer.Builder.OpenComponent<MudDivider>(6);
                renderer.Builder.AddAttribute(7, "DividerType", DividerType.FullWidth);
                renderer.Builder.CloseComponent();
            }
        }
    }
}