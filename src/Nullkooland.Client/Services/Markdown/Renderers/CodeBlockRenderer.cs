using System.Text;
using Markdig.Syntax;
using CodeBlock = Nullkooland.Client.Views.Components.CodeBlock;
using CodeBlockSyntax = Markdig.Syntax.CodeBlock;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class CodeBlockRenderer : RazorComponentObjectRenderer<CodeBlockSyntax>
    {
        protected override void Write(RazorComponentRenderer renderer, CodeBlockSyntax codeBlock)
        {
            if (codeBlock is FencedCodeBlock fencedCodeBlock)
            {
                string code = ExtractSourceCode(codeBlock);
                string? language = fencedCodeBlock.Info;

                var builder = renderer.BuilderStack.Peek();

                builder.OpenComponent<CodeBlock>(renderer.Sequence++);
                builder.AddAttribute(renderer.Sequence++, "Code", code);
                builder.AddAttribute(renderer.Sequence++, "Language", language);
                builder.AddAttribute(renderer.Sequence++, "Inline", false);
                builder.CloseComponent();
            }
        }

        private static string ExtractSourceCode(CodeBlockSyntax node)
        {
            var code = new StringBuilder();
            var lines = node.Lines.Lines;
            int totalLines = lines.Length;
            for (int i = 0; i < totalLines; i++)
            {
                var line = lines[i];
                var slice = line.Slice;
                if (slice.Text == null)
                {
                    continue;
                }

                string lineText = slice.Text.Substring(slice.Start, slice.Length);
                if (i > 0)
                {
                    code.AppendLine();
                }

                code.Append(lineText);
            }

            return code.ToString();
        }
    }
}