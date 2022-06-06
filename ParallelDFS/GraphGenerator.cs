using ParallelDFS.DfsBase;

namespace ParallelDFS;

public class GraphGenerator
{
     private int MaxEdgeValue = 1000;
     
     public Graph Generate(int nodeCount)
     {
          var nodes = new List<Node>();
          var maximumEdgesCount = (int) (nodeCount - 1);
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
          var edges = new List<Edge>();
          var random = new Random();

          var generatedEdgesCount = random.Next(minimumEdgesCount, maximumEdgesCount);

          for (int i = 0; i < generatedEdgesCount; i++)
          {
               var randomEdgeValue = random.Next(MaxEdgeValue);

               edges.Add(new Edge(randomEdgeValue, processedNode, GetRandomNode(random, graphNodes, processedNode, edges)));
          }

          processedNode.Edges = edges;
     }

     private Node GetRandomNode(Random random, List<Node> graphNodes, Node processedNode, IReadOnlyCollection<Edge> processedEdges)
     {
          var randomGeneratedNodeIndex = random.Next(graphNodes.Count);

          while (graphNodes[randomGeneratedNodeIndex] == processedNode || processedEdges.Any(edge => edge.To == graphNodes[randomGeneratedNodeIndex]))
          {
               randomGeneratedNodeIndex = random.Next(graphNodes.Count);
          }

          return graphNodes[randomGeneratedNodeIndex];
     }


     private class MarkableNode
     {
          public bool IsMarked { get; set; }

          public Node NodeModel { get; set; } = new Node();
     }
}