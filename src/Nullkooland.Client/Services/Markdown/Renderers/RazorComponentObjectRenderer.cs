using Markdig.Renderers;
using Markdig.Syntax;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public abstract class RazorComponentObjectRenderer<TObject> : MarkdownObjectRenderer<RazorComponentRenderer, TObject>
        where TObject : MarkdownObject
    {
    }
}