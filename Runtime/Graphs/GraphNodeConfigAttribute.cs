using System;

namespace IRG.Graphs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GraphNodeConfigAttribute : Attribute
    {
        public string Path;
        
        public GraphNodeConfigAttribute(string path)
        {
            Path = path;
        }
    }
}