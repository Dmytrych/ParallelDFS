using System.Collections.Concurrent;
using ParallelDFS.DfsBase;

namespace ParallelDFS.Dfs.Parallel;

public class ClosestNotVisitedNodeProvider
{
    private readonly int usedThreadCount;
    
    public ClosestNotVisitedNodeProvider(int usedThreadCount)
    {
        this.usedThreadCount = usedThreadCount;
    }
    
    public Node Get(IReadOnlyCollection<Node> visitedNodes, ConcurrentDictionary<Node, PathInfo> paths)
    {
        var batches = CollectionHelper.Split(usedThreadCount, paths.Keys.ToList());
        
        using var countDownEvent = new CountdownEvent(batches.Count);
        var closestNodesFromBatches = new BlockingCollection<Node>();

        foreach (var batch in batches)
        {
            var callback = new WaitCallback((callback) =>
            {
                var result = GetClosestNotVisitedNode(visitedNodes, batch, paths);
                if (result != null)
                {
                    closestNodesFromBatches.Add(result);
                }
                countDownEvent.Signal();
            });
            ThreadPool.QueueUserWorkItem(callback);
        }

        countDownEvent.Wait();

        var closestNode = GetClosestNotVisitedNode(visitedNodes, closestNodesFromBatches, paths);

        return closestNode;
    }
    
    private Node GetClosestNotVisitedNode(IReadOnlyCollection<Node> visitedNodes, IReadOnlyCollection<Node> nodesToProcess, IReadOnlyDictionary<Node, PathInfo> paths)
    {
        Node closestNode = null;
        foreach (var node in nodesToProcess)
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
}