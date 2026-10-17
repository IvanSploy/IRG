using System;

namespace IRG.Graphs
{
    [Serializable]
    public sealed class InitialNodeData : NodeData { }
    public class InitialNodeAction : NodeAction
    {
        public override void SetData(NodeData data) { }
    }
}