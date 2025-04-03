using UnityEngine;

using static SpotON.SpotonAPP;
using static Draggables.Draggable;

using Drawers;
using SpotON;
using MixedReality.Toolkit.UX;
using TemporaryDIALOG;
using MixedReality.Toolkit.SpatialManipulation;

namespace TutorialPROMPT
{
    public class TutorialPrompt : MonoBehaviour
    {
        private GameObject spotonApp { get; set; }
        private GameObject tutorialPrompt { get; set; }
        private GameObject buttons { get; set; }

        public void Awake()
        {
            tutorialPrompt = this.gameObject;
            spotonApp = tutorialPrompt.transform.parent.gameObject;
            buttons = tutorialPrompt.transform.Find("Canvas").Find("Horizontal").gameObject;
            tutorialPrompt.transform.Find("Canvas").Find("Horizontal").Find("Positive").GetComponent<PressableButton>().OnClicked.AddListener(() => positive());
            tutorialPrompt.transform.Find("Canvas").Find("Horizontal").Find("Negative").GetComponent<PressableButton>().OnClicked.AddListener(() => negative());
        }

        public void positive()
        {
            spotonApp.GetComponent<SpotonAPP>().show(show_object.TUTORIAL);
        }

        public void negative()
        {
            spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Great!", "Have fun exploring the app. Main menu will appear shortly!", show_object.START_MENU, 5f);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);
            spotonApp.GetComponent<SpotonAPP>().enableProcessesOnStart(true);
        }
    }
}

