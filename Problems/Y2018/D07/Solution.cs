using System.Text;
using System.Text.RegularExpressions;
using Problems.Y2018.Common;
using Utilities.Graph;

namespace Problems.Y2018.D07;

/// <summary>
/// The Sum of Its Parts: https://adventofcode.com/2018/day/7
/// </summary>
public class Solution : SolutionBase2018
{
    public override int Day => 7;
    
    public override object Run(int part)
    {
        var edges = ParseInputLines(parseFunc: ParseEdge);
        var dag = new DirectedGraph<char>(edges);
        
        return part switch
        {
            1 => TopologicalSort(graph: dag),
            2 => TopologicalSortTimed(graph: dag, baseStepTime: 60, agents: 5),
            _ => ProblemNotSolvedString
        };
    }

    private static string TopologicalSort(DirectedGraph<char> graph)
    {
        var initialSteps = graph.GetVerticesNoIncoming();
        var withPriority = initialSteps.Select(v => (v, v));
        
        var steps = new StringBuilder();
        var ready = new PriorityQueue<char, char>(items: withPriority);

        while (ready.Count > 0)
        {
            var vertex = ready.Dequeue();
            steps.Append(vertex);

            foreach (var adjacent in graph.Outgoing[vertex])
            {
                graph.RemoveEdge(vertex, adjacent);
                if (graph.Incoming[adjacent].All(v => v == vertex))
                {
                    ready.Enqueue(
                        element: adjacent,
                        priority: adjacent);
                }
            }
        }

        return steps.ToString();
    }

    private static int TopologicalSortTimed(DirectedGraph<char> graph, int baseStepTime, int agents)
    {
        var initialSteps = graph.GetVerticesNoIncoming();
        var withPriority = initialSteps.Select(v => (v, v));

        var time = 0;
        var tasks = new PriorityQueue<char, int>(); 
        var ready = new PriorityQueue<char, char>(items: withPriority);
        var complete = new HashSet<char>();

        while (ready.Count > 0 || tasks.Count > 0)
        {
            while (tasks.TryPeek(out var step, out var completeAt) && completeAt <= time)
            {
                tasks.Dequeue();
                complete.Add(step);
                
                foreach (var adjacent in graph.Outgoing[step])
                {
                    if (graph.Incoming[adjacent].All(complete.Contains))
                    {
                        ready.Enqueue(
                            element: adjacent,
                            priority: adjacent);
                    }
                }
            }

            while (ready.Count > 0 && tasks.Count < agents)
            {
                var step = ready.Dequeue();
                var completeAt = time + GetStepTime(step: step, baseStepTime: baseStepTime);

                tasks.Enqueue(step, completeAt);
            }

            if (tasks.TryPeek(out _, out var fastForwardTo))
            {
                time = fastForwardTo;
            }
        }
        
        return time;
    }

    private static int GetStepTime(char step, int baseStepTime)
    {
        return baseStepTime + step + 1 - 'A';
    }
    
    private static DirectedGraph<char>.Edge ParseEdge(string line)
    {
        var matches = Regex.Matches(line, @"\b([A-Z])\b");
        return new DirectedGraph<char>.Edge(
            From: matches[0].Value.Single(),
            To: matches[1].Value.Single());
    }
}