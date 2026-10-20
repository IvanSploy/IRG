using System;

namespace IRG.Graphs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeAttribute : Attribute
    {
        public string Title;
        public PortType OutputPortType;
        public string Color = string.Empty;
        public string Filter = string.Empty;
        public string Group = string.Empty;
        
        public NodeAttribute() { }
        public NodeAttribute(string title)
        {
            Title = title;
        }
    }
}