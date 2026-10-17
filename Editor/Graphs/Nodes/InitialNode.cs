using UnityEngine;

namespace IRG.Graphs.Editor
{
    public sealed class InitialNode : GraphNode
    {
        protected override bool HasInput => false;
        protected override string GetTitle() => "Initial Node";
        protected override Color HeaderColor => Color.softRed;

        protected override void OnDrawNode()
        {
            style.minWidth = 100;
            style.maxWidth = 100;
        }
    }
}