using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace IRG.Graphs.Editor
{
    public static class GraphIO
    {
        private static string GetFullPath(string folderName, string fileName) => $"Assets/{folderName}/{fileName}.asset";

        public static string Create(string folderName)
        {
            int index = 0;
            string fileName;
            do
            {
                fileName = $"New Graph {index}";
                index++;
            } while (AssetDatabase.LoadAssetAtPath<GraphData>(GetFullPath(folderName, fileName)));
            GraphData graphData = CreateAsset<GraphData>(folderName, fileName);
            graphData?.Initialize(fileName);
            return fileName;
        }

        public static GraphData Load(CustomGraphView graphView, string fileName, bool displayPopUp = true)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            
            GraphData graphData = LoadAsset<GraphData>(graphView.Folder, fileName);

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
            
            CreateFolder(graphView.Folder);
            GraphData graphData = LoadAsset<GraphData>(graphView.Folder, fileName);
            if (graphData == null)
            { 
                graphData = CreateAsset<GraphData>(graphView.Folder, fileName);
            }
            graphData.Initialize(fileName);
            
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

            SaveAsset(graphData);
        }

        public static void CreateFolder(string folderName)
        {
            if (AssetDatabase.IsValidFolder($"Assets/{folderName}")) return;
            AssetDatabase.CreateFolder("Assets/", folderName);
        }
        
        public static void CreateFolder(string parent, string folderName)
        {
            if (AssetDatabase.IsValidFolder($"{parent}/{folderName}")) return;
            AssetDatabase.CreateFolder(parent, folderName);
        }

        public static void RemoveFolder(string path)
        {
            FileUtil.DeleteFileOrDirectory($"{path}.meta");
            FileUtil.DeleteFileOrDirectory($"{path}/");
        }

        public static T CreateAsset<T>(string folder, string assetName) where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(assetName)) return null;
            string fullPath = GetFullPath(folder, assetName);

            T asset = LoadAsset<T>(folder, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();

                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }

        public static T LoadAsset<T>(string folder, string assetName) where T : ScriptableObject
        {
            string fullPath = GetFullPath(folder, assetName);
            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        public static void RenameAsset(string folder, string fileName, string newName)
        {
            var path = GetFullPath(folder, fileName);
            AssetDatabase.RenameAsset(path, newName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void SaveAsset(Object asset)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void RemoveAsset(string folder, string assetName)
        {
            AssetDatabase.DeleteAsset(GetFullPath(folder, assetName));
        }
    }
}