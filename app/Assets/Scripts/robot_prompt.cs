using UnityEngine;

using static SpotON.SpotonAPP;
using static Robots.Robot;

using Robots;
using SpotON;
using MixedReality.Toolkit.UX;
using TemporaryDIALOG;
using TMPro;
using DrawerPROMPT;
using Drawers;
using GLTFast.Schema;

namespace RobotPROMPT
{
    public class RobotPrompt : MonoBehaviour
    {
        private GameObject spotonApp { get; set; }
        private GameObject robotPrompt { get; set; }
        private GameObject buttons { get; set; }
        private Robot robot {  get; set; }
        private TextMeshProUGUI body {  get; set; }

        public void Awake()
        {
            robotPrompt = this.gameObject;
            spotonApp = robotPrompt.transform.parent.gameObject;
            buttons = robotPrompt.transform.Find("body").Find("Horizontal").gameObject;

            buttons.transform.Find("Positive").GetComponent<PressableButton>().OnClicked.AddListener(() => positive());
            body = robotPrompt.transform.Find("body").Find("Main Text").gameObject.GetComponent<TextMeshProUGUI>();
            robotPrompt.transform.Find("close").Find("CloseButton").Find("close_button").GetComponent<PressableButton>().OnClicked.AddListener(() => close());
        }

        public void setRobot(Robot new_robot)
        {
            robot = new_robot;
        }

        public void setHeader(string header)
        {
            robotPrompt.transform.Find("body").Find("Header").GetComponent<TextMeshProUGUI>().text = header;
        }

        public void setBody(string body)
        {
            robotPrompt.transform.Find("body").Find("Main Text").GetComponent<TextMeshProUGUI>().text = body;
        }

        public void close()
        {
            spotonApp.GetComponent<SpotonAPP>().show(show_object.MAIN_SCENE);
        }

        public void positive()
        {
            spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Watch out!", "You have initiated return to start. Be carefull, the robots might start moving now.", show_object.MAIN_SCENE, 3f);

            spotonApp.GetComponent<SpotonAPP>().notifyServer(robot.robotId, "?job_type=back_to_start&robot_id=0", message_type.JOB_REQUEST);
            spotonApp.GetComponent<SpotonAPP>().notifyServer(robot.robotId, "?job_type=localize&robot_id=0", message_type.JOB_REQUEST);
            print("BACK TO START INITIATED: " + robot.name);

            spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);
        }

        public void OnEnable()
        {
            if (robotPrompt != null)
            {
                robotPrompt.transform.Find("close").Find("CloseButton").gameObject.SetActive(true);
            }
        }

    }
}

