using System;
using UnityEngine;

namespace IRG.Graphs
{
    [Serializable]
    public abstract class NodeData
    {
        public string ID;
        public string GroupID;
        public Vector2 Position;

        public NodeData Clone()
        {
            return (NodeData)MemberwiseClone();
        }
    }
}
