using System;
using System.Collections.Generic;
using IRG.Graphs.Drawers;
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
        private int _inputCount;
        private int _outputCount;

        private struct PortElement
        {
            public VisualElement Extension;
            public Button Remove;
        }
        
        public string ID { get; private set; }
        public bool IsInitialNode => ID == "initial_node";
        public GraphGroup Group { get; set; }
        public Port InputPort { get; private set; }
        private readonly List<PortElement> _inputExtensions = new();
        public List<Port> OutputPorts { get; } = new();
        private readonly List<PortElement> _outputExtensions = new();

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
            OnLoad();
        }
        
        public void Init(NodeData data)
        {
            Init(GraphNodeContext.Get(data.GetType()), data.Position, data.ID);
            _data = data.Clone();
            OnLoad();
        }
        
        public event Action OnInit;
        
        public event Func<VisualElement> OnAdd;
        public event Action<int> OnRemove;

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
            if (_context.InputType == PortType.Fixed)
            {
                _inputCount = _context.InputIds.Count;
                foreach (var portName in _context.InputIds)
                {
                    string id = portName.ToSnakeCase();
                    InputPort = CreatePort(portName, id, direction: Direction.Input);
                    InputPort.SetID(id);
                    inputContainer.Add(InputPort);
                    inputContainer.style.flexGrow = 0f;
                }
            }
            
            /* Output Container */
            if (_context.OutputType == PortType.Fixed)
            {
                foreach (var portName in _context.OutputIds)
                {
                    AddOutPort(portName);
                }
            }
            else if (_context.OutputType == PortType.Custom)
            {
                /* Main Container */
                Button addChoiceButton = new Button
                {
                    text = "Add Choice",
                };
                addChoiceButton.clicked += () =>
                {
                    _graph.RecordState("Graph Added Choice");
                    AddOutPort();
                    
                    if (OnAdd == null) return;
                    foreach (var del in OnAdd.GetInvocationList())
                    {
                        var callback = (Func<VisualElement>)del;
                        VisualElement ve = callback.Invoke();
                        _outputExtensions[^1].Extension.Add(ve);
                    }
                };
                mainContainer.Insert(1, addChoiceButton);
            }

            OnDrawData();
            
            RefreshExpandedState();
        }

        private void OnDrawData()
        {
            if (_data == null) return;
            
            extensionContainer.Clear();
            
            var dataVe = _data.OnDraw();
            
            var drawers = NodeFieldDrawerFactory.CreateDrawers(_data);
            foreach (var drawer in drawers)
            {
                if(drawer.IsDebug && !Graph.IsDebug) continue;
                drawer.OnBeforeValueChanged += () => _graph.RecordState();
                if(dataVe != null) drawer.OnValueChanged += () => _data.OnRefresh(dataVe);
                var ve = drawer.Build();
                if (drawer is PortListDrawer portDrawer)
                {
                    int i = 0;
                    while (ve.childCount > 0)
                    {
                        if(i >= _outputExtensions.Count)
                        {
                            if (_context.OutputType != PortType.Custom) break;
                            AddOutPort();
                        }
                        var element = ve.ElementAt(0);
                        _outputExtensions[i].Extension.Clear();
                        _outputExtensions[i].Extension.Add(element);
                        i++;
                    }
                    OnAdd += () => portDrawer.OnAdd();
                    OnRemove += portDrawer.OnRemove;
                }
                else
                {
                    extensionContainer.Add(ve);
                }
            }
            
            if(dataVe != null)
            {
                extensionContainer.Add(dataVe);
                _data.OnRefresh(dataVe);
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

            portID = portID.Replace("choice_", "out_");
            
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
        
        private Port CreatePort(string text, string id, Orientation orientation = Orientation.Horizontal,
            Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Multi)
        {
            Port port = InstantiatePort(orientation, direction, capacity, typeof(bool));
            port.portName = text;
            port.Q<Label>().tooltip = $"Code id: {id}";
            port.tooltip = $"Code id: {id}";
            port.AddManipulator(new EdgeConnector<Edge>(new EdgeConnectorManipulator(_graph)));
            return port;
        }
        
        private void Serialize(NodeData data)
        {
            data.ID = ID;
            data.Position = GetPosition().position;
            data.GroupID = Group?.ID;
        }

        public NodeData Serialize()
        {
            OnSave();
            Serialize(_data);
            return _data.Clone();
        }
        
        private void OnLoad() { }
        private void OnSave() { }
        
        private void AddOutPort(string portName = null)
        {
            var portElement = new PortElement();
            
            portName = string.IsNullOrEmpty(portName) ? $"Out {_outputCount + 1}" : portName;
            string portId = portName.ToSnakeCase();

            _outputCount++;
            
            /* Top Container */
            Port port = CreatePort(portName, portId);
            
            port.style.width = new StyleLength(StyleKeyword.Auto);
            port.style.height = new StyleLength(StyleKeyword.Auto);
            port.SetID(portId);

            port.style.alignItems = Align.FlexStart;

            var extension = new VisualElement();
            portElement.Extension = extension;
            
            extension.style.flexGrow = 1f;
            extension.style.flexShrink = 1f;
            port.Add(extension);

            if (_context.OutputType == PortType.Custom)
            {
                Button removeChoiceButton = new Button()
                {
                    text = "X",
                };
                portElement.Remove = removeChoiceButton;
            
                removeChoiceButton.style.flexGrow = 0f;
                removeChoiceButton.style.flexShrink = 0f;
                removeChoiceButton.clicked += () =>
                {
                    _graph.RecordState("Graph Removed Choice");
                    OnRemove?.Invoke(OutputPorts.IndexOf(port));
                    RemoveOutPort(port);
                };
                port.Add(removeChoiceButton);
            }
            
            _outputExtensions.Add(portElement);

            outputContainer.Add(port);
            outputContainer.style.flexShrink = 1f;
            
            OutputPorts.Add(port);
            RefreshExpandedState();
            RefreshPortIds();
        }
        
        private void RemoveOutPort(Port port)
        {
            _outputCount--;
            var portIndex = OutputPorts.IndexOf(port);
            _outputExtensions.RemoveAt(portIndex);
            _graph.DeleteElements(port.connections);
            outputContainer.Remove(port);
            OutputPorts.Remove(port);
            RefreshPortIds();
        }

        public void RefreshPortIds()
        {
            if (_context.OutputType != PortType.Custom) return;
            for (var i = 0; i < OutputPorts.Count; i++)
            {
                var port = OutputPorts[i];
                port.portName = "Out " + (i + 1);
                port.SetID($"out_{i+1}");
            }
        }
    }
}