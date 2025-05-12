using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using Cozyupk.HelloShadowDI.ShadowPkg.ComponentMeta.Attributes;

namespace Cozyupk.HelloShadowDI.ShadowPkg.ComponentMeta.Utils.Impl2
{
    /// <summary>
    /// Analyzes assembly dependencies and generates a dependency graph.
    /// </summary>
    public class AssemblyDependencyAnalyzer
    {
        /// <summary>
        /// Creates a graph of assembly dependencies based on the provided assemblies.
        /// </summary>
        /// <param name="assemblies">Array of assemblies to analyze.</param>
        /// <returns>A dictionary representing the dependency graph.</returns>
        private static Dictionary<string, HashSet<string>> CreateAssemblyGraph(Assembly[] assemblies)
        {
            // Map of assembly names to Assembly objects
            var assemblyDict = assemblies.ToDictionary(a => a.GetName().FullName);

            // Dependency graph (name-based)
            var graph = new Dictionary<string, HashSet<string>>();

            foreach (var asm in assemblies)
            {
                // Retrieve custom attribute to determine if the assembly is a debug target
                var parameters = asm.GetCustomAttributes(typeof(DebuggableApplicationParametersAttribute), false)
                                    .OfType<DebuggableApplicationParametersAttribute>()
                                    .FirstOrDefault();

                if (parameters == null || !parameters.IsDebugOutputTargetBool)
                {
                    continue;
                }

                var name = asm.GetName().FullName;
                var refs = new HashSet<string>(
                    asm.GetReferencedAssemblies()
                        .Select(r => r.FullName)
                        .Where(n => assemblyDict.ContainsKey(n)) // Only include loaded assemblies
                );

                graph[name] = refs;
            }

            return graph;
        }

        /// <summary>
        /// Generates a dependency graph starting from the specified root assembly.
        /// </summary>
        /// <param name="rootAsmName">The full name of the root assembly to start the graph generation.</param>
        /// <param name="graph">The dictionary representing the dependency graph where keys are assembly names and values are their dependencies.</param>
        /// <param name="result">A list to store the string representation of the dependency graph.</param>
        /// <returns>Void. The result is appended to the provided list.</returns>
        private static void GenerateDependencyGraph(
            string rootAsmName,
            Dictionary<string, HashSet<string>> graph,
            List<string> result
        )
        {
            var stack = new Stack<(string current, int depth)>();
            var visited = new Dictionary<string, int>();

            // Perform a depth-first traversal of the graph
            stack.Push((rootAsmName, 0));
            while (0 < stack.Count)
            {
                var (asmName, depth) = stack.Pop();
                if (!visited.TryGetValue(asmName, out var visitedDepth)
                    || visitedDepth < depth)
                {
                    visited[asmName] = depth;
                }
                if (graph.TryGetValue(asmName, out var refs))
                {
                    foreach (var r in refs)
                    {
                        stack.Push((r, depth + 1));
                    }
                }
            }

            // Remove transitive edges to simplify the graph
            PruneTransitiveEdges(rootAsmName, graph);

            // Generate a tree-like representation of the graph
            GenerateAssemblyTree(rootAsmName, graph, result);
        }

        /// <summary>
        /// Removes transitive edges from the dependency graph.
        /// </summary>
        /// <param name="node">The current node being processed.</param>
        /// <param name="graph">The dependency graph.</param>
        /// <returns>A set of all descendants of the node.</returns>
        private static HashSet<string> PruneTransitiveEdges(string node, Dictionary<string, HashSet<string>> graph)
        {
            var closure = new HashSet<string>();

            if (!graph.TryGetValue(node, out var children) || children.Count == 0)
            {
                return closure;
            }

            // Recursively retrieve descendants for each child node
            var allProgeny = new HashSet<string>();
            foreach (var child in children)
            {
                // Since this feature is only used in debug builds, 
                // the possibility of circular references is not checked 
                // and recursion depth is not restricted.
                var descendants = PruneTransitiveEdges(child, graph);
                foreach (var d in descendants)
                {
                    allProgeny.Add(d);
                }
            }

            // Remove transitive children (descendants of other children)
            var toRemove = children.Where(c => allProgeny.Contains(c)).ToList();
            foreach (var r in toRemove)
            {
                children.Remove(r);
            }

            // Add current direct children to the closure
            foreach (var child in children)
            {
                closure.Add(child);
            }
            // Also include descendants of the children in the closure
            closure.UnionWith(allProgeny);

            return closure;
        }

