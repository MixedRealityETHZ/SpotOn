using System.Collections.Generic;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Linq;

namespace SceneGRAPHS
{
    public enum object_type
    {
        UNMOVABLE = 0,
        DRAWER = 1,
        DRAGGABLE = 2,
        LIGHT_SWITCH = 3,
        LAMP = 4,
        ROBOT = 5,
        BASE = 6
    }; 

    public class SceneGraphData
    {
        public Dictionary<int, List<int>> Ingoing { get; set; }
        public Dictionary<int, int> Outgoing { get; set; }
        public Dictionary<int, List<int>> Labels { get; set; }
        public List<string> Unmovable { get; set; }
        public List<List<double>> Pose { get; set; }
        public float Min_Confidence { get; set; }
        public string Pcd { get; set; }
        public string Mesh { get; set; }
        public Dictionary<int, ObjectNode> Nodes { get; set; }
        public static SceneGraphData CreateGraphFromJson(string jsonString)
        {
            return JsonConvert.DeserializeObject<SceneGraphData>(jsonString);
        }
    }

    public class ObjectNode
    {
        public string Type { get; set; }
        public string Label { get; set; }
        public List<float> Centroid { get; set; }
        public List<float> Color { get; set; }
        public string Mesh_Mask { get; set; }
        public string Pcd { get; set; }
        public string Pcd_Real{ get; set; }

        [JsonIgnore]
        public object_type Object_Type { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Mesh_Mask = ProcessMeshMask(Mesh_Mask);
            Pcd = ProcessPcd(Pcd);
            Pcd_Real = ProcessPcd(Pcd_Real);
            Object_Type = ProcessType(Type);
            Name = ProcessName(Pcd);
        }

        private string ProcessMeshMask(string meshMask)
        {
            return meshMask.Split('/').Last();
        }

        private string ProcessPcd(string pcd)
        {
            return pcd.Split('/').Last();
        }

        private object_type ProcessType(string Type)
        { 
            if (Type == "drawer") return object_type.DRAWER;
            if (Type == "lamp") return object_type.LAMP;
            if (Type == "light_switch") return object_type.LIGHT_SWITCH;
            if (Type == "unmovable") return object_type.UNMOVABLE;
            if (Type == "draggable") return object_type.DRAGGABLE;
            return object_type.UNMOVABLE;
        }

        private string ProcessName(string pcd)
        {
            return pcd.Split('.')[0];
        }
    } 
}