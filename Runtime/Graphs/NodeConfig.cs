using UnityEngine;

namespace IRG.Graphs
{
    [CreateAssetMenu(fileName = "NodeConfig", menuName = "Graphs/Node Config")]
    public class NodeConfig : ScriptableObject
    {
        public NodeContext Context = NodeContext.Default();
    }
}