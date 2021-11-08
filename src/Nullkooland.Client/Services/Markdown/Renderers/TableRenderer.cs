using Markdig.Extensions.Tables;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class TableRenderer : ComponentObjectRenderer<Table>
    {
        protected override void Write(ComponentRenderer renderer, Table table)
        {
            renderer.Builder.OpenComponent<MudSimpleTable>(0);
            renderer.Builder.AddAttribute(1, "FixedHeader", true);
            renderer.Builder.AddAttribute(2, "Hover", true);
            renderer.Builder.AddAttribute(3, "Striped", true);

            // Render table contents using HtmlRenderer and trim the '<table></table>' tag
            string tableContent = renderer.RenderHtml(table)[8..^9];
            renderer.Builder.AddAttribute(4, "ChildContent",
                (RenderFragment)(builder => builder.AddMarkupContent(5, tableContent)));

            renderer.Builder.CloseComponent();
        }
    }
}