using System.Collections.Generic;
using IRG.Editor;
using UnityEditor;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace IRG.Graphs.Editor
{
    public static class GraphIO
    {
        public static GraphData Create(string folderName)
        {
            GraphData graphData = AssetsIO.Create<GraphData>(folderName);
            graphData?.Initialize();
            return graphData;
        }

        public static GraphData Load(CustomGraphView graphView, string fileName, bool displayPopUp = true)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            
            GraphData graphData = AssetsIO.Load<GraphData>(AssetsIO.Combine(graphView.Folder, fileName));

            if (graphData == null)
            {
                if (displayPopUp)
                {
                    EditorUtility.DisplayDialog(
                        "Could not find the file!",
                        "The file at the following path could not be found:\n\n" +
                        $"\"Assets/{graphView.Folder}/{fileName}\".\n\n" +
                        "Make sure you chose the right file and it's placed at the folder path mentioned above.",
                        "Okay"
                    );
                }
                return null;
            }

            graphView.ClearGraph();
            
            var groups = new Dictionary<string, GraphGroup>();
            foreach (var groupData in graphData.Groups)
            {
                GraphGroup group = graphView.CreateGroup(groupData);
                graphView.AddElement(group);
                groups[groupData.ID] = group;
            }

            var nodes = new Dictionary<string, GraphNode>();
            if(graphData.InitialNode != null)
            {
                var graphNode = graphView.CreateInitialNode(graphData.InitialNode);
                graphView.AddElement(graphNode);
                nodes[graphNode.ID] = graphNode;

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
                
                var graphNode = graphView.CreateNode(nodeData);
                graphView.AddElement(graphNode);
                nodes.Add(graphNode.ID, graphNode);

                if (string.IsNullOrEmpty(nodeData.GroupID)) 
                    continue;

                GraphGroup group = groups[nodeData.GroupID];
                graphNode.Group = group;
                group.AddElement(graphNode);
            }
            
            foreach (var edgeData in graphData.Edges)
            {
                if(string.IsNullOrEmpty(edgeData.FromNodeID) || string.IsNullOrEmpty(edgeData.ToNodeID)) continue;

                if (!nodes.TryGetValue(edgeData.FromNodeID, out var fromNode)) continue;
                var fromPort = fromNode.GetOutputPort(edgeData.FromPortID);
             
                if(fromPort == null) continue;
                
                if(!nodes.TryGetValue(edgeData.ToNodeID, out var toNode)) continue;
                var toPort = toNode.InputPort;
                
                if(toPort == null) continue;
                
                Edge edge = fromPort.ConnectTo(toPort);
                graphView.AddElement(edge);
                fromNode.RefreshPorts();
            }
            
            return graphData;
        }
        
        public static void Save(CustomGraphView graphView, string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;
            
            string path = AssetsIO.Combine(graphView.Folder, fileName);
            GraphData graphData = AssetsIO.Load<GraphData>(path);
            if (graphData == null)
            { 
                graphData = AssetsIO.Create<GraphData>(path);
            }
            graphData.Initialize();
            
            graphView.graphElements.ForEach(graphElement =>
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
        }
    }
}