using System;
using System.Collections.Generic;
using UnityEngine;

namespace IRG.Graphs
{
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
    
    [NodeConfig("NodeConfigs/TestIn")]
    [Serializable]
    public class TestListData : NodeData
    {
        public TestClassObject ClassObject;
        public TestObject TestObject;
        public string[] ArrayString;
        [OutputField] public List<string> ListStringField;
        [OutputField] public List<int> ListIntField;
        [InputField] public List<TestObject> ListStructField;
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