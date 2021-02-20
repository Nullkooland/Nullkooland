using Markdig.Syntax;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class ListRenderer : ComponentObjectRenderer<ListBlock>
    {
        protected override void Write(ComponentRenderer renderer, ListBlock listBlock)
        {
            for (int i = 0; i < listBlock.Count; i++)
            {
                var item = listBlock[i] as ListItemBlock;
                renderer.Builder.OpenElement(0, "div");
                renderer.Builder.AddAttribute(1, "class", "ml-4 d-flex flex-row justify-start align-start");


                renderer.Builder.OpenComponent<MudText>(2);
                renderer.Builder.AddAttribute(3, "Class", "mt-2 mr-2");
                renderer.Builder.AddAttribute(4, "Style", "font-weight: bolder");

                string bullet = GetBullet(listBlock.BulletType, i);
                renderer.Builder.AddAttribute(5, "ChildContent",
                    (RenderFragment) (builder => builder.AddMarkupContent(6, bullet))
                );

                renderer.Builder.AddAttribute(7, "Color", Color.Primary);
                renderer.Builder.CloseComponent();

                renderer.Write(item[0]);
                renderer.Builder.CloseElement();
            }
        }

        private static string GetBullet(char type, int index)
        {
            return type switch
            {
                '-' => "✧",
                '*' => "✱",
                '1' => $"{index + 1}.",
                'a' => $"{(char) ('a' + index)}.",
                'A' => $"{(char) ('A' + index)}.",
                'i' => $"{new string('i', index + 1)}.",
                _ => null
            };
        }
    }
}