        /// <summary>
        /// Generates a tree-like representation of the assembly dependencies.
        /// </summary>
        /// <param name="asmName">The full name of the assembly to generate the tree for.</param>
        /// <param name="graph">The dictionary representing the dependency graph where keys are assembly names and values are their dependencies.</param>
        /// <param name="result">A list to store the string representation of the dependency tree.</param>
        /// <param name="prefix">The prefix for formatting the tree structure.</param>
        /// <param name="isLast">Indicates whether the current node is the last child in its level.</param>
        /// <param name="isRoot">Indicates whether the current node is the root of the tree.</param>
        /// <returns>Void. The result is appended to the provided list.</returns>
        private static void GenerateAssemblyTree(
            string asmName,
            Dictionary<string, HashSet<string>> graph,
            List<string> result,
            string prefix = "",
            bool isLast = true,
            bool isRoot = true
        )
        {
            var branch = isRoot ? "" : isLast ? "┗" : "┣";
            var nextPrefix = prefix + (isRoot ? "" : isLast ? "  " : "┃ ");

            // Retrieve assembly information for display
            var infoText = GetAssemblyInfoText(asmName);

            result.Add(isRoot ? $"{infoText}" : $"{prefix}{branch} {infoText}");

            if (!graph.TryGetValue(asmName, out var refs))
            {
                return;
            }

            // Sort references for consistent output
            var arrRefs = refs
                .OrderBy(r => r.Contains('[') ? 0 : 1) // Prioritize items containing "["
                .ThenBy(r => r, StringComparer.OrdinalIgnoreCase) // Alphabetical order (case-insensitive)
                .ToList();
            for (int i = 0; i < arrRefs.Count; i++)
            {
                var r = arrRefs[i];
                bool isChildLast = i == arrRefs.Count - 1;

                GenerateAssemblyTree(r, graph, result, nextPrefix, isChildLast, false);
            }

            return;
        }

        /// <summary>
        /// Retrieves detailed information about an assembly.
        /// </summary>
        /// <param name="asmName">The name of the assembly.</param>
        /// <returns>A string containing assembly information.</returns>
        private static string GetAssemblyInfoText(string asmName)
        {
            var asm = AppDomain.CurrentDomain
                        .GetAssemblies()
                        .FirstOrDefault(a => a.GetName().FullName == asmName);

            if (asm == null)
            {
                return asmName + " (not loaded)";
            }

            // Retrieve custom attribute to determine if the assembly is a debug target
            var parameters = asm
                .GetCustomAttributes(typeof(DebuggableApplicationParametersAttribute), false)
                .OfType<DebuggableApplicationParametersAttribute>()
                .FirstOrDefault();

            if (parameters == null || !parameters.IsDebugOutputTargetBool)
            {
                return asm.GetName().Name; // Return simple name if not a debug target
            }

            // Retrieve TargetFrameworkAttribute
            var tfAttr = asm.GetCustomAttributes(typeof(TargetFrameworkAttribute), false)
                            .OfType<TargetFrameworkAttribute>()
                            .FirstOrDefault();

            string tfText = tfAttr?.FrameworkName ?? "(unknown)";

            // Check if the assembly depends on PresentationFramework.dll (WPF indicator)
            bool usesWpf = asm.GetReferencedAssemblies()
                              .Any(r => r.Name == "PresentationFramework");

            return $"{asm.GetName().Name} [TFM={tfText}{(usesWpf ? ", using WPF" : "")}]";
        }

        /// <summary>
        /// Generates the dependency graph for the current AppDomain.
        /// </summary>
        /// <param name="targetAssemblyFullName">Optional target assembly name to focus on.</param>
        /// <returns>A List of string representation of the dependency graph.</returns>
        public List<string> GetDependencyGraph(string? targetAssemblyFullName = null)
        {
#if DEBUG
            var graph = CreateAssemblyGraph(AppDomain.CurrentDomain.GetAssemblies());

            IEnumerable<string> roots;
            if (targetAssemblyFullName != null)
            {
                roots = new[] { targetAssemblyFullName };
            }
            else
            {
                // Identify root assemblies (not referenced by any other assembly)
                var allTargets = graph.SelectMany(kvp => kvp.Value);
                roots = graph.Keys.Except(allTargets);
            }

            var result = new List<string>();
            foreach (var root in roots)
            {
                GenerateDependencyGraph(root, graph, result);
            }
            return result;
#else
            return "Dependency graph generation is disabled in non-debug mode.";
#endif
        }
    }
}
