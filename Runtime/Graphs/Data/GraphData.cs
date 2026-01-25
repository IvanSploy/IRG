using System.Collections.Generic;
using UnityEngine;

namespace IRG.Graphs
{
    public class GraphData : ScriptableObject
    {
        public string Name;
        public InitialNodeData InitialNode;
        [SerializeReference] public List<NodeData> Nodes;
        public List<EdgeData> Edges;
        public List<GroupData> Groups;

        public void Initialize(string fileName)
        {
            Name = fileName;
            Nodes = new();
            Edges = new();
            Groups = new();
        }
    }
}
