using UnityEngine;

using static SpotON.SpotonAPP;

using SpotON;
using MixedReality.Toolkit.UX;
using TemporaryDIALOG;
using TMPro;
using LanguageINTERFACES;

namespace LanguagePROMPTS
{
    public class LanguagePrompt : MonoBehaviour
    {
        private GameObject spotonApp { get; set; }
        private GameObject languagePrompt { get; set; }
        private GameObject buttons { get; set; }
        private GameObject languageInterface { get; set; }

        public void Awake()
        {
            languagePrompt = this.gameObject;
            spotonApp = languagePrompt.transform.parent.gameObject;
            languageInterface = spotonApp.transform.Find("language_interface").gameObject;
            buttons = languagePrompt.transform.Find("Canvas").Find("Horizontal").gameObject;

            languagePrompt.transform.Find("Canvas").Find("Horizontal").Find("Negative").GetComponent<PressableButton>().OnClicked.AddListener(() => negative());
        }

        public void setListener(UnityEngine.Events.UnityAction function)
        {
            languagePrompt.transform.Find("Canvas").Find("Horizontal").Find("Positive").GetComponent<PressableButton>().OnClicked.RemoveAllListeners();
            languagePrompt.transform.Find("Canvas").Find("Horizontal").Find("Positive").GetComponent<PressableButton>().OnClicked.AddListener(function);
        }

        public void removeListeners()
        {
            languagePrompt.transform.Find("Canvas").Find("Horizontal").Find("Positive").GetComponent<PressableButton>().OnClicked.RemoveAllListeners();
        }

        public void setButtonText(string positive, string negative)
        {
            languagePrompt.transform.Find("Canvas").Find("Horizontal").Find("Positive").Find("Frontplate/AnimatedContent/Text").GetComponent<TextMeshProUGUI>().text = positive;
            languagePrompt.transform.Find("Canvas").Find("Horizontal").Find("Negative").Find("Frontplate/AnimatedContent/Text").GetComponent<TextMeshProUGUI>().text = negative;
        }

        public void setHeader(string header)
        {
            languagePrompt.transform.Find("Canvas").Find("Header").GetComponent<TextMeshProUGUI>().text = header;
        }

        public void setBody(string body)
        {
            languagePrompt.transform.Find("Canvas").Find("Main Text").GetComponent<TextMeshProUGUI>().text = body;
        }

        public void negative()
        {
            removeListeners();
            languageInterface.GetComponent<LanguageInterface>().enableVoice();
            spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Alright!", "Your request in cancelled, you can use our language interface anytime later. We will get you back to the scene!", show_object.MAIN_SCENE, 5f);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);
        }
    }
}

