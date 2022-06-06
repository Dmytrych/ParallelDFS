namespace ParallelDFS.DfsBase;

public class PathInfo
{
    public int PathLength { get; set; }

    public IReadOnlyCollection<Edge> Path { get; set; } = new List<Edge>();
}