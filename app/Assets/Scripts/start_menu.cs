using UnityEngine;

using static SpotON.SpotonAPP;

using MainSCENE;
using SpotON;
using MixedReality.Toolkit.UX;
using TemporaryDIALOG;
using MixedReality.Toolkit;


namespace StartMENU
{
    public class StartMenu : MonoBehaviour
    {
        private GameObject spotonApp;
        private GameObject startMenu;
        private GameObject buttons;

        public void Start()
        {
            startMenu = this.gameObject;
            spotonApp = startMenu.transform.parent.gameObject;
            buttons = startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop").Find("MenuContent-Canvas").Find("Backplate-HorizontalLayout").Find("ButtonsWide-VerticalLayout").gameObject;

            buttons.transform.Find("toggle_scene_button").GetComponent<PressableButton>().OnClicked.AddListener(() => toggleScenes());
            buttons.transform.Find("tutorial_button").GetComponent<PressableButton>().OnClicked.AddListener(() => showTutorial());
            buttons.transform.Find("restart_scene").GetComponent<PressableButton>().OnClicked.AddListener(() => restartScene());
            buttons.transform.Find("exit_app").GetComponent<PressableButton>().OnClicked.AddListener(() => exitApp());
            buttons.transform.Find("scene").GetComponent<PressableButton>().OnClicked.AddListener(() => closeMenu());

            startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop").Find("MenuContent-Canvas").Find("Backplate-HorizontalLayout").Find("info_panel-vertical").Find("robot_information").Find("help_me_button").GetComponent<PressableButton>().OnClicked.AddListener(() => getHelp());
            startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop").Find("MenuContent-Canvas").Find("Backplate-HorizontalLayout").Find("info_panel-vertical").Find("robot_information").Find("night_mode").GetComponent<PressableButton>().OnClicked.AddListener(() => nightMode());

            //startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop").Find("MenuContent-Canvas").Find("CloseButton").Find("close_button").GetComponent<PressableButton>().OnClicked.AddListener(() => closeMenu());
        }

        public void nightMode()
        {
            spotonApp.GetComponent<SpotonAPP>().toggleMode();
        }

        public void OnEnable()
        {
            if (startMenu != null)
            {
                startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop").gameObject.SetActive(true);
                startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop").Find("MenuContent-Canvas").gameObject.SetActive(true);
                startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop").Find("MenuContent-Canvas").Find("Backplate-HorizontalLayout").gameObject.SetActive(true);
                startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop").Find("MenuContent-Canvas").Find("Backplate-HorizontalLayout").Find("ButtonsWide-VerticalLayout").gameObject.SetActive(true);
                //startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop").Find("MenuContent-Canvas").Find("CloseButton").gameObject.SetActive(true);
            }
        }

        public void getHelp()
        {
            spotonApp.GetComponent<SpotonAPP>().show(show_object.HELPER_WINDOW);
        }

        public void toggleScenes()
        {
            spotonApp.transform.Find("main_scene").gameObject.GetComponent<MainScene>().toggle_colors();
            spotonApp.GetComponent<SpotonAPP>().show(show_object.MAIN_SCENE);
        }

        public void restartScene()
        {
            spotonApp.transform.Find("main_scene").gameObject.GetComponent<MainScene>().restartScene();
            spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("You have restarted the scene!", "Wait until scene is reinitialized to default.", show_object.MAIN_SCENE, 5f);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);
        }

        public void exitApp()
        {
            spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Goodbye!", "Thank you for using our app!", show_object.EXIT_APP, 5f);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);
        }

        public void showTutorial()
        {
            spotonApp.GetComponent<SpotonAPP>().show(show_object.TUTORIAL);
        }

        public void closeMenu()
        {
            spotonApp.GetComponent<SpotonAPP>().show(show_object.MAIN_SCENE);
        }

        public void Update()
        {
            //startMenu.transform.rotation = Quaternion.Euler(41.689f, -135.92f, 0f);
        }
    }
}

