using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs
{
    [Serializable]
    public abstract class NodeData
    {
        [OnlyDebug] public string ID;
        [HideInNode] public string GroupID;
        [HideInNode] public Vector2 Position;

        public NodeData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return (NodeData)JsonUtility.FromJson(json, GetType());
        }

        public virtual VisualElement OnDraw() => null;
        public virtual void OnRefresh(VisualElement ve) {}
    }
}
