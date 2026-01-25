using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Editor
{
    [GraphNode(group: "Time")]
    public class DelayNode : GraphNode<DelayData>
    {
        private FloatField _delay;
        
        protected override string GetTitle() => "Delay";
        protected override Color HeaderColor => new(0f, 0.56f, 1f);

        protected override void OnDraw()
        {
            /* Extension Container */
            _delay = new FloatField()
            {
                label = "Delay Seconds",
                value = Data.Delay,
            };
            _delay.RegisterValueChangedCallback(evt => Data.Delay = evt.newValue);
            extensionContainer.Add(_delay);
        }
    }
}