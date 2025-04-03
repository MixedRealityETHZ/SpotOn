using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace RobotsDATA
{
    public enum RobotStatus
    {
        IDLE = 0,
        BUSY = 1
    }

    public class RobotData
    {
        [JsonProperty("robots")]
        public Dictionary<string, RobotNode> Robots { get; set; }

        public static RobotData CreateRobotListFromJSON(string jsonString)
        {
            return JsonConvert.DeserializeObject<RobotData>(jsonString);
        }
    }

    public class RobotNode
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("position")]
        public List<float> Position { get; set; }

        [JsonProperty("rotation")]
        public List<float> Rotation { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("has_arm")]
        public bool HasArm { get; set; }

        [JsonProperty("battery")]
        public int Battery { get; set; }

        [JsonIgnore]
        public RobotStatus Robot_Status { get; set; }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Robot_Status = ProcessRobotStatus(Status);
        }

        private RobotStatus ProcessRobotStatus(string status)
        {
            return status switch
            {
                "STOPPED" => RobotStatus.IDLE,
                "BUSY" => RobotStatus.BUSY,
                _ => RobotStatus.IDLE
            };
        }

        public string stringifyStatus()
        {
            return Status switch
            {
                "STOPPED" => "IDLE",
                "BUSY" => "BUSY",
                _ => "IDLE"
            };
        }

        public static RobotNode CreateRobotFromJSON(string jsonString)
        {
            return JsonConvert.DeserializeObject<RobotNode>(jsonString);
        }
    }
}
