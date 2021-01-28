using System.Text;
using Markdig.Syntax;
using CodeBlock = Nullkooland.Client.Controls.CodeBlock;
using CodeBlockSyntax = Markdig.Syntax.CodeBlock;

namespace Nullkooland.Client.Services.Markdown.Renderers
{
    public class CodeBlockRenderer : ComponentObjectRenderer<CodeBlockSyntax>
    {
        protected override void Write(ComponentRenderer renderer, CodeBlockSyntax codeBlock)
        {
            if (codeBlock is FencedCodeBlock fencedCodeBlock)
            {
                renderer.Builder.OpenComponent<CodeBlock>(0);

                string language = fencedCodeBlock.Info;
                renderer.Builder.AddAttribute(1, "Language", language);
                string code = ExtractSourceCode(codeBlock);
                renderer.Builder.AddAttribute(2, "Code", code);

                renderer.Builder.CloseComponent();
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
                if (slice.Text == null) continue;

                string lineText = slice.Text.Substring(slice.Start, slice.Length);
                if (i > 0) code.AppendLine();

                code.Append(lineText);
            }

            return code.ToString();
        }
    }
}