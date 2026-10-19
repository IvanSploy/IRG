using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace IRG.Graphs.Editor
{
    public sealed class GraphNode : Node
    {
        private readonly Graph _graph;
        private NodeData _data;
        private GraphNodeContext _context;
        
        public string ID { get; private set; }
        public bool IsInitialNode => ID == "initial_node";
        public GraphGroup Group { get; set; }
        public Port InputPort { get; private set; }
        public List<Port> OutputPorts { get; } = new();

        private Label _title;
        
        public GraphNode(Graph graph)
        {
            _graph = graph;
        }
        
        public void Init(GraphNodeContext context, Vector2 pos, string id = null)
        {
            _context = context;
            ID = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            SetPosition(new Rect(pos, Vector2.zero));
            OnInit?.Invoke();
        }
        
        public void Init(Type type, Vector2 pos, string id = null)
        {
            Init(GraphNodeContext.Get(type), pos, id);
            _data = (NodeData)Activator.CreateInstance(type);
            Serialize(_data);
            OnLoad?.Invoke();
        }
        
        public void Init(NodeData data)
        {
            Init(GraphNodeContext.Get(data.GetType()), data.Position, data.ID);
            _data = data.Clone();
            OnLoad?.Invoke();
        }
        
        public event Action OnInit;

        public event Action OnLoad;
        public event Action OnSave;

        public void Draw()
        {
            this.Q("node-border").style.flexShrink = 0f;
            this.Q("contents").style.backgroundColor = Color.HSVToRGB(0f, 0f, 0.2f);
            
            style.minWidth = _context.MinWidth;
            style.maxWidth = _context.MaxWidth;
            
            /* Title Container */
            _title = new Label($"  {_context.Title}");
            _title.style.flexGrow = 1f;
            _title.style.unityTextAlign = TextAnchor.MiddleLeft;
            _title.style.unityFontStyleAndWeight = FontStyle.Bold;
            _title.style.marginLeft = 5;
            _title.style.color = _context.TitleColor;
            TextShadow textShadow = new TextShadow();
            textShadow.color = Color.black;
            textShadow.offset = Vector2.left;
            _title.style.textShadow = textShadow;
            
            titleContainer.Insert(0, _title);
            titleContainer.style.backgroundColor = _context.HeaderColor;

            /* Top Container */
            if (_context.HasInput)
            {
                InputPort = CreatePort("In", direction: Direction.Input);
                InputPort.SetID("In");
                inputContainer.Add(InputPort);
                inputContainer.style.flexGrow = 0f;
            }
            
            /* Output Container */
            if (_context.HasOutput)
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

        private void OnDrawData()
        {
            if (_data == null) return;
            var drawers = NodeFieldDrawerFactory.GetDrawers(_data);
            foreach (var drawer in drawers)
            {
                if(drawer.IsDebug && !Graph.IsDebug) continue;
                var ve = drawer.Build();
                extensionContainer.Add(ve);
                drawer.OnBeforeValueChanged += () => _graph.RecordState();
            }
        }

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

        private void DisconnectPorts(VisualElement visualElement)
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
        
        private Port CreatePort(string text = "", Orientation orientation = Orientation.Horizontal,
            Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Multi)
        {
            Port port = InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.portName = text;
            port.AddManipulator(new EdgeConnector<Edge>(new EdgeConnectorManipulator(_graph)));
            return port;
        }

        public NodeData Serialize()
        {
            OnSave?.Invoke();
            Serialize(_data);
            return _data.Clone();
        }
        
        private void Serialize(NodeData data)
        {
            data.ID = ID;
            data.Position = GetPosition().position;
            data.GroupID = Group?.ID;
        }
    }
}