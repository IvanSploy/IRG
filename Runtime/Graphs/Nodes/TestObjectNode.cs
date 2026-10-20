using System;
using UnityEngine;

namespace IRG.Graphs
{
    [Node("Test Object", Color = "gray", Filter = "Test"), Serializable]
    public class TestObjectNode : NodeData
    {
        public UnityEngine.Object ObjectField;
        public Sprite SpriteField;
        public RectTransform.Axis AxisEnumField;
    }
}