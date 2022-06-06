using System.Diagnostics;
using ParallelDFS.Dfs.Parallel;
using ParallelDFS.Dfs.Sequential;

namespace ParallelDFS
{
    public static class Program
    {
        private static List<int> testCases = new List<int> {100, 1000, 1500, 2000, 2500, 3000, 5000};
        private static List<int> threadsVariants = new List<int> {6, 12, 24, 48};

        
        public static void Main()
        {
            var generator = new GraphGenerator();
            var stopwatch = new Stopwatch();
            var parallelDfsFinder = new ParallelDfsPathFinder();
            var dfsFinder = new DeijkstraPathFinder();

            foreach (var testCaseNodeCount in testCases)
            {
                var graph = generator.Generate(testCaseNodeCount);
                var topNode = graph.Nodes.First();
                foreach (var threadsVariant in threadsVariants)
                {

                    stopwatch.Start();
                    parallelDfsFinder.Process(graph, topNode, threadsVariant);
                    stopwatch.Stop();
            
                    Print(stopwatch.Elapsed, "Parallel", testCaseNodeCount, threadsVariant);
            
                    stopwatch.Reset();
                }
                
                stopwatch.Start();
                dfsFinder.Process(graph, topNode);
                stopwatch.Stop();
            
                Print(stopwatch.Elapsed, "Seq", testCaseNodeCount);
                Console.WriteLine();
                    
                stopwatch.Reset();
            }
        }

        private static void Print(TimeSpan ts, string algo, int nodeCount, int threadCount = 1)
        {
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine($"{algo}, nodes: {nodeCount}, threads: {threadCount}, time: {elapsedTime}");
        }
    }
}