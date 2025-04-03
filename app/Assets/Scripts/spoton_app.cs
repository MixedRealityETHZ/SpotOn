using UnityEngine;
using System.Threading.Tasks;
using MixedReality.Toolkit.SpatialManipulation;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

using static MainSCENE.MainScene;

using MainSCENE;
using System;
using MixedReality.Toolkit.Subsystems;
using StartMENU;
using Microsoft.MixedReality.GraphicsTools;
using GLTFast.Schema;
using TMPro;
using MixedReality.Toolkit;
using Unity.VisualScripting;
using LanguageINTERFACES;

namespace SpotON
{
    public class SpotonAPP : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.Material nightMaterial;
        [SerializeField]
        private UnityEngine.Material dayMaterial;

        public enum show_object
        {
            EXIT_APP = 0,
            MAIN_SCENE = 1,
            LOADING_SCREEN = 2,
            START_MENU = 3,
            TUTORIAL_PROMPT = 4,
            TEMPORARY_DIALOG = 5,
            OBJECT_INFORMATION = 6,
            DRAWER_PROMPT = 7,
            LAMP_PROMPT = 8,
            LIGHT_SWITCH_PROMPT = 9,
            DRAGGABLE_PROMPT = 10,
            TUTORIAL = 11,
            LANGUAGE_INTERFACE = 12,
            LANGUAGE_PROMPT = 13,
            HELPER_WINDOW = 14,
            ROBOT_PROMPT = 15
        }

        public enum message_type
        {
            JOB_REQUEST = 0,
            ROBOT_REQUEST = 1
        }

        public enum app_mode
        {
            NIGHT = 0,
            DAY = 1
        }

        private GameObject spotonAPP { get; set; }
        private GameObject mainScene { get; set; }
        private GameObject loadingScreen { get; set; }
        private GameObject startMenu { get; set; }
        private GameObject tutorialPrompt { get; set; }
        private GameObject temporaryDialog { get; set; }
        private GameObject objectInformation { get; set; }
        private GameObject drawerPrompt { get; set; }
        private GameObject lampPrompt { get; set; }
        private GameObject lightSwitchPrompt { get; set; }
        private GameObject draggablePrompt { get; set; }
        private GameObject tutorial { get; set; }
        private GameObject languagePrompt { get; set; }
        private GameObject languageInterface { get; set; }
        private GameObject helperWindow { get; set; }
        private GameObject robotPrompt { get; set; }

        private List<string> servers = new List<string> {
            "http://192.168.1.244:8080",
            "http://192.168.1.244:8081"
        };

        private app_mode mode {  get; set; }

        public GameObject getTemporaryDialog()
        {
            return temporaryDialog;
        }

        public void Start()
        {
            spotonAPP = this.gameObject;
            mode = app_mode.DAY;

            if (spotonAPP != null)
            {
                #if UNITY_EDITOR
                    spotonAPP.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                #endif

                mainScene = spotonAPP.transform.Find("main_scene").gameObject;
                loadingScreen = spotonAPP.transform.Find("loading_screen").gameObject;
                startMenu = spotonAPP.transform.Find("start_menu").gameObject;
                tutorialPrompt = spotonAPP.transform.Find("tutorial_prompt").gameObject;
                temporaryDialog = spotonAPP.transform.Find("temporary_dialog").gameObject;
                objectInformation = spotonAPP.transform.Find("object_information").gameObject;
                drawerPrompt = spotonAPP.transform.Find("drawer_prompt").gameObject;
                lightSwitchPrompt = spotonAPP.transform.Find("light_switch_prompt").gameObject;
                lampPrompt = spotonAPP.transform.Find("lamp_prompt").gameObject;
                draggablePrompt = spotonAPP.transform.Find("drag_and_drop_prompt").gameObject;
                tutorial = spotonAPP.transform.Find("tutorial").gameObject;
                languageInterface = spotonAPP.transform.Find("language_interface").gameObject;
                languagePrompt = spotonAPP.transform.Find("language_prompt").gameObject;
                helperWindow = spotonAPP.transform.Find("helper_window").gameObject;
                robotPrompt = spotonAPP.transform.Find("robot_prompt").gameObject;

                mainScene.SetActive(true);
                startMenu.SetActive(true);
                tutorialPrompt.SetActive(true);
                temporaryDialog.SetActive(true);
                objectInformation.SetActive(true);
                drawerPrompt.SetActive(true);
                lightSwitchPrompt.SetActive(true);
                lampPrompt.SetActive(true);
                draggablePrompt.SetActive(true);
                languagePrompt.SetActive(true);
                helperWindow.SetActive(true);
                robotPrompt.SetActive(true);
                languageInterface.SetActive(true);
            }

            hideAll();
            runLoading();

            int currentHour = System.DateTime.Now.Hour;
            if(currentHour <= 5 || currentHour >= 18)
            {
                toggleMode();
            }
        }

        public void toggleModeParam(app_mode new_mode = app_mode.DAY)
        {
            mode = new_mode;
            toggleMode();
        }

