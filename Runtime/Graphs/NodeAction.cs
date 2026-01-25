using System.Collections.Generic;

namespace IRG.Graphs
{
    public abstract class NodeAction<TData> : NodeAction where TData : NodeData
    {
        protected TData Data;

        public sealed override void SetData(NodeData data)
        {
            Data = (TData)data;
        }
    }

    public abstract class NodeAction
    {
        private readonly Dictionary<string, List<NodeAction>> _nextActions = new();
        private int _readyNodes;
        private int _requiredNodes;
        public bool IsActive { get; private set; }
        public bool IsReady => _readyNodes == _requiredNodes;
        
        public void Reset()
        {
            IsActive = false;
            _readyNodes = 0;
            _requiredNodes = 0;
            _nextActions.Clear();
        }
        
        public void ConnectTo(NodeAction next, string portId)
        {
            next._requiredNodes++;
            _nextActions.AddToList(portId, next);
        }
        
        public void AddReadyNode()
        {
            IsActive = true;
            _readyNodes++;
        }
        
        public abstract void SetData(NodeData data);
        
        public virtual void OnInit() { }
        public virtual bool OnUpdate() => true;
        public virtual void OnEnd() { }
        
        protected virtual string GetOutPortName() => "next";
        
        public List<NodeAction> GetNextNodes()
        {
            var portName = GetOutPortName();
            _nextActions.TryGetValue(portName, out var next);
            foreach (var pair in _nextActions)
            {
                if(pair.Key == portName) continue;
                foreach (var nodeAction in pair.Value)
                {
                    nodeAction.RemoveRequiredNode();
                }
            }
            return next;
        }
        
        private void RemoveRequiredNode()
        {
            _requiredNodes--;
            if (_requiredNodes != 0) return;
            foreach (var next in _nextActions)
            {
                foreach (var nodeAction in next.Value)
                {
                    nodeAction.RemoveRequiredNode();
                }
            }
        }
    }
}

