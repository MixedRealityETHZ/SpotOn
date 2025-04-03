using UnityEngine;

using static SpotON.SpotonAPP;
using static Drawers.Drawer;

using Drawers;
using SpotON;
using MixedReality.Toolkit.UX;
using TMPro;
using TemporaryDIALOG;

namespace DrawerPROMPT
{
    public class DrawerPrompt : MonoBehaviour
    {
        private GameObject spotonApp { get; set; }
        private GameObject drawerPrompt {  get; set; }
        private GameObject buttons {  get; set; }
        private Drawer drawer { get; set; }
        private TextMeshProUGUI body { get; set; }

        public void Awake()
        {
            drawerPrompt = this.gameObject;
            spotonApp = drawerPrompt.transform.parent.gameObject;
            buttons = drawerPrompt.transform.Find("body").Find("Horizontal").gameObject;
            body = drawerPrompt.transform.Find("body").Find("Main Text").gameObject.GetComponent<TextMeshProUGUI>();


            drawerPrompt.transform.Find("body").Find("Horizontal").Find("Positive").GetComponent<PressableButton>().OnClicked.AddListener(() => positive());
            drawerPrompt.transform.Find("body").Find("Horizontal").Find("Negative").GetComponent<PressableButton>().OnClicked.AddListener(() => negative());
            drawerPrompt.transform.Find("body").Find("Horizontal").Find("Neutral").GetComponent<PressableButton>().OnClicked.AddListener(() => neutral());
            drawerPrompt.transform.Find("close").Find("CloseButton").Find("close_button").GetComponent<PressableButton>().OnClicked.AddListener(() => close());
        }

        public void setDrawer(Drawer new_drawer)
        {
            drawer = new_drawer;
        }

        public void positive()
        {
            drawer.searchAndDrop();
        }

        public void negative()
        {
            drawer.open();
        }

        public void neutral()
        {
            drawer.close();
        }

        public void close()
        {
            drawer.closeDialog();
        }

        public void OnEnable()
        {
            if (drawerPrompt != null)
            {
                drawerPrompt.transform.Find("close").Find("CloseButton").gameObject.SetActive(true);
                if (drawer != null)
                {
                    body.text = drawer.setBody();
                }
            }
        }
    }
}

