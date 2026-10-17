using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace IRG.Graphs.Editor
{
    public class EdgeConnectorManipulator : IEdgeConnectorListener
    {
        private readonly Graph _graph;

        public EdgeConnectorManipulator(Graph graph)
        {
            _graph = graph;
        }

        public void OnDrop(GraphView graphView, Edge edge) { }
        
        public void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            _graph.StartConnection(edge, position);
        }
    }
}