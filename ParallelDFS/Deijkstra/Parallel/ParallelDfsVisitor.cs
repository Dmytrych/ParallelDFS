using System.Collections.Concurrent;
using ParallelDFS.DfsBase;

namespace ParallelDFS.Dfs.Parallel;

public class ParallelDfsVisitor
{
    private readonly int threadsToUse;
    
    public ParallelDfsVisitor(int threadsToUse)
    {
        this.threadsToUse = threadsToUse;
    }
    
    public void Visit(Node node, ConcurrentDictionary<Node, PathInfo> paths)
    {
        var edgeBatches = CollectionHelper.Split(threadsToUse, node.Edges);

        using var countDownEvent = new CountdownEvent(edgeBatches.Count);

        foreach (var batch in edgeBatches)
        {
            var callback = new WaitCallback((callback) =>
            {
                VisitInternal(node, batch, paths);
                countDownEvent.Signal();
            });
            ThreadPool.QueueUserWorkItem(callback);
        }

        countDownEvent.Wait();
    }
    
    private void VisitInternal(Node node, IReadOnlyCollection<Edge> edgesToProcess, ConcurrentDictionary<Node, PathInfo> paths)
    {
        foreach (var edge in edgesToProcess)
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
                paths.GetOrAdd(edge.To, GetUpdatedPathInfo(edge, paths));
            }
        }
    }
    
    private PathInfo GetUpdatedPathInfo(Edge visitedEdge, ConcurrentDictionary<Node, PathInfo> paths)
    {
        return new PathInfo
        {
            Path = paths[visitedEdge.From].Path.Append(visitedEdge).ToList(),
            PathLength = paths[visitedEdge.From].PathLength + visitedEdge.Value
        };
    }
}