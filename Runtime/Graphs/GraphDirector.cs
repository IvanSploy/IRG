using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IRG.Graphs
{
    public abstract class GraphDirector : MonoBehaviour
    {
        private InitialNodeAction _initialNode;
        private readonly Dictionary<string, NodeAction> _nodes = new();
        private readonly List<Coroutine> _threads = new();
        private int _finishedThreads;
        
        private GraphData _graphData;

        public void Load(GraphData graphData)
        {
            if (!graphData) return;
            _graphData = graphData;
            CreateNodes();
            RePlay();
        }
        
        public void Stop()
        {
            if (_finishedThreads == _threads.Count) return;
            foreach (var thread in _threads)
            {
                if(thread == null) continue;
                StopCoroutine(thread);
            }

            _finishedThreads = _threads.Count;
        }
        
        public void RePlay()
        {
            Stop();
            Initialize();
            if(_initialNode == null) return;

            _threads.Clear();
            _finishedThreads = 0;
            foreach (var node in _initialNode.GetNextNodes())
            {
                _threads.Add(StartCoroutine(PlayThread(node)));
            }
        }
        
        private IEnumerator PlayThread(NodeAction initialNode)
        {
            NodeAction current = initialNode;
            while (current != null)
            {
                current.AddReadyNode();
                while (!current.IsReady) yield return null;
                current.OnInit();
                while(!current.OnUpdate()) yield return null;
                current.OnEnd();
                var nextNodes = current.GetNextNodes();
                current = null;
                if (nextNodes is not { Count: > 0 }) continue;

                if (nextNodes[0].IsActive) nextNodes[0].AddReadyNode();
                else current = nextNodes[0];

                foreach (var nextNode in nextNodes.Skip(1))
                {
                    if(nextNode.IsActive) nextNode.AddReadyNode();
                    else _threads.Add(StartCoroutine(PlayThread(nextNode)));
                }
            }

            _finishedThreads++;
        }

        private void CreateNodes()
        {
            _initialNode = new InitialNodeAction();
            
            _nodes.Clear();
            foreach (var nodeData in _graphData.Nodes)
            {
                var node = GenericFactory<NodeAction>.Create(nodeData.GetType());
                node.SetData(nodeData);
                _nodes.Add(nodeData.ID, node);
            }
        }

        private void Initialize()
        {
            if (!_graphData) return;
            foreach (var nodeAction in _nodes.Values)
            {
                nodeAction.Reset();
            }
            
            foreach (var edgeData in _graphData.Edges)
            {
                var fromNode = edgeData.FromNodeID == "initial_node" ? _initialNode : _nodes[edgeData.FromNodeID];
                var toNode = _nodes[edgeData.ToNodeID];
                fromNode.ConnectTo(toNode, edgeData.FromPortID);
            }
        }
    }
}

