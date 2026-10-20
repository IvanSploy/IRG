using System;

namespace IRG.Graphs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GraphNodeAttribute : Attribute
    {
        public string Title;
        public PortType OutputPortType;
        public string Color = string.Empty;
        public string Filter = string.Empty;
        public string Group = string.Empty;
        
        public GraphNodeAttribute() { }
        public GraphNodeAttribute(string title)
        {
            Title = title;
        }
    }
}