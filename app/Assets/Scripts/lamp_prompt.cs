using UnityEngine;

using static SpotON.SpotonAPP;
using static Lamps.Lamp;

using Lamps;
using SpotON;
using MixedReality.Toolkit.UX;
using TMPro;

namespace LampPROMPT
{
    public class LampPrompt : MonoBehaviour
    {
        private GameObject spotonApp { get; set; }
        private GameObject lightPrompt { get; set; }
        private GameObject buttons { get; set; }
        private Lamp lamp { get; set; }
        private TextMeshProUGUI body { get; set; }

        public void Awake()
        {
            lightPrompt = this.gameObject;
            spotonApp = lightPrompt.transform.parent.gameObject;
            buttons = lightPrompt.transform.Find("body").Find("Horizontal").gameObject;
            body = lightPrompt.transform.Find("body").Find("Main Text").gameObject.GetComponent<TextMeshProUGUI>();


            lightPrompt.transform.Find("body").Find("Horizontal").Find("Positive").GetComponent<PressableButton>().OnClicked.AddListener(() => positive());
            lightPrompt.transform.Find("body").Find("Horizontal").Find("Negative").GetComponent<PressableButton>().OnClicked.AddListener(() => negative());
            lightPrompt.transform.Find("body").Find("Horizontal").Find("Neutral").GetComponent<PressableButton>().OnClicked.AddListener(() => neutral());
            lightPrompt.transform.Find("close").Find("CloseButton").Find("close_button").GetComponent<PressableButton>().OnClicked.AddListener(() => close());
        }

        public void setLamp(Lamp new_lamp)
        {
            lamp = new_lamp;
        }

        public void positive()
        {
            lamp.checkLampState();
        }

        public void negative()
        {
            
        }

        public void neutral()
        {
            
        }

        public void close()
        {
            lamp.closeDialog();
        }

        public void OnEnable()
        {
            if (lightPrompt != null)
            {
                lightPrompt.transform.Find("close").Find("CloseButton").gameObject.SetActive(true);

                if (lamp != null)
                {
                    body.text = lamp.setBody();
                }
            }
        }
    }
}

