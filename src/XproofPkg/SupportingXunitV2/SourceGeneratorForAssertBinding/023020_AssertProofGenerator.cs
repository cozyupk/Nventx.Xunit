using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Xproof.SupportSourceGenerator;

namespace Xproof.SupportingXunit.SourceGeneratorForAssertBinding
{
    [Generator]
    public class AssertProofGenerator : AssertProofGeneratorBase, IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // クラス + 属性のあるやつをフィルター
            var candidateClasses = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (node, _) => IsCandidate(node),
                    transform: static (ctx, _) => GetClassInfo(ctx))
                .Where(static m => m is not null)
                .Collect();

            // 1. Collect でまとめた classInfo を一個ずつバラす
            var exploded = candidateClasses.SelectMany((classes, _) => classes);

            // 2. それを compilation と Combine する
            context.RegisterSourceOutput(exploded.Combine(context.CompilationProvider), (spc, pair) =>
            {
                var (classInfo, compilation) = pair;
                var methods = GetAssertMethods(compilation);

                foreach (var m in methods)
                {
                    if (m.GetAttributes().Any(attr =>
                    attr.AttributeClass?.ToDisplayString() == "System.ObsoleteAttribute"))
                    {
                        // skip
                        continue;
                    }
                    if (classInfo is null)
                    {
                        continue;
                    }
                    var suffix = string.Join("_", m.Parameters.Select(p => Sanitize(p.Type)));
                    var filename = $"{m.Name}_{suffix}_AssertProof.g.cs";
                    var source = GenerateCode(classInfo, m); // ← これで両方使える！

                    spc.AddSource(filename, SourceText.From(source, Encoding.UTF8));
                }
            });
        }

        private static string Sanitize(ITypeSymbol t)
        {
            var name = t.Name;

            if (t is INamedTypeSymbol named && named.IsGenericType)
            {
                name = name.Split('`')[0]; // Dictionary`2 → Dictionary
                var args = string.Join("_", named.TypeArguments.Select(Sanitize));
                return $"{name}_{args}";
            }

            if (t.TypeKind == TypeKind.Array)
                return Sanitize(((IArrayTypeSymbol)t).ElementType) + "Array";

            return name.Replace(".", "_").Replace("[]", "Array");
        }

        private static ImmutableArray<IMethodSymbol> GetAssertMethods(Compilation compilation)
        {
            var assertType = compilation.GetTypeByMetadataName("Xunit.Assert");
            if (assertType is null)
                return ImmutableArray<IMethodSymbol>.Empty;

            return assertType
                .GetMembers()
                .OfType<IMethodSymbol>()
                .Where(m => m.DeclaredAccessibility == Accessibility.Public &&
                            m.IsStatic)
                .OrderBy(m => m.Name) // 名前でソート！
                .ThenBy(m => m.Parameters.Length)
                .ThenBy(m => string.Join(",", m.Parameters.Select(p => p.Type.Name)))
                .ToImmutableArray();
        }

        private static bool IsCandidate(SyntaxNode node)
            => node is ClassDeclarationSyntax cds &&
               cds.AttributeLists.SelectMany(x => x.Attributes)
                  .Any(a => a.Name.ToString().Contains("GenerateAssertProof"));

        private static ClassInfo? GetClassInfo(GeneratorSyntaxContext context)
        {
            if (context.Node is not ClassDeclarationSyntax cds)
                return null;

            if (context.SemanticModel.GetDeclaredSymbol(cds) is not INamedTypeSymbol symbol)
                return null;

            var @namespace = symbol.ContainingNamespace.ToDisplayString();
            var name = symbol.Name;
            return new ClassInfo(@namespace, name, symbol);
        }

        override protected internal void AppendFixedHeader(StringBuilder sb)
        {
            base.AppendFixedHeader(sb);
            sb.AppendLine("// This code wraps methods from xUnit's Assert class (Apache License, Version 2.0).");
            sb.AppendLine("// xUnit is maintained by the .NET Foundation and Contributors.");
            sb.AppendLine("// https://github.com/xunit/assert.xunit");
            sb.AppendLine("// https://github.com/xunit/xunit");
            sb.AppendLine();
            sb.AppendLine("using Xunit;");
            sb.AppendLine();
        }
        private static void AppendMethodCalling(
            StringBuilder sb,
            IMethodSymbol m,
            string paramDecls,
            string paramArgs,
            string? genericDecl = null,
            string? whereClause = null)
        {
            var methodName = m.Name;
            var paramTypes = string.Join(", ", m.Parameters.Select(p => $"typeof({FormatType(p.Type)})"));

            var isGeneric = m.IsGenericMethod;
            var genericArgNames = isGeneric ? m.TypeParameters.Select(tp => tp.Name).ToList() : new();
            var genericTypesArray = string.Join(", ", genericArgNames.Select(name => $"typeof({name})"));

            sb.AppendLine($"        public {(m.ReturnsVoid ? "void" : FormatType(m.ReturnType))} {m.Name}{genericDecl}(\n"
                         + $"                       {paramDecls},\n"
                         + "                       [CallerFilePath] string? callerFilePath = null,\n"
                         + "                       [CallerLineNumber] int callerLineNumber = 0,\n"
                         + "                       [CallerMemberName] string? callerMemberName = null\n"
                         + "        )");
            if (!string.IsNullOrWhiteSpace(whereClause))
                sb.AppendLine($"\n        {whereClause}       ");
            sb.AppendLine("        {");

            if (!m.ReturnsVoid)
                sb.AppendLine($"            #pragma warning disable CS8603");

            sb.AppendLine($"            {(m.ReturnsVoid ? "" : "return ")}Probe(() => Assert.{methodName}{genericDecl}({paramArgs}),");

            sb.AppendLine($"                callerFilePath: callerFilePath,");
            sb.AppendLine($"                callerLineNumber: callerLineNumber,");
            sb.AppendLine($"                callerMemberName: callerMemberName,");

            // --- 🧠 MakeGenericMethod対応ロジック
            if (isGeneric)
            {
                sb.AppendLine($"                invokedMethodInfo: typeof(Assert)");
                sb.AppendLine($"                    .GetMethod(\"{methodName}\", new[] {{ {paramTypes} }})!");
                sb.AppendLine($"                    .MakeGenericMethod({genericTypesArray}),");
            }
            else
            {
                sb.AppendLine($"                invokedMethodInfo: typeof(Assert).GetMethod(\"{methodName}\", new[] {{ {paramTypes} }}),");
            }

            sb.AppendLine($"                invokedParameters: new object?[] {{ {paramArgs} }}");
            sb.AppendLine($"            );");

            if (!m.ReturnsVoid)
                sb.AppendLine($"            #pragma warning restore CS8603");

            sb.AppendLine("        }");
        }


        // 呼び出し側：GenerateCode(classInfo, m)
        private string GenerateCode(ClassInfo classInfo, IMethodSymbol m)
        {
            var sb = new StringBuilder();
            AppendFixedHeader(sb);                        // ヘッダ共通部（override で拡張済み）

            // ----- namespace & class -----
            sb.AppendLine($"namespace {classInfo.Namespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    partial class {classInfo.Symbol.Name}{ToAngleBracketedList(classInfo.Symbol.TypeParameters)}");
            sb.AppendLine("    {");

            // ----- メソッドシグネチャ -----
            if (m.IsGenericMethod)
            {
                // ① ジェネリック型引数
                var genericParams = string.Join(", ", m.TypeParameters.Select(tp => tp.Name));
                var genericDecl = $"<{genericParams}>";

                // ② パラメータ宣言 & 呼び出し引数
                var paramDecls = string.Join(", ", m.Parameters.Select(FormatParameterDecl));
                var paramArgs = string.Join(", ", m.Parameters.Select(p => AvoidCollision(p.Name)));

                // ③ 制約句
                var constraints = m.TypeParameters
                                   .Select(BuildConstraintClause)
                                   .Where(c => c.Length > 0);
                var whereClause = string.Join(" ", constraints);

                // ④ 出力
                AppendMethodCalling(sb, m, paramDecls, paramArgs, genericDecl, whereClause);
            }
            else
            {
                var paramDecls = string.Join(", ", m.Parameters.Select(FormatParameterDecl));
                var paramArgs = string.Join(", ", m.Parameters.Select(p => AvoidCollision(p.Name)));

                AppendMethodCalling(sb, m, paramDecls, paramArgs);
            }
            sb.AppendLine("    }");   // class
            sb.AppendLine("}");       // namespace
            return sb.ToString();


            // ---------- ローカル: 型パラメータ制約ビルダー ----------
            static string BuildConstraintClause(ITypeParameterSymbol tp)
            {
                var parts = new List<string>();

                if (tp.HasValueTypeConstraint) parts.Add("struct");
                if (tp.HasReferenceTypeConstraint) parts.Add("class");
                if (tp.HasNotNullConstraint) parts.Add("notnull");
                if (tp.HasUnmanagedTypeConstraint) parts.Add("unmanaged");
                if (tp.HasConstructorConstraint) parts.Add("new()");

                parts.AddRange(tp.ConstraintTypes.Select(FormatType));

                return parts.Count == 0
                    ? string.Empty
                    : $"where {tp.Name} : {string.Join(", ", parts)}";
            }
        }

        private class ClassInfo
        {
            public string Namespace { get; }
            public string ClassName { get; }
            public INamedTypeSymbol Symbol { get; }

            public ClassInfo(string @namespace, string className, INamedTypeSymbol symbol)
            {
                Namespace = @namespace;
                ClassName = className;
                Symbol = symbol;
            }
        }
    }
}
