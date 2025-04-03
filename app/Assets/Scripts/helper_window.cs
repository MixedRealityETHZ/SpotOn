using UnityEngine;

using static SpotON.SpotonAPP;

using SpotON;
using MixedReality.Toolkit.UX;
using TemporaryDIALOG;
using JetBrains.Annotations;
using TMPro;

namespace HelperWINDOWS
{
    public enum helper_window
    {
        GENERAL_INTERACTION = 0,
        START_MENU = 1,
        DRAWER = 2,
        LAMP = 3,
        LIGHT_SWITCH = 4,
        DRAGGABLE = 5,
        ROBOT = 6,
        LANGUAGE = 7,
    }

    public class HelperWindow : MonoBehaviour
    {
        private GameObject spotonApp { get; set; }
        private GameObject helperWindow { get; set; }
        private GameObject buttons { get; set; }
        private helper_window current_window { get; set; }
        private bool first = true;

        public void Start()
        {
            helperWindow = this.gameObject;
            spotonApp = helperWindow.transform.parent.gameObject;
            buttons = helperWindow.transform.Find("Canvas").Find("Horizontal").gameObject;
            helperWindow.transform.Find("Canvas").Find("Horizontal").Find("Positive").GetComponent<PressableButton>().OnClicked.AddListener(() => positive());
            helperWindow.transform.Find("Canvas").Find("Horizontal").Find("Negative").GetComponent<PressableButton>().OnClicked.AddListener(() => negative());
            
            current_window = helper_window.GENERAL_INTERACTION;
            positive();
        }

        public void positive()
        {
            if (first)
            {
                current_window = helper_window.GENERAL_INTERACTION;
                first = false;
            }

            string message = "";
            switch (current_window)
            {
                case helper_window.GENERAL_INTERACTION:
                    message = "GENERAL INTERACTION: \n\n Our app consists of start menu and a scene with robots. You can interact with object by either hovering or clicking. Language interface is soon to come too. There are five different objects you can interact with. Hovering over them with a pointer you will see most important information regarding that object, using clicking, you will get a prompt and be able choose what you want to do with this object. We advise to use spyder motion to click on the objects. This is done by pointing a laser pointer and pinching your fingers.\n";
                    current_window = helper_window.START_MENU;
                    break;
                case helper_window.START_MENU:
                    message = "START MENU:\r\n\r\n1. Tutorial - Open the tutorial if you perhaps skipped it.\r\n2. Toogle the scene - Toggles between scene graph colors and real life colors of the objects.\r\n3. Reinitialize the scene - This restarts all object to their default states and repositions the scene to the start postion.\r\n4. Exit the app - Closes the application.\r\n 5. Night/Day mode - Switches into Night/Day mode. \n6. Scene - Shows main scene";
                    current_window = helper_window.DRAWER;
                    break;
                case helper_window.DRAWER:
                    message = "DRAWER OBJECT: \n\n1. Search and Drop - Robot attempts to open a drawer and seacrhes for objects inside. If object is in the drawer. It will fetch it into a basket on top of second robot. \n2. Open - Opens a drawer if it is not already open \n3. Close - Closes a drawer if it is not already closed\n";
                    current_window = helper_window.LAMP;
                    break;
                case helper_window.LAMP:
                    message = "LAMP OBJECT: \n\n1. Check Lamp State - Robot turns to given light \n and tries to check whether it is on.\n";
                    current_window = helper_window.LIGHT_SWITCH;
                    break;
                case helper_window.LIGHT_SWITCH:
                    message = "LIGHT SWITCH OBJECT: \n\n1. Operate Light Switch - Directly operates given light switch. \n2. Check Lamp Connections - This task uses two robots. One firstly checks the state of the lights in the scene, while other operates a light switch. Then another light check is performed by the first robot which then gives us which lights are connected to tthis light switch.\n";
                    current_window = helper_window.DRAGGABLE;
                    break;
                case helper_window.DRAGGABLE:
                    message = "DRAGGABLE OBJECT: \n\n 1. Fetch the Object - Robot with an arm grabs an object and puts it in a basket on top of second robot. \n";
                    current_window = helper_window.ROBOT;
                    break;
                case helper_window.ROBOT:
                    message = "ROBOT OBJECT: \n\n 1. Return to Start - Returns robot to the starting position and localizes. \n";
                    current_window = helper_window.LANGUAGE;
                    break;
                case helper_window.LANGUAGE:
                    message = "LANGUAGE INTERFACE: \n\n Our App also has language interface for some functionalities. After first start menu appereance you will be able to use voice commands to do different functions. Some of these will require your confirmation, but most of the will not. Those that need your attention we will label with (prompt): \n1. Open menu - Opens start menu \n2. Close - Closes any window that is currently on and goes back to scene \n3.Help me - Opens this helper window \n4. Restart scene - Restarts the scene (prompt) \n5. Open tutorial - Opens tutorial window \n6. Toggle scene - Toggles scene color \n7. Show scene - Shows main scene \n8. Night mode - Turns application into night mode. All windows stay open \n9. Day mode - Turns application into day mode. All windows stay open";
                    current_window = helper_window.GENERAL_INTERACTION;
                    break;
            }

            helperWindow.transform.Find("Canvas").Find("window_info").GetComponent<TextMeshProUGUI>().text = message;

        }

        public void negative()
        {
            current_window = helper_window.GENERAL_INTERACTION;
            first = true;
            spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Have fun!", "You are now exiting the helper window and going to our main menu.", show_object.START_MENU, 3f);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);
        }

        public void onEnabled()
        {
            current_window = helper_window.GENERAL_INTERACTION;
            first = true;
        }
    }
}
