using MixedReality.Toolkit.UX;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Windows;
using static Unity.VisualScripting.Member;

[System.Serializable]
public class SceneGraphData
{
    public Dictionary<int, List<int>> Ingoing { get; set; }
    public Dictionary<int, List<int>> Outgoing { get; set; }
    public Dictionary<int, List<int>> Labels { get; set; }
    public List<string> Unmovable { get; set; }
    public List<List<double>> Pose { get; set; }
    public double MinConfidence { get; set; }
    public string Pcd { get; set; }
    public string Mesh { get; set; }
    public Dictionary<int, ObjectNode> Nodes { get; set; }
    public static SceneGraphData CreateGraphFromJson(string jsonString)
    {
        return JsonUtility.FromJson<SceneGraphData>(jsonString);
    }
}

[System.Serializable]
public class ObjectNode
{
    public string Type { get; set; }
    public string Label { get; set; }
    public List<double> Centroid { get; set; }
    public List<double> Color { get; set; }
    public string MeshMask { get; set; }
    public string Pcd { get; set; }
}

[System.Serializable]
public class DrawerNode : ObjectNode
{
    public int BelongsTo { get; set; }
    public List<double> Normal { get; set; }
}
