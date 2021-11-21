using Markdig.Extensions.Tables;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class TableRenderer : RazorComponentObjectRenderer<Table>
    {
        protected override void Write(RazorComponentRenderer renderer, Table table)
        {
            var builder = renderer.BuilderStack.Peek();

            builder.OpenComponent<MudSimpleTable>(renderer.Sequence++);
            builder.AddAttribute(renderer.Sequence++, "FixedHeader", true);
            builder.AddAttribute(renderer.Sequence++, "Hover", true);
            builder.AddAttribute(renderer.Sequence++, "Striped", true);

            builder.AddAttribute(renderer.Sequence++, "ChildContent", (RenderFragment)(inlineBuilder =>
            {
                renderer.BuilderStack.Push(inlineBuilder);

                inlineBuilder.OpenElement(renderer.Sequence++, "thead");
                RenderTableRow(renderer, (TableRow)table[0], true);
                inlineBuilder.CloseElement();

                inlineBuilder.OpenElement(renderer.Sequence++, "tbody");
                for (int i = 1; i < table.Count; i++)
                {
                    RenderTableRow(renderer, (TableRow)table[i], false);
                }
                inlineBuilder.CloseElement();

                renderer.BuilderStack.Pop();
            }));

            builder.CloseComponent();
        }

        private static void RenderTableRow(RazorComponentRenderer renderer, TableRow row, bool isHead)
        {
            var builder = renderer.BuilderStack.Peek();

            builder.OpenElement(renderer.Sequence++, "tr");
            for (int j = 0; j < row.Count; j++)
            {
                builder.OpenElement(renderer.Sequence++, isHead ? "th" : "td");
                renderer.Write(row[j]);
                builder.CloseElement();
            }
            builder.CloseElement();
        }
    }
}