using System.Collections.Generic;
using UnityEngine;

namespace IRG.Graphs
{
    public class GraphData : ScriptableObject
    {
        public InitialNodeData InitialNode;
        [SerializeReference] public List<NodeData> Nodes;
        public List<EdgeData> Edges;
        public List<GroupData> Groups;

        public void Initialize()
        {
            Nodes = new List<NodeData>();
            Edges = new List<EdgeData>();
            Groups = new List<GroupData>();
        }
    }
}
