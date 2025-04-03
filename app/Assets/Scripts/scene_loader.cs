using System.IO;
using UnityEngine;

using static SceneGRAPHS.SceneGraphData;
using static SceneGRAPHS.ObjectNode;
using static RobotsDATA.RobotData;
using static RobotsDATA.RobotNode;
using static Drawers.Drawer;
using static Lamps.Lamp;
using static LightSWITCHES.LightSwitch;
using static Unmovables.Unmovable;
using static Draggables.Draggable;
using static SpotON.SpotonAPP;

using Unmovables;
using Draggables;
using Lamps;
using SceneGRAPHS;
using RobotsDATA;
using Drawers;
using LightSWITCHES;
using SpotON;
using MixedReality.Toolkit.SpatialManipulation;
using Robots;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;
using LanguagePROMPTS;
using MixedReality.Toolkit;
using ObjectINFORMATION;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


namespace MainSCENE
{
    public class MainScene : MonoBehaviour
    {

        private SceneGraphData scene_graph_data { get; set; }
        private RobotData robot_data { get; set; }
        private GameObject room { get; set; }
        private GameObject scene { get; set; }
        private GameObject room_plane { get; set; }
        private GameObject spotonApp { get; set; }

        private bool show_scene_graph = false;
        private ObjectInformation sceneInformation {  get; set; }

        public void Awake()
        {
            scene = this.gameObject;
            room_plane = scene.transform.Find("room_plane").gameObject;
            spotonApp = scene.transform.parent.gameObject;
            room_plane.GetComponent<ObjectManipulator>().OnClicked.AddListener(() => openMenu());
            room_plane.GetComponent<ObjectManipulator>().hoverEntered.AddListener(OnHoverEntered);
            room_plane.GetComponent<ObjectManipulator>().hoverExited.AddListener(OnHoverExited);
            sceneInformation = spotonApp.transform.Find("object_information").gameObject.GetComponent<ObjectInformation>();
        }

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            if (args.interactorObject is XRGazeInteractor) return;
            if (args.interactorObject is XRRayInteractor)
            {
                sceneInformation.set_object_information("Click on the object you have hovered over to interact with it. To enter main menu, click on the floor or just say open menu!", object_type.BASE);
                spotonApp.GetComponent<SpotonAPP>().show(show_object.OBJECT_INFORMATION, false);
            }
        }

        private void OnHoverExited(HoverExitEventArgs args)
        {
            if (args.interactorObject is XRGazeInteractor) return;
            if (args.interactorObject is XRRayInteractor)
            {
                spotonApp.GetComponent<SpotonAPP>().hide(show_object.OBJECT_INFORMATION);
            }
        }

        private void load_robots_data()
        {
            string fileName = Path.Combine(Application.streamingAssetsPath, "robot_info.json");
            byte[] bytes = UnityEngine.Windows.File.ReadAllBytes(fileName);
            string jsonString = System.Text.Encoding.ASCII.GetString(bytes);
            robot_data = RobotData.CreateRobotListFromJSON(jsonString);
        }

        private void load_robots()
        {
            foreach (var robot in robot_data.Robots)
            {
                int nodeId = int.Parse(robot.Key);
                RobotNode node = robot.Value;
                GameObject newObject = new GameObject(node.Name);
                Robot newRobot = newObject.AddComponent<Robot>();
                newObject.transform.parent = room_plane.transform;
                newRobot.Initialize(node, nodeId);
            }
        }

        private void load_scene_data()
        {
            string fileName = Path.Combine(Application.streamingAssetsPath, "scene_graph.json");
            byte[] bytes = UnityEngine.Windows.File.ReadAllBytes(fileName);
            string jsonString = System.Text.Encoding.ASCII.GetString(bytes);
            scene_graph_data = SceneGraphData.CreateGraphFromJson(jsonString);
        }

        private void load_scene()
        {
            room = new GameObject("room");
            room.transform.parent = room_plane.transform;

            room.transform.localPosition = new Vector3(0f, -1f, -5.5f);
            room.transform.localRotation = Quaternion.Euler(0, 0, 0);
            room.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            foreach (var node_dict in scene_graph_data.Nodes)
            {

                int nodeId = node_dict.Key;
                ObjectNode node = node_dict.Value;

                GameObject newObject = new GameObject(node.Name);
                newObject.transform.parent = room.transform;

                switch (node.Object_Type)
                {
                    case object_type.UNMOVABLE:
                        Unmovable newObjectUnmovable = newObject.AddComponent<Unmovable>();
                        newObjectUnmovable.Initialize(node, nodeId);
                        break;

                    case object_type.DRAWER:
                        Drawer newObjectDrawer = newObject.AddComponent<Drawer>();
                        newObjectDrawer.Initialize(node, nodeId);
                        break;

                    case object_type.DRAGGABLE:
                        Draggable newObjectDraggable = newObject.AddComponent<Draggable>();
                        newObjectDraggable.Initialize(node, nodeId);
                        break;

                    case object_type.LIGHT_SWITCH:
                        LightSwitch newObjectLightSwitch = newObject.AddComponent<LightSwitch>();
                        newObjectLightSwitch.Initialize(node, nodeId);
                        break;

                    case object_type.LAMP:
                        Lamp newObjectLamp = newObject.AddComponent<Lamp>();
                        newObjectLamp.Initialize(node, nodeId);
                        break;
                }
            }
            show_colors();
        }

        private void show_colors()
        {
            foreach (var node_dict in scene_graph_data.Nodes)
            {
                ObjectNode node = node_dict.Value;

                GameObject currentObject = room.transform.Find(node.Name).gameObject;

                if (currentObject != null)
                {
                    switch (node.Object_Type)
                    {
                        case object_type.UNMOVABLE:
                            Unmovable targetUnmovable = currentObject.GetComponent<Unmovable>();
                            targetUnmovable.show_color(show_scene_graph);
                            break;
                        case object_type.DRAWER:
                            Drawer targetObject = currentObject.GetComponent<Drawer>();
                            targetObject.show_color(show_scene_graph);
                            break;
                        case object_type.DRAGGABLE:
                            Draggable targetDraggable = currentObject.GetComponent<Draggable>();
                            targetDraggable.show_color(show_scene_graph);
                            break;
                        case object_type.LIGHT_SWITCH:
                            LightSwitch targetLightSwitch = currentObject.GetComponent<LightSwitch>();
                            targetLightSwitch.show_color(show_scene_graph);
                            break;
                        case object_type.LAMP:
                            Lamp targetLamp = currentObject.GetComponent<Lamp>();
                            targetLamp.show_color(show_scene_graph);
                            break;
                    }

                }

            }
        }

        public void openMenu()
        {
            scene.transform.parent.GetComponent<SpotonAPP>().show(show_object.START_MENU);
        }

        public void Initialize()
        {
            load_scene_data();
            load_scene();

            load_robots_data();
            load_robots();
        }

        public void restartScene()
        {
            Destroy(room);

            scene.transform.localPosition = Vector3.zero;
            scene.transform.localRotation = Quaternion.identity;
            scene.transform.localScale = Vector3.one * 0.25f;

            scene.transform.Find("room_plane").transform.localPosition = new Vector3(-1f, -2.5f, 0f);
            scene.transform.Find("room_plane").transform.localRotation = Quaternion.Euler(0, -60, 0); ;
            scene.transform.Find("room_plane").transform.localScale = Vector3.one * 1.2f;

            Initialize();
        }

        public void toggle_colors()
        {
            show_scene_graph = !show_scene_graph;
            show_colors();
        }

        public void Update()
        {

        }
    }
}
