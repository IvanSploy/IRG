using System;

namespace IRG.Graphs.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GraphNodeAttribute : Attribute
    {
        public readonly string Filter;
        public readonly string Group;
        
        public GraphNodeAttribute(string filter = "", string group = "")
        {
            Filter = filter.ToLowerInvariant();
            Group = group;
        }
    }
}