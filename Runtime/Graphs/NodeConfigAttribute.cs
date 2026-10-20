using System;

namespace IRG.Graphs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeConfigAttribute : Attribute
    {
        public string ResourcesPath;
        
        public NodeConfigAttribute(string resourcesPath)
        {
            ResourcesPath = resourcesPath;
        }
    }
}