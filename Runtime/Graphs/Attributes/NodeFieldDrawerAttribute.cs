using System;

namespace IRG.Graphs
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class NodeFieldDrawerAttribute : Attribute
    {
        public Type Type { get; }
        public int Priority { get; set; }
        public bool AllowChildren { get; set; }

        public NodeFieldDrawerAttribute(Type type)
        {
            Type = type;
        }
        
        public NodeFieldDrawerAttribute(Type type, int priority, bool allowChildren = false)
        {
            Type = type;
            Priority = priority;
            AllowChildren = allowChildren;
        }
    }
}