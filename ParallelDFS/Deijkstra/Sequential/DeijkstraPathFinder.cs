using System.Diagnostics;
using ParallelDFS.DfsBase;

namespace ParallelDFS.Dfs.Sequential;

public class DeijkstraPathFinder
{
    public void Process(Graph graph, Node startNode)
    {
        var paths = new Dictionary<Node, PathInfo>();
        paths.Add(startNode, new PathInfo
        {
            Path = new List<Edge>(),
            PathLength = 0
        });
        Visit(startNode, paths);
        var visitedNodes = new List<Node> { startNode };

        while (true)
        {
            var closestNode = GetClosestNotVisitedNode(visitedNodes, paths);

            if (closestNode != null)
            {
                Visit(closestNode, paths);
                visitedNodes.Add(closestNode);
            }
            else
            {
                return;
            }
        }
    }

    private Node GetClosestNotVisitedNode(IReadOnlyCollection<Node> visitedNodes, Dictionary<Node, PathInfo> paths)
    {
        Node closestNode = null;
        foreach (var node in paths.Keys)
        {
            if (visitedNodes.Contains(node))
            {
                continue;
            }

            if (closestNode == null || paths[closestNode].PathLength > paths[node].PathLength)
            {
                closestNode = node;
            }
        }

        return closestNode;
    }

    private void Visit(Node node, Dictionary<Node, PathInfo> paths)
    {
        foreach (var edge in node.Edges)
        {
            if (paths.TryGetValue(edge.To, out PathInfo existingPathInfo))
            {
                var newPathLength = paths[node].PathLength + edge.Value;
                if (existingPathInfo.PathLength > newPathLength)
                {
                    paths[edge.To] = GetUpdatedPathInfo(edge, paths);
                }
            }
            else
            {
                paths.Add(edge.To, GetUpdatedPathInfo(edge, paths));
            }
        }
    }

    private PathInfo GetUpdatedPathInfo(Edge visitedEdge, Dictionary<Node, PathInfo> paths)
    {
        return new PathInfo
        {
            Path = paths[visitedEdge.From].Path.Append(visitedEdge).ToList(),
            PathLength = paths[visitedEdge.From].PathLength + visitedEdge.Value
        };
    }
}