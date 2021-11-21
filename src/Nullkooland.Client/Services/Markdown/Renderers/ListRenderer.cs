using Markdig.Syntax;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class ListRenderer : RazorComponentObjectRenderer<ListBlock>
    {
        protected override void Write(RazorComponentRenderer renderer, ListBlock listBlock)
        {
            var builder = renderer.BuilderStack.Peek();

            for (int i = 0; i < listBlock.Count; i++)
            {
                var item = (listBlock[i] as ListItemBlock)!;
                builder.OpenElement(renderer.Sequence++, "div");
                builder.AddAttribute(renderer.Sequence++, "class", "ml-4 d-flex flex-row justify-start align-center");

                builder.OpenComponent<MudText>(renderer.Sequence++);
                builder.AddAttribute(renderer.Sequence++, "Class", "mr-2");
                builder.AddAttribute(renderer.Sequence++, "Style", "font-weight: bolder");

                string bullet = GetBullet(listBlock.BulletType, i);
                builder.AddAttribute(renderer.Sequence++, "ChildContent",
                    (RenderFragment)(inlineBuilder => inlineBuilder.AddContent(renderer.Sequence++, bullet))
                );

                builder.AddAttribute(renderer.Sequence++, "Color", Color.Secondary);
                builder.CloseComponent();

                renderer.Write(item[0]);
                builder.CloseElement();
            }
        }

        private static string GetBullet(char type, int index)
        {
            return type switch
            {
                '-' => "•",
                '*' => "✱",
                '1' => $"{index + 1}.",
                'a' => $"{(char)('a' + index)}.",
                'A' => $"{(char)('A' + index)}.",
                'i' => $"{new string('i', index + 1)}.",
                _ => string.Empty
            };
        }
    }
}