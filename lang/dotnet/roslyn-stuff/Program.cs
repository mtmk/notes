using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

var filePath = "C:\\Users\\mtmk\\src\\notes\\lang\\dotnet\\roslyn-stuff\\Class1.cs"; // Replace with your actual file path
var code = File.ReadAllText(filePath);
var tree = CSharpSyntaxTree.ParseText(code);
var root = tree.GetRoot();

var walker = new MethodWalker();
walker.Visit(root);

class MethodWalker : CSharpSyntaxWalker
{
    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        var lineSpan = node.GetLocation().GetLineSpan();
        var start = lineSpan.StartLinePosition.Line;
        var end = lineSpan.EndLinePosition.Line;
        var count = end - start;

        Console.WriteLine($"Method found: {node.Identifier.ValueText}, number of lines: {count}");

        base.VisitMethodDeclaration(node);
    }
}
