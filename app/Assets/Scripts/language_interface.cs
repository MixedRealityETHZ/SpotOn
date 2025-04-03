using UnityEngine;

using static SpotON.SpotonAPP;

using SpotON;
using MRTKExtensions.SpeechRecognition;
using MixedReality.Toolkit.Subsystems;
using MixedReality.Toolkit;
using MainSCENE;
using TemporaryDIALOG;
using LanguagePROMPTS;
using Unity.VisualScripting;

namespace LanguageINTERFACES
{
    public class LanguageInterface : MonoBehaviour
    {
        private GameObject spotonApp { get; set; }
        private GameObject languagePrompt { get; set; }
        private GameObject languageInterface { get; set; }
        private GameObject speechRecognitionSystem {  get; set; }
        private KeywordRecognitionSubsystem keywordRecognitionSubsystem { get; set; }

        public void Awake()
        {
            languageInterface = this.gameObject;
            spotonApp = languageInterface.transform.parent.gameObject;
            languagePrompt = spotonApp.transform.Find("language_prompt").gameObject;
            speechRecognitionSystem = GameObject.Find("MRTK Speech");
            keywordRecognitionSubsystem = XRSubsystemHelpers.GetFirstRunningSubsystem<KeywordRecognitionSubsystem>();

            if (keywordRecognitionSubsystem != null)
            {
                keywordRecognitionSubsystem.CreateOrGetEventForKeyword("open menu").AddListener(() => openMenu());
                keywordRecognitionSubsystem.CreateOrGetEventForKeyword("help me").AddListener(() => helpMe());
                keywordRecognitionSubsystem.CreateOrGetEventForKeyword("restart scene").AddListener(() => restartSceneFirst());
                keywordRecognitionSubsystem.CreateOrGetEventForKeyword("close").AddListener(() => closeMenu());
                keywordRecognitionSubsystem.CreateOrGetEventForKeyword("open tutorial").AddListener(() => openTutorial());
                keywordRecognitionSubsystem.CreateOrGetEventForKeyword("toggle scene").AddListener(() => toggleScene());
                keywordRecognitionSubsystem.CreateOrGetEventForKeyword("night mode").AddListener(() => toggleNightMode());
                keywordRecognitionSubsystem.CreateOrGetEventForKeyword("day mode").AddListener(() => toggleDayMode());
                keywordRecognitionSubsystem.CreateOrGetEventForKeyword("show scene").AddListener(() => showScene());
            }

            disableVoice();
        }

        public void enableVoice()
        {
            if (keywordRecognitionSubsystem != null)
            {
                keywordRecognitionSubsystem.Start();
            }
        }

        public void disableVoice()
        {
            if (keywordRecognitionSubsystem != null)
            {
                keywordRecognitionSubsystem.Stop();
            }
        }

        public void showScene()
        {
            spotonApp.GetComponent<SpotonAPP>().show(show_object.MAIN_SCENE);
        }

        public void toggleDayMode()
        {
            spotonApp.GetComponent<SpotonAPP>().toggleModeParam(app_mode.NIGHT);
        }

        public void toggleNightMode()
        {
            spotonApp.GetComponent<SpotonAPP>().toggleModeParam(app_mode.DAY);
        }

        public void toggleScene()
        {
            spotonApp.transform.Find("main_scene").gameObject.GetComponent<MainScene>().toggle_colors();
            spotonApp.GetComponent<SpotonAPP>().show(show_object.MAIN_SCENE);
        }

        public void openTutorial()
        {
            spotonApp.GetComponent<SpotonAPP>().show(show_object.TUTORIAL);
        }

        public void helpMe()
        {
            spotonApp.GetComponent<SpotonAPP>().show(show_object.HELPER_WINDOW);
        }

        public void restartSceneFirst()
        {
            disableVoice();
            languagePrompt.GetComponent<LanguagePrompt>().setButtonText("Restart the scene!", "No, go back!");
            languagePrompt.GetComponent<LanguagePrompt>().setListener(restartSceneSecond);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.LANGUAGE_PROMPT);
        }

        public void restartSceneSecond()
        {
            enableVoice();
            spotonApp.transform.Find("main_scene").gameObject.GetComponent<MainScene>().restartScene();
            spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("You have restarted the scene!", "Wait until scene is reinitialized to default.", show_object.MAIN_SCENE, 5f);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);
        }

        public void closeMenu()
        {
            spotonApp.GetComponent<SpotonAPP>().show(show_object.MAIN_SCENE);
        }

        public void openMenu()
        {
            spotonApp.GetComponent<SpotonAPP>().show(show_object.START_MENU);
        }
    }
}

