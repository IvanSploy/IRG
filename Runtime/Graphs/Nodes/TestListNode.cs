using System;
using System.Collections.Generic;
using UnityEngine;

namespace IRG.Graphs
{
    [Serializable]
    public class TestClassObject
    {
        public long LongValue;
        public Vector2 Vector2Value;
        public Bounds BoundsValue;
    }
    
    [Serializable]
    public struct TestObject
    {
        public string Name;
        public int Cactus;
        public Rect Wakala;
    }
    
    [Node("Test List", Color = "gray2", Filter = "Test"), Serializable]
    public class TestListData : NodeData
    {
        public TestClassObject ClassObject;
        public TestObject TestObject;
        public string[] ArrayString;
        public List<string> ListStringField;
        public List<int> ListIntField;
        public List<TestObject> ListStructField;
        public List<TestClassObject> ListObjectField;
    }
    
    public class TestListAction : NodeAction<TestListData>
    {
        public override void OnStart()
        {
            SetDone();
        }
    }
}