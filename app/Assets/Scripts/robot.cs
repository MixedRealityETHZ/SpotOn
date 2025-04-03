using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using static SpotON.SpotonAPP;
using static RobotPROMPT.RobotPrompt;

using RobotPROMPT;
using SpotON;

using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using RobotsDATA;
using ObjectINFORMATION;
using SceneGRAPHS;
using System.Collections;
using DrawerPROMPT;


namespace Robots
{

    public class Robot : MonoBehaviour
    {
        private GameObject robotGameObject { get; set; }
        private BoxCollider robotBoxColider { get; set; }
        private RobotNode robotNode { get; set; }
        private XRSimpleInteractable robotInteractor { get; set; }
        private XRSimpleInteractable standingInteractor { get; set; }
        private XRSimpleInteractable lyingInteractor { get; set; }
        private GameObject standingSpot {  get; set; }
        private GameObject layingSpot { get; set; }
        private GameObject spotonApp { get; set; }
        private RobotPrompt robotPrompt { get; set; }
        private ObjectInformation robotInformation { get; set; }
        public int robotId { get; set; }

        private int slow_down_counter = 1;

        public void Initialize(RobotNode new_robot_node, int new_robot_id)
        {
            robotNode = new_robot_node;
            robotId = new_robot_id;
            robotGameObject = this.gameObject;

            //robotInteractor = robotGameObject.AddComponent<XRSimpleInteractable>();
            //robotInteractor.selectEntered.AddListener(OnSelectEntered);
            //robotInteractor.hoverEntered.AddListener(OnHoverEntered);
            //robotInteractor.hoverExited.AddListener(OnHoverExited);

            spotonApp = robotGameObject.transform.parent.transform.parent.transform.parent.gameObject;
            robotPrompt = spotonApp.transform.Find("robot_prompt").gameObject.GetComponent<RobotPrompt>();
            robotInformation = spotonApp.transform.Find("object_information").gameObject.GetComponent<ObjectInformation>();

            standingSpot = Instantiate(Resources.Load<GameObject>("spot_model/yellow_standing_spot"));
            layingSpot = Instantiate(Resources.Load<GameObject>("spot_model/yellow_laying_spot"));

            standingSpot.transform.Find("stitch_result_stitch_all").gameObject.AddComponent<BoxCollider>();
            layingSpot.transform.Find("stitch_result_stitch_all").gameObject.AddComponent<BoxCollider>();

            standingInteractor = standingSpot.transform.Find("stitch_result_stitch_all").gameObject.AddComponent<XRSimpleInteractable>();
            standingInteractor.selectEntered.AddListener(OnSelectEntered);
            standingInteractor.hoverEntered.AddListener(OnHoverEntered);
            standingInteractor.hoverExited.AddListener(OnHoverExited);
            standingInteractor.enabled = false;

            lyingInteractor = layingSpot.transform.Find("stitch_result_stitch_all").gameObject.AddComponent<XRSimpleInteractable>();
            lyingInteractor.selectEntered.AddListener(OnSelectEntered);
            lyingInteractor.hoverEntered.AddListener(OnHoverEntered);
            lyingInteractor.hoverExited.AddListener(OnHoverExited);
            lyingInteractor.enabled = false;

            standingSpot.transform.parent = robotGameObject.transform;
            layingSpot.transform.parent = robotGameObject.transform;

            robotGameObject.transform.localPosition = new Vector3(robotNode.Position[0], 0.55f, robotNode.Position[1]);
            robotGameObject.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            Vector3 euler = new Quaternion(robotNode.Rotation[0], robotNode.Rotation[1], robotNode.Rotation[2], robotNode.Rotation[3]).eulerAngles;
            robotGameObject.transform.localRotation = Quaternion.Euler(euler[0], -(euler[2] - 90f), euler[1]);
            if (robotNode.Robot_Status == RobotStatus.IDLE) sit(true);
            else sit(false);
        }

        private void sit(bool sit)
        {
            if (sit)
            {
                layingSpot.SetActive(true);
                standingSpot.SetActive(false);
                lyingInteractor.enabled = true;
            }
            else
            {
                layingSpot.SetActive(false);
                standingSpot.SetActive(true);
                standingInteractor.enabled = true;
            }
        }
        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            if (args.interactorObject is XRGazeInteractor) return;
            if (args.interactorObject is XRRayInteractor)
            {
                robotInformation.GetComponent<ObjectInformation>().set_object_information(setBody(), object_type.ROBOT);
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

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            robotPrompt.setRobot(this);
            robotPrompt.setBody("You have interacted with " + robotNode.Name + ", what do you want to do now? Here are some information about it: \n\n" + setBody());
            spotonApp.GetComponent<SpotonAPP>().show(show_object.ROBOT_PROMPT);
        }

        public string setBody()
        {
            string general = "This is: " + robotNode.Name + "\n ";
            string status = "Status: " + robotNode.stringifyStatus() + "\n";
            string battery = "Battery level: " + robotNode.Battery + "%\n";
            return general+status+battery;
        }

        void Update()
        {
            if (slow_down_counter % 3 == 0)
            {
                if (robotGameObject != null)
                {
                    spotonApp.GetComponent<SpotonAPP>().ReadServer(robotId, "?robot_id=0", message_type.ROBOT_REQUEST, response =>
                    {
                        if (response != null)
                        {
                            robotNode = RobotNode.CreateRobotFromJSON(response);
                        }
                    });

                    robotGameObject.transform.localPosition = new Vector3(robotNode.Position[0], 0.55f, robotNode.Position[1]);
                    robotGameObject.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                    Vector3 euler = new Quaternion(robotNode.Rotation[0], robotNode.Rotation[1], robotNode.Rotation[2], robotNode.Rotation[3]).eulerAngles;
                    robotGameObject.transform.localRotation = Quaternion.Euler(euler[0], -(euler[2] - 90f), euler[1]);
                    if (robotNode.Robot_Status == RobotStatus.IDLE) sit(true);
                    else sit(false);
                }
                slow_down_counter = 1;
            }
            else
            {
                slow_down_counter += 1;
            }
        }
    }
}

