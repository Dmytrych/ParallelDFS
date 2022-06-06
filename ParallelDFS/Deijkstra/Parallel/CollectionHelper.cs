namespace ParallelDFS.Dfs.Parallel;

public static class CollectionHelper
{
    public static IReadOnlyCollection<IReadOnlyCollection<T>> Split<T>(int batchCount, IReadOnlyCollection<T> items)
    {
        var batchSize = (int)Math.Ceiling((double)items.Count / batchCount);
        var batches = new List<List<T>>();

        for (int i = 0; i < batchCount; i++)
        {
            batches.Add(items.Skip(i * batchSize).Take(batchSize).ToList());
        }

        return batches;
    }
}