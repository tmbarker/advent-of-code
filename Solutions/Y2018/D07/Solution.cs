using System.Text;
using System.Text.RegularExpressions;
using Utilities.Graph;

namespace Solutions.Y2018.D07;

[PuzzleInfo("The Sum of Its Parts", Topics.Graphs, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var edges = ParseInputLines(parseFunc: ParseEdge);
        var graph = new DirectedGraph<char>(edges);
        
        return part switch
        {
            1 => TopologicalSort(graph),
            2 => TopologicalSortTimed(graph, baseStepTime: 60, agents: 5),
            _ => PuzzleNotSolvedString
        };
    }

    private static string TopologicalSort(DirectedGraph<char> graph)
    {
        var initialSteps = graph.Sources;
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
        var initialSteps = graph.Sources;
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