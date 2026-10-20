using System;

namespace IRG.Graphs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeConfigAttribute : Attribute
    {
        public string Path;
        
        public NodeConfigAttribute(string path)
        {
            Path = path;
        }
    }
}