using System;
using UnityEngine;

namespace IRG.Graphs
{
    [Serializable]
    public class DelayData : NodeData
    {
        public float Delay;
    }
    
    public class DelayAction : NodeAction<DelayData>
    {
        private float _timer;

        public override void OnStart()
        {
            _timer = Data.Delay;
        }

        public override void OnUpdate()
        {
            if (_timer <= 0)
            {
                SetDone();
                return;
            }

            _timer -= Time.deltaTime;
        }
    }
}