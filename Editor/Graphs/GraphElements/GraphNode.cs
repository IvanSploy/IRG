using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Editor
{
    public abstract class GraphNode<TData> : GraphNode where TData : NodeData, new()
    {
        protected TData Data = new();

        protected Label _debugId;
        
        public override NodeData ToData()
        {
            OnSave();
            var copy = Data.Clone();
            copy.ID = ID;
            copy.Position = GetPosition().position;
            copy.GroupID = Group?.ID;
            return copy;
        }
        
        public override void FromData(NodeData nodeData)
        {
            Data = (TData)nodeData.Clone();
            Initialize(Data.Position, Data.ID);
            OnLoad();
        }
        
        protected override void OnDrawData()
        {
            if (Graph.IsDebug)
            {
                _debugId = new Label
                {
                    text = Data.ID
                };
                extensionContainer.Add(_debugId);
            }
            
            OnDraw();
        }
        protected virtual void OnDraw(){}
    }
    
    public abstract class GraphNode : Node, IConvertible<NodeData>
    {
        protected Graph _graph;
        
        public string ID { get; private set; }
        public GraphGroup Group { get; set; }
        public Port InputPort { get; private set; }
        public List<Port> OutputPorts { get; } = new();

        protected virtual bool HasInput => true;
        protected virtual bool HasOutput => true;

        private Label _title;

        protected abstract string GetTitle();
        protected virtual Color TitleColor => Color.white;
        protected virtual Color HeaderColor => Color.black;
        
        public void Initialize(Vector2 pos, string id = null)
        {
            ID = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            SetPosition(new Rect(pos, Vector2.zero));
            style.minWidth = 150;
            style.maxWidth = 300;
            this.Q("contents").style.backgroundColor = Color.HSVToRGB(0f, 0f, 0.2f);
            OnInit();
        }
        
        protected virtual void OnInit(){}
        protected virtual void OnLoad(){}
        protected virtual void OnSave(){}

        public void SetGraphView(Graph graph)
        {
            _graph = graph;
        }

        public void Draw()
        {
            /* Title Container */
            _title = new Label($"  {GetTitle()}");
            _title.style.flexGrow = 1f;
            _title.style.unityTextAlign = TextAnchor.MiddleLeft;
            _title.style.unityFontStyleAndWeight = FontStyle.Bold;
            _title.style.marginLeft = 5;
            _title.style.color = TitleColor;
            TextShadow textShadow = new TextShadow();
            textShadow.color = Color.black;
            textShadow.offset = Vector2.left;
            _title.style.textShadow = textShadow;
            
            titleContainer.Insert(0, _title);
            titleContainer.style.backgroundColor = HeaderColor;

            /* Top Container */
            if (HasInput)
            {
                InputPort = CreatePort("In", direction: Direction.Input);
                InputPort.SetID("In");
                inputContainer.Add(InputPort);
                inputContainer.style.flexGrow = 0f;
            }
            
            /* Output Container */
            if (HasOutput)
            {
                var portID = "next";
                Port outPort = CreatePort("Next");
                outPort.SetID(portID);
                outputContainer.Add(outPort);
                outputContainer.style.flexShrink = 1f;
                OutputPorts.Add(outPort);
            }

            OnDrawData();
            
            RefreshExpandedState();
        }
        
        protected virtual void OnDrawData(){}

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            
            evt.menu.AppendAction("Disconnect Input Ports", _ => DisconnectInputPorts());
            evt.menu.AppendAction("Disconnect Output Ports", _ => DisconnectOutputPorts());
            
            evt.menu.AppendSeparator();
        }

        public Port GetOutputPort(string portID)
        {
            foreach (var port in OutputPorts)
            {
                if (port.GetID() == portID) return port;
            }
            return null;
        }

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }
        
        public void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }

        public void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }
        

        protected void DisconnectPorts(VisualElement visualElement)
        {
            var edges = new List<Edge>();
            foreach (var child in visualElement.Children())
            {
                if (child is Port port)
                {
                    if(!port.connected) continue;
                    edges.AddRange(port.connections);
                }
            }
            _graph.DeleteElements(edges);
        }
        
        protected Port CreatePort(string text = "", Orientation orientation = Orientation.Horizontal,
            Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Multi)
        {
            Port port = InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.portName = text;
            port.AddManipulator(new EdgeConnector<Edge>(new EdgeConnectorManipulator(_graph)));
            return port;
        }

        public abstract NodeData ToData();
        public abstract void FromData(NodeData data);
    }
}