namespace ParallelDFS.DfsBase;

public class Graph
{
    public Graph(IReadOnlyCollection<Node> nodes)
    {
        Nodes = nodes;
    }
    
    public IReadOnlyCollection<Node> Nodes { get; set; }
}