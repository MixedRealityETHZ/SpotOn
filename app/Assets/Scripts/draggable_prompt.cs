using UnityEngine;

using static SpotON.SpotonAPP;
using static Draggables.Draggable;

using Drawers;
using SpotON;
using MixedReality.Toolkit.UX;
using TMPro;
using Draggables;

namespace DraggablePROMPT
{
    public class DraggablePrompt : MonoBehaviour
    {
        private GameObject spotonApp { get; set; }
        private GameObject draggablePrompt { get; set; }
        private GameObject buttons { get; set; }
        private Draggable draggable { get; set; }
        private TextMeshProUGUI body { get; set; }

        public void Awake()
        {
            draggablePrompt = this.gameObject;
            spotonApp = draggablePrompt.transform.parent.gameObject;
            buttons = draggablePrompt.transform.Find("body").Find("Horizontal").gameObject;
            body = draggablePrompt.transform.Find("body").Find("Main Text").gameObject.GetComponent<TextMeshProUGUI>();


            draggablePrompt.transform.Find("body").Find("Horizontal").Find("Positive").GetComponent<PressableButton>().OnClicked.AddListener(() => positive());
            draggablePrompt.transform.Find("body").Find("Horizontal").Find("Negative").GetComponent<PressableButton>().OnClicked.AddListener(() => negative());
            draggablePrompt.transform.Find("body").Find("Horizontal").Find("Neutral").GetComponent<PressableButton>().OnClicked.AddListener(() => neutral());
        }

        public void setDraggable(Draggable new_draggable)
        {
            draggable = new_draggable;
        }

        public void positive()
        {
            draggable.dragAndDrop();
        }

        public void negative()
        {
            draggable.goBack();
        }

        public void neutral()
        {
            
        }

        public void close()
        {
            draggable.closeDialog();
        }

        public void OnEnable()
        {
            if (draggable != null)
            {
                body.text = draggable.setBody();
            }
        }
    }
}

