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
                renderer.Builder.AddAttribute(1, "class", "ml-4 d-flex flex-row align-center");


                renderer.Builder.OpenComponent<MudText>(2);
                renderer.Builder.AddAttribute(3, "Style", "font-weight: bolder");

                string bullet = GetBullet(listBlock.BulletType, i);
                renderer.Builder.AddAttribute(4, "ChildContent",
                    (RenderFragment) (builder => builder.AddMarkupContent(5, bullet))
                );


                renderer.Builder.AddAttribute(9, "Color", Color.Primary);
                renderer.Builder.CloseComponent();

                renderer.Write(item[0]);
                renderer.Builder.CloseElement();
            }
        }

        private static string GetBullet(char type, int index)
        {
            return type switch
            {
                '-' => "✧ &nbsp;",
                '*' => "✱ &nbsp;",
                '1' => $"{index + 1}. &nbsp;",
                'a' => $"{(char) ('a' + index)}. &nbsp;",
                'A' => $"{(char) ('A' + index)}. &nbsp;",
                'i' => $"{new string('i', index + 1)}. &nbsp;",
                _ => null
            };
        }
    }
}