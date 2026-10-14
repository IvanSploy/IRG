using System;
using System.Collections.Generic;
using System.Linq;
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

        public void Isolate()
        {
            InitialNode = null;
            
            Dictionary<string, string> legacyIds = new Dictionary<string, string>();
            
            foreach (GroupData group in Groups)
            {
                string prev = group.ID;
                group.ID = Guid.NewGuid().ToString();
                legacyIds.Add(prev, group.ID);
            }
            
            foreach (NodeData node in Nodes)
            {
                string prev = node.ID;
                node.ID = Guid.NewGuid().ToString();
                legacyIds.Add(prev, node.ID);
                if(!string.IsNullOrEmpty(node.GroupID)) node.GroupID = legacyIds.GetValueOrDefault(node.GroupID);
            }

            foreach (EdgeData edge in Edges.ToList())
            {
                if(!string.IsNullOrEmpty(edge.FromNodeID)) edge.FromNodeID = legacyIds.GetValueOrDefault(edge.FromNodeID);
                if(!string.IsNullOrEmpty(edge.ToNodeID)) edge.ToNodeID = legacyIds.GetValueOrDefault(edge.ToNodeID);
                if (string.IsNullOrEmpty(edge.FromNodeID) || string.IsNullOrEmpty(edge.ToNodeID))
                {
                    Edges.Remove(edge);
                }
            }
        }

        public void SetRelativePos(Vector2 pos)
        {
            Vector2 minPos;
            minPos.x = float.MaxValue;
            minPos.y = float.MaxValue;

            foreach (var group in Groups)
            {
                if(minPos.x > group.Position.x) minPos.x = group.Position.x;
                if(minPos.y > group.Position.y) minPos.y = group.Position.y;
            }
            
            foreach (var node in Nodes)
            {
                if(minPos.x > node.Position.x) minPos.x = node.Position.x;
                if(minPos.y > node.Position.y) minPos.y = node.Position.y;
            }

            minPos -= pos;
            
            foreach (var group in Groups)
            {
                group.Position -= minPos;
            }
            
            foreach (var node in Nodes)
            {
                node.Position -= minPos;
            }
        }
        
        
    }
}
