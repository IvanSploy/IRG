using System;
using System.Collections.Generic;
using System.Linq;
using IRG.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace IRG.Graphs.Editor
{
    [UxmlElement]
    public partial class Graph : GraphView
    {
        [UxmlAttribute] public string Filter { get; set; } = "";

        private readonly Dictionary<string, Dictionary<string, GraphNode>> _groupedNodes = new();
        private GraphSearchWindow _searchWindow;

        //Used for Search Window when edge creation failed.
        private Edge _pendingEdge;
        private Port _pendingPort;
        
        public Graph()
        {
            RegisterGroupCallbacks();
            RegisterDeleteSelection();
            RegisterGraphViewChanged();
                
            AddManipulators();
            AddBackground();
            
            AddStyle();

            schedule.Execute(Initialize);
        }

        private void Initialize()
        {
            AddSearchWindow();
        }

        private void RegisterGroupCallbacks()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                GraphGroup cinematicGroup = (GraphGroup) group;
                cinematicGroup.title = newTitle;
            };

            elementsAddedToGroup = (group, elements) =>
            {
                GraphGroup groupElement = (GraphGroup) group;
                foreach (GraphElement element in elements)
                {
                    if (element is not GraphNode nodeElement) continue;
                    AddGroupedNode(nodeElement, groupElement);
                }
            };

            elementsRemovedFromGroup = (group, elements) =>
            {
                GraphGroup groupElement = (GraphGroup) group;
                foreach (GraphElement element in elements)
                {
                    if (element is not GraphNode nodeElement) continue;
                    RemoveGroupedNode(nodeElement, groupElement);
                }
            };
        }
        
        private void RegisterDeleteSelection()
        {
            deleteSelection = (s, b) =>
            {
                foreach (var selectable in selection.ToList())
                {
                    if(selectable is InitialNode) continue;
                    
                    if (selectable is GraphElement graphElement)
                    {
                        if (selectable is Edge edge)
                        {
                            edge.input.Disconnect(edge);
                            edge.output.Disconnect(edge);
                        }

                        if (selectable is Node node)
                        {
                            DeleteElements(node.GetConnectedEdges());
                        }
                        RemoveElement(graphElement);
                    }

                    if (selectable is GraphGroup group)
                    {
                        if (_groupedNodes.TryGetValue(group.ID, out var nodeList))
                        {
                            foreach (var groupNode in nodeList.Values.ToList())
                            {
                                RemoveGroupedNode(groupNode, group);
                            }
                        }
                    }
                }
            };
        }
        
        private void RegisterGraphViewChanged()
        {
            graphViewChanged = changes =>
            {
                if (changes.elementsToRemove == null) return changes;
                foreach (var graphElement in changes.elementsToRemove)
                {
                    if (graphElement is not GraphNode node) continue;
                    if (node.Group != null)
                    {
                        RemoveGroupedNode(node, node.Group);
                    }
                }

                return changes;
            };
        }

        public void AddGroupedNode(GraphNode graphNode, GraphGroup group)
        {
            graphNode.Group = group;

            if (!_groupedNodes.ContainsKey(group.ID))
            {
                _groupedNodes.Add(group.ID, new Dictionary<string, GraphNode>());
            }

            _groupedNodes[group.ID][graphNode.ID] = graphNode;
        }

        public void RemoveGroupedNode(GraphNode graphNode, GraphGroup group)
        {
            if (!_groupedNodes.ContainsKey(group.ID)) return;

            graphNode.Group = null;
            _groupedNodes[group.ID].Remove(graphNode.ID);
        }
        
        private void AddBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }
        
        private void AddSearchWindow()
        {
            if (!_searchWindow)
            {
                _searchWindow = ScriptableObject.CreateInstance<GraphSearchWindow>();
                _searchWindow.Initialize(typeof(GraphNode), Filter, OnSearchSelection);
            }

            nodeCreationRequest = context =>
            {
                EndConnection();
                OpenSearchWindow(context.screenMousePosition);
            };
        }

        private bool OnSearchSelection(SearchTreeEntry entry, SearchWindowContext context)
        {
            var pos = GetLocalMousePosition(context.screenMousePosition);
            Type type = (Type)entry.userData;
            GraphNode graphNode = CreateNode(type, pos);
            
            if (graphNode == null) return false;
            
            AddElement(graphNode);
                    
            if (_pendingEdge == null) return true;
                    
            Port nodePort;
            if (_pendingPort.direction == Direction.Input)
            {
                if (graphNode.OutputPorts.Count == 0) return true;
                _pendingEdge.output = nodePort = graphNode.OutputPorts[0];
                _pendingEdge.input = _pendingPort;
            }
            else
            {
                _pendingEdge.input = nodePort = graphNode.InputPort;
                _pendingEdge.output = _pendingPort;
            }

            _pendingPort.Connect(_pendingEdge);
            nodePort.Connect(_pendingEdge);

            AddElement(_pendingEdge);
            EndConnection();
                    
            return true;
        }
        
        public void StartConnection(Edge edge, Vector2 pos)
        {
            _pendingEdge = edge;
            _pendingPort = _pendingEdge.input ?? _pendingEdge.output;
            OpenSearchWindow(pos);
        }

        public void EndConnection()
        {
            _pendingEdge = null;
            _pendingPort = null;
        }

        public void OpenSearchWindow(Vector2 pos)
        {
            SearchWindow.Open(new SearchWindowContext(pos), _searchWindow);
        }

        private void AddStyle()
        {
            var styleSheet = Resources.Load<StyleSheet>("GridBackground");
            styleSheets.Add(styleSheet);
            
            style.flexGrow = 1f;
        }
        
        public GraphGroup CreateGroup(string title, Vector2 pos)
        {
            var graphGroup = new GraphGroup();
            graphGroup.Initialize(title, pos);
            return graphGroup;
        }
        
        public GraphGroup CreateGroup(GroupData groupData)
        {
            var graphGroup = new GraphGroup();
            graphGroup.FromData(groupData);
            return graphGroup;
        }
        
        public new void Clear()
        {
            DeleteElements(graphElements);
        }

        public void ClearAll()
        {
            base.Clear();
        }

        public GraphData CreateNew(string folder)
        {
            GraphData graphData = AssetsIO.Create<GraphData>(folder);
            graphData.Clear();
            return graphData;
        }

        public GraphData Load(string folder, string fileName, bool displayPopUp = true)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            
            GraphData graphData = AssetsIO.Load<GraphData>(AssetsIO.Combine(folder, fileName));
            if (!graphData)
            {
                if (displayPopUp)
                {
                    EditorUtility.DisplayDialog(
                        "Could not find the file!",
                        "The file at the following path could not be found:\n\n" +
                        $"\"Assets/{folder}/{fileName}\".\n\n" +
                        "Make sure you chose the right file and it's placed at the folder path mentioned above.",
                        "Okay"
                    );
                }

                return null;
            }
            
            Clear();
            
            var groups = new Dictionary<string, GraphGroup>();
            foreach (var groupData in graphData.Groups)
            {
                GraphGroup group = CreateGroup(groupData);
                AddElement(group);
                groups[groupData.ID] = group;
            }

            var graphNodes = new Dictionary<string, GraphNode>();
            if(graphData.InitialNode != null)
            {
                var graphNode = CreateInitialNode(graphData.InitialNode);
                AddElement(graphNode);
                graphNodes[graphNode.ID] = graphNode;

                if (!string.IsNullOrEmpty(graphData.InitialNode.GroupID))
                {
                    GraphGroup group = groups[graphData.InitialNode.GroupID];
                    graphNode.Group = group;
                    group.AddElement(graphNode);
                }
            }
            
            foreach (NodeData nodeData in graphData.Nodes)
            {
                if(nodeData == null) continue;
                
                var graphNode = CreateNode(nodeData);
                AddElement(graphNode);
                graphNodes.Add(graphNode.ID, graphNode);

                if (string.IsNullOrEmpty(nodeData.GroupID)) 
                    continue;

                GraphGroup group = groups[nodeData.GroupID];
                graphNode.Group = group;
                group.AddElement(graphNode);
            }
            
            foreach (var edgeData in graphData.Edges)
            {
                if(string.IsNullOrEmpty(edgeData.FromNodeID) || string.IsNullOrEmpty(edgeData.ToNodeID)) continue;

                if (!graphNodes.TryGetValue(edgeData.FromNodeID, out var fromNode)) continue;
                var fromPort = fromNode.GetOutputPort(edgeData.FromPortID);
             
                if(fromPort == null) continue;
                
                if(!graphNodes.TryGetValue(edgeData.ToNodeID, out var toNode)) continue;
                var toPort = toNode.InputPort;
                
                if(toPort == null) continue;
                
                Edge edge = fromPort.ConnectTo(toPort);
                AddElement(edge);
                fromNode.RefreshPorts();
            }

            return graphData;
        }

        public GraphData Save(string folder, string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            
            string path = AssetsIO.Combine(folder, fileName);
            GraphData graphData = AssetsIO.Load<GraphData>(path);
            if (!graphData) graphData = AssetsIO.Create<GraphData>(path);
            
            graphData.Clear();
            
            graphElements.ForEach(graphElement =>
            {
                switch (graphElement)
                {
                    case InitialNode initialNode:
                    {
                        graphData.InitialNode = (InitialNodeData)initialNode.ToData();
                    } break;
                    case GraphNode node:
                    {
                        graphData.Nodes.Add(node.ToData());
                    } break;
                    case GraphGroup group: 
                    { 
                        graphData.Groups.Add(group.ToData());
                    } break;
                    case Edge edge:
                    {
                        var fromNode = (GraphNode)edge.output.node;
                        var fromPortID = edge.output.GetID();
                        var toNode = (GraphNode)edge.input.node;
            
                        graphData.Edges.Add(new EdgeData
                        {
                            FromNodeID = fromNode.ID,
                            FromPortID = fromPortID,
                            ToNodeID = toNode.ID
                        });
                    } break;
                }
            });
            
            AssetsIO.Save(graphData);
            return graphData;
        }

        public InitialNode CreateInitialNode(InitialNodeData nodeData)
        {
            var initialNode = new InitialNode();
            nodeData.ID = "initial_node";
            initialNode.FromData(nodeData);
            initialNode.SetGraphView(this);
            initialNode.Draw();
            return initialNode;
        }
        
        public GraphNode CreateNode(NodeData nodeData)
        {
            var graphNode = GenericFactory<GraphNode>.Create(nodeData.GetType());
            graphNode.FromData(nodeData);
            graphNode.SetGraphView(this);
            graphNode.Draw();
            return graphNode;
        }

        private T CreateNode<T>(Vector2 pos, bool shouldDraw = true) where T : GraphNode => CreateNode(typeof(T), pos, shouldDraw) as T;

        private GraphNode CreateNode(Type type, Vector2 pos, bool shouldDraw = true)
        {
            var element = Activator.CreateInstance(type);
            if (element is not GraphNode nodeElement) return null;
            
            nodeElement.SetGraphView(this);
            nodeElement.Initialize(pos);
            if (shouldDraw)
            {
                nodeElement.Draw();
            }
            return nodeElement;
        }

        private Vector2 GetLocalMousePosition(Vector2 worldPos)
        {
            var localPos = contentViewContainer.WorldToLocal(worldPos);
            return localPos;
        }
        
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            ports.ForEach(port =>
                {
                    if (port == startPort) return; //If same port.
                    if (port.node == startPort.node) return; //If same node port.
                    if (port.direction == startPort.direction) return; //If direction is the same (input-input).
                    compatiblePorts.Add(port);
                }
            );
            return compatiblePorts;
        }

        #region Manipulators
        
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            this.AddManipulator(CreateGroupContextualMenu());
        }

        public void AddNodeToMenu<T>(string title) where T : GraphNode
        {
            this.AddManipulator(CreateNodeContextualMenu<T>(title));
        }
        
        private IManipulator CreateGroupContextualMenu()
        {
            return new ContextualMenuManipulator(
                menuEvent =>
                {
                    menuEvent.menu.AppendAction("Add Group",
                        action =>
                        {
                            var group = CreateGroup("Group", GetLocalMousePosition(action.eventInfo.localMousePosition));
                            
                            AddElement(group);
                            
                            foreach (var selectable in selection.ToList())
                            {
                                if (selectable is GraphNode node)
                                {
                                    group.AddElement(node);
                                }
                            }
                        });
                }
            );
        }

        private IManipulator CreateNodeContextualMenu<T>(string actionTitle) where T : GraphNode
        {
            return new ContextualMenuManipulator(
                menuEvent =>
                {
                    menuEvent.menu.AppendAction(actionTitle,
                        action =>
                        {
                            T node = CreateNode<T>(GetLocalMousePosition(action.eventInfo.localMousePosition));
                            AddElement(node);

                            //If group selected, add to group.
                            ISelectable groupElement = selection.FirstOrDefault(selectable => selectable is Group);
                            if(groupElement is GraphGroup group)
                            {
                                group.AddElement(node);
                                AddGroupedNode(node, group);
                            }
                        });
                }
            );
        }
        
        #endregion
    }

}
