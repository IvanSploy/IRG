using Editor;
using IRG.Graphs;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public static class OpenGraph
{
    [OnOpenAsset]
    private static bool OnOpenAsset(EntityId entityId)
    {
        var asset = EditorUtility.EntityIdToObject(entityId);
        if (asset is not GraphData graphData) return false;

        GraphEditorTool.Open(graphData);
        return true;
    }
}

