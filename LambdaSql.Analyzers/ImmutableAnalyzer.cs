using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace LambdaSql.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ImmutableAnalyzer : DiagnosticAnalyzer
    {
        internal const string MESSAGE_FORMAT = "'{0}' is immutable and '{1}' will not have any effect on it. Consider using the return value from '{1}'.";

        private static readonly DiagnosticDescriptor _rule = new DiagnosticDescriptor("LSql1000",
            "Do not ignore values returned by methods on immutable objects.",
            MESSAGE_FORMAT,
            "Immutability",
            DiagnosticSeverity.Error, isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(_rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(compilationStartContext =>
            {
                var lambdaSqlAssembly = compilationStartContext.Compilation.GetTypeByMetadataName("LambdaSql.SqlSelect")?.ContainingAssembly;
                compilationStartContext.RegisterSyntaxNodeAction(analysisContext =>
                    Analyze(analysisContext, lambdaSqlAssembly), SyntaxKind.InvocationExpression);
            });
        }

        private static void Analyze(SyntaxNodeAnalysisContext context, IAssemblySymbol lambdaSqlAssembly)
        {
            var invocation = (InvocationExpressionSyntax)context.Node;
            var symbolInfo = context.SemanticModel.GetSymbolInfo(invocation, context.CancellationToken).Symbol;
            if (symbolInfo != null && ReferenceEquals(symbolInfo.ContainingAssembly, lambdaSqlAssembly) && invocation.Parent.IsKind(SyntaxKind.ExpressionStatement))
            {
                var diagnostic = Diagnostic.Create(_rule, invocation.GetLocation(),
                    symbolInfo.ContainingType.ToString(), symbolInfo.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
