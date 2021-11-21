using Markdig.Syntax.Inlines;

namespace Nullkooland.Client.Services.Markdown.Renderers.Inlines
{
    public class EmphasisInlineRenderer : RazorComponentObjectRenderer<EmphasisInline>
    {
        protected override void Write(RazorComponentRenderer renderer, EmphasisInline emphasisInline)
        {
            string emphasisElement = GetEmphasisElement(emphasisInline.DelimiterChar, emphasisInline.DelimiterCount);

            var builder = renderer.BuilderStack.Peek();

            builder.OpenElement(renderer.Sequence++, emphasisElement);
            renderer.WriteChildren(emphasisInline);
            builder.CloseElement();
        }

        private static string GetEmphasisElement(char delimiter, int count)
        {
            return (delimiter, count) switch
            {
                ('*', 2) => "b",
                ('*', 1) => "i",
                ('~', 2) => "del",
                ('_', 2) => "ins",
                _ => "b"
            };
        }
    }
}