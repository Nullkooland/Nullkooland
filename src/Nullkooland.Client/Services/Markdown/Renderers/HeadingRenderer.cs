using System;
using Markdig.Syntax;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class HeadingsRenderer : RazorComponentObjectRenderer<HeadingBlock>
    {
        public string FontFamily { get; set; } = "sans-serif";

        private static readonly Typo[] _headingTypos =
        {
            Typo.h4, // Level 2
            Typo.h5, // Level 3
            Typo.h6, // Level 4
        };

        protected override void Write(RazorComponentRenderer renderer, HeadingBlock heading)
        {
            if (heading.Inline == null)
            {
                return;
            }

            // Only supports 1/2/3/4 level.
            int level = Math.Min(Math.Max(heading.Level, 1), 4);

            // Ignore the post (level 1) title,
            // let PostPage component to render it with greater flexibilities.
            if (level == 1)
            {
                return;
            }

            string title = renderer.RenderHtml(heading.Inline)!;
            string id = heading.Inline.ToPositionText();

            var builder = renderer.BuilderStack.Peek();

            builder.OpenComponent<MudText>(renderer.Sequence++);
            builder.AddAttribute(renderer.Sequence++, "id", id);
            builder.AddAttribute(renderer.Sequence++, "Class", $"my-{6 - level}");
            builder.AddAttribute(renderer.Sequence++, "Typo", _headingTypos[level - 2]);
            builder.AddAttribute(renderer.Sequence++, "Style", $"font-family: {FontFamily}");

            if (level == 2)
            {
                // Use primary color for section (level 2) title.
                builder.AddAttribute(renderer.Sequence++, "Color", Color.Primary);
            }

            builder.AddAttribute(renderer.Sequence++, "ChildContent", (RenderFragment)(inlineBuilder =>
            {
                renderer.BuilderStack.Push(inlineBuilder);
                renderer.WriteChildren(heading.Inline);
                renderer.BuilderStack.Pop();
            }));

            builder.CloseComponent();
        }
    }
}