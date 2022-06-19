using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ParallelDFS.Dfs.Parallel;
using ParallelDFS.Dfs.Sequential;
using ParallelDFS.DfsBase;

namespace ParallelDFS
{
    public static class Program
    {
        private static List<int> testCases = new List<int> {1000, 1500, 2000, 2500, 3000, 5000, 7500, 10000, 15000};
        private static List<int> threadsVariants = new List<int> {6, 12, 24, 48, 96};

        public static void Main()
        {
            var generator = new AlternativeGraphGenerator(); //new GraphGenerator();
            var stopwatch = new Stopwatch();
            var parallelDfsFinder = new ParallelDeijkstraPathFinder();
            var dfsFinder = new DeijkstraPathFinder();

            foreach (var testCaseNodeCount in testCases)
            {
                var graph = generator.Generate(testCaseNodeCount);
                //PrintGraph(graph);
                var topNode = graph.Nodes.First();
                foreach (var threadsVariant in threadsVariants)
                {

                    ThreadPool.SetMaxThreads(threadsVariant, threadsVariant);
                    ThreadPool.SetMinThreads(threadsVariant, threadsVariant);
                    stopwatch.Start();
                    var result = parallelDfsFinder.Process(graph, topNode, threadsVariant);
                    stopwatch.Stop();
                    //PrintDistances(result);
            
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

        private static void PrintGraph(Graph graph)
        {
            foreach (var fromNode in graph.Nodes)
            {
                var connections = new int[graph.Nodes.Count];
                foreach (var edge in fromNode.Edges)
                {
                    connections[edge.To.Id] = edge.Value;
                }

                PrintRow(fromNode.Id, connections);
            }
        }

        private static void PrintRow(int nodeIndex, int[] connections)
        {
            var line = nodeIndex + ": ";
            foreach (var connectionValue in connections)
            {
                line += connectionValue;
                line += " ";
            }
            Console.WriteLine(line);
        }

        private static void PrintDistances(ConcurrentDictionary<Node, PathInfo> results)
        {
            foreach (var node in results.Keys)
            {
                Console.WriteLine(node.Id + ": " + results[node].PathLength);
            }
        }
    }
}