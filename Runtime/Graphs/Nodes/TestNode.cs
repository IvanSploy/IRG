using System;
using UnityEngine;

namespace IRG.Graphs
{
    [Node("Test", Color = "gray", Filter = "Test"), Serializable]
    public class TestData : NodeData
    {
        public int IntField;
        public uint UIntField;
        public long LongField;
        public ulong ULongField;
        public float FloatField;
        public double DoubleField;
        public string StringField;
        public Bounds BoundsField;
        public BoundsInt BoundsIntField;
        public Vector2 Vector2Field;
        public Vector2Int Vector2IntField;
        public Vector3 Vector3Field;
        public Vector3Int Vector3IntField;
        public Vector4 Vector4Field;
        public GUID GUIDField;
        public Rect RectField;
        public RectInt RectIntField;
    }
    
    public class Test : Node<TestData>
    {
        public override void OnStart()
        {
            SetDone();
        }
    }
}