using UnityEngine;

namespace IRG.Graphs.Editor
{
    public sealed class InitialNode : GraphNode<InitialNodeData>
    {
        protected override bool HasInput => false;
        protected override string GetTitle() => "Initial Node";
        protected override Color HeaderColor => Color.softRed;
    }
}