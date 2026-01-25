using System;

namespace IRG.Graphs
{
    [Serializable] public sealed class InitialNodeData : NodeData { }
    public sealed class InitialNodeAction : NodeAction
    {
        public override void SetData(NodeData data) { }
    }
}
