using System;
using System.Collections.Generic;
using System.Linq;
using ParallelDFS.DfsBase;

namespace ParallelDFS
{
    public class AlternativeGraphGenerator
    {
        private int MaxEdgeValue = 1000;
         
        public Graph Generate(int nodeCount)
        {
            var nodes = new List<Node>();
            var maximumEdgesCount = (int) (nodeCount / 2);
            var minimumEdgesCount = 2;
    
            for (int i = 0; i < nodeCount; i++)
            {
                nodes.Add(new Node { Id = i });
            }
    
            for (int i = 0; i < nodeCount; i++)
            {
                SetEdges(nodes[i], i, nodes, maximumEdgesCount, minimumEdgesCount);
            }
    
            return new Graph(nodes);
        }
    
        private void SetEdges(Node processedNode, int nodeIndex, List<Node> graphNodes, int maximumEdgesCount, int minimumEdgesCount)
        {
            if (nodeIndex + 2 >= graphNodes.Count)
            {
                return;
            }

            var edges = new List<Edge>();
            var random = new Random();
            var maxNodeIndex = graphNodes.Count - 1;
            var generatedEdgesCount = random.Next(minimumEdgesCount, maximumEdgesCount);
            var connectedNodeIndexes = Enumerable
                .Repeat(0, generatedEdgesCount)
                .Select(i => random.Next(nodeIndex + 2, maxNodeIndex))
                .ToArray();

            edges.AddRange(connectedNodeIndexes.Select(index => CreateEdge(processedNode, graphNodes[index], random)));

            processedNode.Edges = edges;

            // for (int i = 0; i < generatedEdgesCount; i++)
            // {
            //     var randomEdgeValue = random.Next(MaxEdgeValue);
            //
            //     edges.Add(new Edge(randomEdgeValue, processedNode, GetRandomNode(random, graphNodes, processedNode, edges)));
            // }
            //
            // processedNode.Edges = edges;
        }

        private Edge CreateEdge(Node startNode, Node endNode, Random random)
            => new Edge(random.Next(MaxEdgeValue), startNode, endNode);
    }
}