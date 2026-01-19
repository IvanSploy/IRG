using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace IRG.Utils
{
    public static class NSUtils
    {
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
