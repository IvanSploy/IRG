using System.Collections.Generic;
using UnityEngine;

namespace IRG.Graphs
{
    public class GraphData : ScriptableObject
    {
        public InitialNodeData InitialNode;
        [SerializeReference] public List<NodeData> Nodes = new();
        public List<EdgeData> Edges = new();
        public List<GroupData> Groups = new();

        public void Clear()
        {
            InitialNode = null;
            Nodes.Clear();
            Edges.Clear();
            Groups.Clear();
        }
        
        
    }
}
