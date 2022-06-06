namespace ParallelDFS.DfsBase;

public class Edge
{
    public Edge(int value, Node from, Node to)
    {
        Value = value;
        From = from;
        To = to;
    }
    
    public int Value { get; set; }
    
    public Node From { get; set; }
    
    public Node To { get; set; }
}