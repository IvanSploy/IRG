using UnityEditor.Experimental.GraphView;

namespace IRG.Graphs.Editor
{
    public static class PortExtensions
    {
        public static void SetID(this Port port, string id)
        {
            port.userData = id;
        }
        
        public static string GetID(this Port port)
        {
            return port.userData as string;
        }
    }
}