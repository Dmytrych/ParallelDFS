namespace ParallelDFS.DfsBase;

public class Node
{
    public int Id { get; set; }
    
    public IReadOnlyCollection<Edge> Edges { get; set; } = new List<Edge>();
}