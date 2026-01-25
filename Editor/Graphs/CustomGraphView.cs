using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Edge = UnityEditor.Experimental.GraphView.Edge;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace IRG.Graphs.Editor
{
    public class CustomGraphView : GraphView
    {
        public readonly string Folder;
        public readonly string Filter;
        
        private readonly EditorWindow _editorWindow;
        private GraphSearchWindow _searchWindow;
        
        public readonly Dictionary<string, Dictionary<string, GraphNode>> GroupedNodes = new();
        
        public CustomGraphView(string folder, string filter, EditorWindow editorWindow)
        {
            Folder = folder;
            Filter = filter;
            _editorWindow = editorWindow;

            RegisterGroupCallbacks();
            RegisterDeleteSelection();
            RegisterGraphViewChanged();
                
            AddManipulators();
            AddBackground();
            AddSearchWindow();
            
            AddStyle();
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
                        if (GroupedNodes.TryGetValue(group.ID, out var nodeList))
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
                if (changes.elementsToRemove != null)
                {
                    foreach (var graphElement in changes.elementsToRemove)
                    {
                        if (graphElement is GraphNode node)
                        {
                            if (node.Group != null)
                            {
                                RemoveGroupedNode(node, node.Group);
                            }
                        }
                    }
                }
                
                return changes;
            };
        }

        public void AddGroupedNode(GraphNode graphNode, GraphGroup group)
        {
            graphNode.Group = group;

            if (!GroupedNodes.ContainsKey(group.ID))
            {
                GroupedNodes.Add(group.ID, new Dictionary<string, GraphNode>());
            }

            GroupedNodes[group.ID][graphNode.ID] = graphNode;
        }

        public void RemoveGroupedNode(GraphNode graphNode, GraphGroup group)
        {
            if (!GroupedNodes.ContainsKey(group.ID)) return;

            graphNode.Group = null;
            GroupedNodes[group.ID].Remove(graphNode.ID);
        }
        
        private void AddBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }
        
        private void AddSearchWindow()
        {
            if (_searchWindow == null)
            {
                _searchWindow = ScriptableObject.CreateInstance<GraphSearchWindow>();
                _searchWindow.Initialize(typeof(GraphNode), Filter, (entry, context) =>
                {
                    var pos = GetLocalMousePosition(context.screenMousePosition, true);
                    Type type = (Type)entry.userData;
                    GraphElement graphElement;
                    if (type == typeof(GraphGroup))
                    {
                        graphElement = CreateGroup("Cinematic Group", pos);
                    }
                    else
                    {
                        graphElement = CreateNode(type, pos);
                    }
            
                    if (graphElement == null) return false;
            
                    AddElement(graphElement);
                    return true;
                });
            }
            
            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

        private void AddStyle()
        {
            var styleSheet = (StyleSheet) Resources.Load("GridBackground");
            styleSheets.Add(styleSheet);
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
        public GraphNode CreateNode(Type type, Vector2 pos, bool shouldDraw = true)
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
        
        public Vector2 GetLocalMousePosition(Vector2 worldPos, bool isSearchWindow = false)
        {
            if (isSearchWindow)
            {
                worldPos -= _editorWindow.position.position;
            }
            var localPos = contentViewContainer.WorldToLocal(worldPos);
            return localPos;
        }

        public void ClearGraph()
        {
            DeleteElements(graphElements);
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
            /* Default manipulators */
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            /* Contextual Menu */
            //Create Groups
            this.AddManipulator(CreateGroupContextualMenu());
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
        
        public IManipulator CreateNodeContextualMenu<T>(string actionTitle) where T : GraphNode
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
