using System.Collections.Concurrent;
using ParallelDFS.DfsBase;

namespace ParallelDFS.Dfs.Parallel;

public class ParallelDfsPathFinder
{
    public void Process(Graph graph, Node startNode, int threadsToUse)
    {
        var parallelVisitedNodeProvider = new ClosestNotVisitedNodeProvider(threadsToUse);
        var parallelVisitor = new ParallelDfsVisitor(threadsToUse);
        var paths = new ConcurrentDictionary<Node, PathInfo>();
        paths.GetOrAdd(startNode, new PathInfo
        {
            Path = new List<Edge>(),
            PathLength = 0
        });

        parallelVisitor.Visit(startNode, paths);
        var visitedNodes = new List<Node> { startNode };

        while (true)
        {
            var closestNode = parallelVisitedNodeProvider.Get(visitedNodes, paths);

            if (closestNode != null)
            {
                parallelVisitor.Visit(closestNode, paths);
                visitedNodes.Add(closestNode);
            }
            else
            {
                return;
            }
        }
    }
}