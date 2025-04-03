using UnityEngine;

using static SpotON.SpotonAPP;
using static LightSWITCHES.LightSwitch;

using LightSWITCHES;
using SpotON;
using MixedReality.Toolkit.UX;
using TMPro;
using TemporaryDIALOG;
using System.Collections.Generic;

namespace LightSwitchPROMPT
{
    public class LightSwitchPrompt : MonoBehaviour
    {
        private GameObject spotonApp { get; set; }
        private GameObject lightSwitchPrompt { get; set; }
        private GameObject buttons { get; set; }
        private LightSwitch lightSwitch { get; set; }
        private TextMeshProUGUI body { get; set; }
   
        public void Awake()
        {
            lightSwitchPrompt = this.gameObject;
            spotonApp = lightSwitchPrompt.transform.parent.gameObject;
            buttons = lightSwitchPrompt.transform.Find("body").Find("Horizontal").gameObject;
            body = lightSwitchPrompt.transform.Find("body").Find("Main Text").gameObject.GetComponent<TextMeshProUGUI>();


            lightSwitchPrompt.transform.Find("body").Find("Horizontal").Find("Positive").GetComponent<PressableButton>().OnClicked.AddListener(() => positive());
            lightSwitchPrompt.transform.Find("body").Find("Horizontal").Find("Negative").GetComponent<PressableButton>().OnClicked.AddListener(() => negative());
            lightSwitchPrompt.transform.Find("body").Find("Horizontal").Find("Neutral").GetComponent<PressableButton>().OnClicked.AddListener(() => neutral());
            lightSwitchPrompt.transform.Find("close").Find("CloseButton").Find("close_button").GetComponent<PressableButton>().OnClicked.AddListener(() => close());
        }

        public void setLightSwitch(LightSwitch new_light_switch)
        {
            lightSwitch = new_light_switch;
        }

        public void positive()
        {
            lightSwitch.operateLightSwitch();
        }

        public void negative()
        {
            lightSwitch.checkConnection();
        }
        public void neutral()
        {
            
        }

        public void close()
        {
            lightSwitch.closeDialog();
        }

        public void OnEnable()
        {
            if (lightSwitchPrompt != null)
            {
                lightSwitchPrompt.transform.Find("close").Find("CloseButton").gameObject.SetActive(true);
                if (lightSwitch != null)
                {
                    body.text = lightSwitch.setBody();
                }
            }
        }
    }
}



