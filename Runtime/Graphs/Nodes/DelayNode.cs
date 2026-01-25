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

        public override void OnInit()
        {
            _timer = Data.Delay;
        }

        public override bool OnUpdate()
        {
            if (_timer <= 0)
            {
                return true;
            }

            _timer -= Time.deltaTime;
            return false;
        }
    }
}