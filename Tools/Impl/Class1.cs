using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;

/*
class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: dotnet run <SolutionPath> <ProjectName>");
            return;
        }

        var slnPath = Path.GetFullPath(args[0]);
        var targetProjectName = args[1];

        if (!File.Exists(slnPath))
        {
            Console.WriteLine($"Solution not found: {slnPath}");
            return;
        }

        var solution = SolutionFile.Parse(slnPath);
        var dependencies = GetTransitiveDependencies(solution, targetProjectName);

        Console.WriteLine($"Dependencies of '{targetProjectName}' (based on .sln build order):");
        foreach (var dep in dependencies)
        {
            var proj = solution.ProjectsInOrder.FirstOrDefault(p => p.ProjectName == dep);
            if (proj == null || !File.Exists(proj.AbsolutePath))
            {
                Console.WriteLine($"  [!] Project not found: {dep}");
                continue;
            }

            var msbuildProj = new Project(proj.AbsolutePath);
            var outputPath = msbuildProj.GetPropertyValue("OutputPath");
            var outputDir = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(proj.AbsolutePath)!, outputPath));

            Console.WriteLine($"  {dep} → {outputDir}");
        }
    }

    static HashSet<string> GetTransitiveDependencies(SolutionFile sln, string projectName)
    {
        var graph = new Dictionary<string, List<string>>();

        foreach (var proj in sln.ProjectsInOrder)
        {
            if (!graph.ContainsKey(proj.ProjectName))
                graph[proj.ProjectName] = new List<string>();

            if (proj.ProjectDependencies != null)
            {
                foreach (var depGuid in proj.ProjectDependencies)
                {
                    var depProj = sln.ProjectsInOrder.FirstOrDefault(p => p.ProjectGuid == depGuid);
                    if (depProj != null)
                    {
                        graph[proj.ProjectName].Add(depProj.ProjectName);
                    }
                }
            }
        }

        var visited = new HashSet<string>();
        var stack = new Stack<string>();
        stack.Push(projectName);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (visited.Add(current))
            {
                if (graph.TryGetValue(current, out var children))
                {
                    foreach (var child in children)
                        stack.Push(child);
                }
            }
        }

        visited.Remove(projectName); // 自分自身は除外
        return visited;
    }
}

*/