        public void toggleMode()
        {
            switch (mode)
            {
                case app_mode.DAY:
                    startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop").Find("MenuContent-Canvas").Find("Backplate-HorizontalLayout").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop").Find("MenuContent-Canvas").Find("Backplate-HorizontalLayout").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    tutorialPrompt.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    tutorialPrompt.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    temporaryDialog.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    temporaryDialog.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    objectInformation.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    objectInformation.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    helperWindow.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    helperWindow.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    languagePrompt.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    languagePrompt.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    draggablePrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    draggablePrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    drawerPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    drawerPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;
                    drawerPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    drawerPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    lightSwitchPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    lightSwitchPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;
                    lightSwitchPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    lightSwitchPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    lampPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    lampPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;
                    lampPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    lampPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    robotPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    robotPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;
                    robotPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().material = nightMaterial;
                    robotPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop/MenuContent-Canvas/Backplate-HorizontalLayout/info_panel-vertical/robot_information/night_mode/Frontplate/AnimatedContent/Text").GetComponent<TextMeshProUGUI>().text = "Day Mode";
                    mode = app_mode.NIGHT;
                    break;

                case app_mode.NIGHT:
                    startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop").Find("MenuContent-Canvas").Find("Backplate-HorizontalLayout").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop").Find("MenuContent-Canvas").Find("Backplate-HorizontalLayout").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    tutorialPrompt.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    tutorialPrompt.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    temporaryDialog.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    temporaryDialog.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    objectInformation.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    objectInformation.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    helperWindow.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    helperWindow.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    languagePrompt.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    languagePrompt.transform.Find("Canvas/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    draggablePrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    draggablePrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    drawerPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    drawerPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;
                    drawerPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    drawerPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    lightSwitchPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    lightSwitchPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;
                    lightSwitchPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    lightSwitchPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    lampPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    lampPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;
                    lampPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    lampPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    robotPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    robotPrompt.transform.Find("body/Plate").GetComponent<CanvasElementRoundedRect>().color = Color.black;
                    robotPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().material = dayMaterial;
                    robotPrompt.transform.Find("close/CloseButton").GetComponent<CanvasElementRoundedRect>().color = Color.black;

                    startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop/MenuContent-Canvas/Backplate-HorizontalLayout/info_panel-vertical/robot_information/night_mode/Frontplate/AnimatedContent/Text").GetComponent<TextMeshProUGUI>().text = "Night Mode";
                    mode = app_mode.DAY;
                    break;
            }
        }

        public string stringifyMessageType(message_type type)
        {
            switch (type)
            {
                case message_type.JOB_REQUEST:
                    return "/command/";
                case message_type.ROBOT_REQUEST:
                    return "/robot_info";
                default:
                    return "/command/";

            }
        }

        public IEnumerator notifyServer(int id, string message, message_type type)
        {
            string ip = servers[id];
            string mess_type = stringifyMessageType(type);
            UnityWebRequest uwr = UnityWebRequest.PostWwwForm(ip + mess_type + message, string.Empty);
            uwr.SetRequestHeader("Content-Type", "application/json");
            yield return uwr.SendWebRequest();
        }

        public void notifyAllServers(string message, message_type type)
        {
            for (int id = 0; id < servers.Count; id++)
            {
                StartCoroutine(notifyServer(id, message, type));
            }
        }

        public void ReadServer(int id, string message, message_type type, Action<string> onResponse)
        {
            StartCoroutine(ReadServerCoroutine(id, message, type, onResponse));
        }

        private IEnumerator ReadServerCoroutine(int id, string message, message_type type, Action<string> onResponse)
        {
            string ip = servers[id];
            string mess_type = stringifyMessageType(type);
            string url = ip + mess_type + message;

            UnityWebRequest uwr = UnityWebRequest.Get(url);
            uwr.SetRequestHeader("Content-Type", "application/json");

            yield return uwr.SendWebRequest();

            if (!(uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError))
            {
                onResponse?.Invoke(uwr.downloadHandler.text);
            }
        }

        public void enableProcessesOnStart(bool enable)
        {
            startMenu.transform.Find("HandMenuLarge_WorldLockOnHandDrop").GetComponent<HandConstraint>().enabled = enable;
            if (enable) languageInterface.GetComponent<LanguageInterface>().enableVoice();
            else languageInterface.GetComponent<LanguageInterface>().disableVoice();
        }

        async void runLoading()
        {
            await Task.WhenAll(loadingScreenProcess(), loadSceneProcess());
        }

        Task loadingScreenProcess()
        {
            show(show_object.TUTORIAL_PROMPT);
            return Task.CompletedTask;
        }

        Task loadSceneProcess()
        {
            mainScene.GetComponent<MainScene>().Initialize();
            return Task.CompletedTask;
        }

        public void hideAll()
        {
            mainScene.SetActive(false);
            loadingScreen.SetActive(false);
            startMenu.SetActive(false);
            tutorialPrompt.SetActive(false);
            temporaryDialog.SetActive(false);
            objectInformation.SetActive(false);
            drawerPrompt.SetActive(false);
            lightSwitchPrompt.SetActive(false);
            lampPrompt.SetActive(false);
            draggablePrompt.SetActive(false);
            tutorial.SetActive(false);
            languagePrompt.SetActive(false);
            helperWindow.SetActive(false);
            robotPrompt.SetActive(false);
        }

        public void hideInstead(show_object which_object)
        {
            if (which_object != show_object.MAIN_SCENE) mainScene.SetActive(false);
            if (which_object != show_object.LOADING_SCREEN) loadingScreen.SetActive(false);
            if (which_object != show_object.START_MENU) startMenu.SetActive(false);
            if (which_object != show_object.TUTORIAL_PROMPT) tutorialPrompt.SetActive(false);
            if (which_object != show_object.TEMPORARY_DIALOG) temporaryDialog.SetActive(false);
            if (which_object != show_object.OBJECT_INFORMATION) objectInformation.SetActive(false);
            if (which_object != show_object.DRAWER_PROMPT) drawerPrompt.SetActive(false);
            if (which_object != show_object.LIGHT_SWITCH_PROMPT) lightSwitchPrompt.SetActive(false);
            if (which_object != show_object.LAMP_PROMPT) lampPrompt.SetActive(false);
            if (which_object != show_object.DRAGGABLE_PROMPT) draggablePrompt.SetActive(false);
            if (which_object != show_object.TUTORIAL) tutorial.SetActive(false);
            if (which_object != show_object.LANGUAGE_PROMPT) languagePrompt.SetActive(false);
            if (which_object != show_object.HELPER_WINDOW) helperWindow.SetActive(false);
            if (which_object != show_object.ROBOT_PROMPT) robotPrompt.SetActive(false);
        }

        public void show_menu()
        {
            show(show_object.START_MENU);
        }

        public void show(show_object which_object, bool hide_all = true)
        {
            if (hide_all)
            {
                hideInstead(which_object);
            }

            switch (which_object) 
            {
                case show_object.EXIT_APP:
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                    #else
                        Application.Quit();
                    #endif
                    break;

                case show_object.MAIN_SCENE:
                    mainScene.SetActive(true);
                    break;

                case show_object.LOADING_SCREEN:
                    loadingScreen.SetActive(true);
                    break;

                case show_object.START_MENU:
                    startMenu.SetActive(true);
                    break;

                case show_object.TUTORIAL_PROMPT:
                    tutorialPrompt.SetActive(true);
                    break;

                case show_object.TEMPORARY_DIALOG:
                    temporaryDialog.SetActive(true);
                    break;

                case show_object.OBJECT_INFORMATION:
                    objectInformation.SetActive(true);
                    break;

                case show_object.LAMP_PROMPT:
                    lampPrompt.SetActive(true);
                    break;

                case show_object.LIGHT_SWITCH_PROMPT:
                    lightSwitchPrompt.SetActive(true);
                    break;

                case show_object.DRAWER_PROMPT:
                    drawerPrompt.SetActive(true);
                    break;

                case show_object.DRAGGABLE_PROMPT:
                    draggablePrompt.SetActive(true);
                    break;

                case show_object.LANGUAGE_PROMPT:
                    languagePrompt.SetActive(true);
                    break;

                case show_object.HELPER_WINDOW:
                    helperWindow.SetActive(true);
                    break;

                case show_object.TUTORIAL:
                    tutorial.SetActive(true);
                    break;

                case show_object.ROBOT_PROMPT:
                    robotPrompt.SetActive(true);
                    break;
            }
        }

        public void hide(show_object which_object)
        {
            switch (which_object)
            {
                case show_object.EXIT_APP:
                    Application.Quit();
                    break;

                case show_object.MAIN_SCENE:
                    mainScene.SetActive(false);
                    break;

                case show_object.LOADING_SCREEN:
                    loadingScreen.SetActive(false);
                    break;

                case show_object.START_MENU:
                    startMenu.SetActive(false);
                    break;

                case show_object.TUTORIAL_PROMPT:
                    tutorialPrompt.SetActive(false);
                    break;

                case show_object.TEMPORARY_DIALOG:
                    temporaryDialog.SetActive(false);
                    break;

                case show_object.OBJECT_INFORMATION:
                    objectInformation.SetActive(false);
                    break;

                case show_object.LAMP_PROMPT:
                    lampPrompt.SetActive(false);
                    break;

                case show_object.LIGHT_SWITCH_PROMPT:
                    lightSwitchPrompt.SetActive(false);
                    break;

                case show_object.DRAWER_PROMPT:
                    drawerPrompt.SetActive(false);
                    break;

                case show_object.DRAGGABLE_PROMPT:
                    lightSwitchPrompt.SetActive(false);
                    break;

                case show_object.TUTORIAL:
                    tutorial.SetActive(false);
                    break;

                case show_object.LANGUAGE_PROMPT:
                    languagePrompt.SetActive(false);
                    break;

                case show_object.HELPER_WINDOW:
                    helperWindow.SetActive(false);
                    break;

                case show_object.ROBOT_PROMPT:
                    robotPrompt.SetActive(false);
                    break;

            }
        }

        public void Update()
        {

        }
    }

}
