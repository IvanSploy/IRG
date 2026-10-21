using System.Collections.Generic;

namespace IRG.Graphs
{
    public abstract class Node<TData> : NodeAction where TData : NodeData
    {
        protected TData Data;

        public sealed override void SetData(NodeData data)
        {
            Data = (TData)data;
        }
    }

    public abstract class NodeAction
    {
        private readonly Dictionary<string, List<NodeAction>> _nextNodes = new();
        private int _readyNodes;
        private int _requiredNodes;
        public bool IsActive { get; private set; }
        public bool IsReady => _readyNodes == _requiredNodes;
        public bool IsDone { get; private set; }
        private string _outPortId;
        
        public void Reset(string outPortId = null)
        {
            IsActive = false;
            IsDone = false;
            _outPortId = outPortId;
            _readyNodes = 0;
            _requiredNodes = 0;
            _nextNodes.Clear();
        }
        
        public void ConnectTo(string fromPortId, NodeAction to)
        {
            to._requiredNodes++;
            _nextNodes.AddToList(fromPortId, to);
        }
        
        public void AddReadyNode()
        {
            IsActive = true;
            _readyNodes++;
        }
        
        public abstract void SetData(NodeData data);
        
        public virtual void OnStart() { }
        public virtual void OnUpdate() { }
        public virtual void OnEnd() { }
        
        public List<NodeAction> GetNextNodes()
        {
            _nextNodes.TryGetValue(_outPortId, out var nextActions);
            foreach (var pair in _nextNodes)
            {
                if(pair.Key == _outPortId) continue;
                foreach (var nextAction in pair.Value)
                {
                    nextAction.RemoveRequiredNode();
                }
            }
            return nextActions;
        }
        
        private void RemoveRequiredNode()
        {
            _requiredNodes--;
            if (_requiredNodes != 0) return;
            foreach (var next in _nextNodes)
            {
                foreach (var nextAction in next.Value)
                {
                    nextAction.RemoveRequiredNode();
                }
            }
        }

        protected void SetDone(string outNode = "next")
        {
            IsDone = true;
            _outPortId = outNode;
        }
    }
}

