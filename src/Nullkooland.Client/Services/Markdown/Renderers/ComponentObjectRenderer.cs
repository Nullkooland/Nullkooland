using Markdig.Renderers;
using Markdig.Syntax;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public abstract class ComponentObjectRenderer<TObject> : MarkdownObjectRenderer<ComponentRenderer, TObject>
        where TObject : MarkdownObject
    {
    }
}