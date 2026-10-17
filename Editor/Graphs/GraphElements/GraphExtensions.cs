using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace IRG.Graphs.Editor
{
    public static class GraphExtensions
    {
        public static void SetID(this Port port, string id)
        {
            port.userData = id;
        }
        
        public static string GetID(this Port port)
        {
            return port.userData as string;
        }

        public static bool IsFullyConnected(this Edge edge)
        {
            return edge.input != null
                   && edge.output != null
                   && edge.input.connections.Contains(edge) && edge.output.connections.Contains(edge);
        }
        
        public static List<Edge> GetConnectedEdges(this Node node)
        {
            var edges = new List<Edge>();
            foreach (var visualElement in node.inputContainer.Children())
            {
                if (visualElement is Port port)
                {
                    if(!port.connected) continue;
                    edges.AddRange(port.connections);
                }
            }
            
            foreach (var visualElement in node.outputContainer.Children())
            {
                if (visualElement is Port port)
                {
                    if(!port.connected) continue;
                    edges.AddRange(port.connections);
                }
            }

            return edges;
        }
    }
}