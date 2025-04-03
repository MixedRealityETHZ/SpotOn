using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

using static SpotON.SpotonAPP;
using static SceneGRAPHS.object_type;

using SpotON;
using SceneGRAPHS;

namespace ObjectINFORMATION
{
    public class ObjectInformation : MonoBehaviour
    {
        private TextMeshProUGUI header { get; set; }
        private TextMeshProUGUI body { get; set; }
        private Image icon { get; set; }
        private GameObject temporaryDialog { get; set; }
        private object_type objectType { get; set; }

        public void Awake()
        {
            temporaryDialog = this.gameObject;
            header = temporaryDialog.transform.Find("Canvas").Find("Header").gameObject.GetComponent<TextMeshProUGUI>();
            body = temporaryDialog.transform.Find("Canvas").Find("Main Text").gameObject.GetComponent<TextMeshProUGUI>();
            icon = temporaryDialog.transform.Find("Canvas").Find("icon").Find("Image").gameObject.GetComponent<Image>();
        }

        public void set_object_information(string new_body, object_type object_type)
        {
            if (temporaryDialog == null)
            {
                temporaryDialog = this.gameObject;
                header = temporaryDialog.transform.Find("Canvas").Find("Header").gameObject.GetComponent<TextMeshProUGUI>();
                body = temporaryDialog.transform.Find("Canvas").Find("Main Text").gameObject.GetComponent<TextMeshProUGUI>();
                icon = temporaryDialog.transform.Find("Canvas").Find("icon").Find("Image").gameObject.GetComponent<Image>();
            }

            temporaryDialog.transform.localScale = new Vector3(3f, 3f, 3f);
            temporaryDialog.transform.localPosition = temporaryDialog.transform.localPosition + new Vector3(0f, -1f, 0f);

            objectType = object_type;
            switch (objectType)
            {
                case object_type.DRAWER:
                    icon.sprite = Resources.Load<Sprite>(Path.Combine("icons", "Drawer"));
                    icon.transform.parent.localScale = new Vector3(0.3f, 0.5f, 1f);
                    header.text = "Drawer";
                    break;
                case object_type.LAMP:
                    icon.sprite = Resources.Load<Sprite>(Path.Combine("icons", "Light"));
                    icon.transform.parent.localScale = new Vector3(0.3f, 0.5f, 1f);
                    header.text = "Lamp";
                    break;
                case object_type.DRAGGABLE:
                    icon.sprite = Resources.Load<Sprite>(Path.Combine("icons", "Draggable"));
                    icon.transform.parent.localScale = new Vector3(0.3f, 0.5f, 1f);
                    header.text = "Draggable Object";
                    break;
                case object_type.LIGHT_SWITCH:
                    icon.sprite = Resources.Load<Sprite>(Path.Combine("icons", "LightSwitch"));
                    icon.transform.parent.localScale = new Vector3(0.6f, 0.8f, 1f);
                    header.text = "Light Switch";
                    break;
                case object_type.ROBOT:
                    icon.sprite = Resources.Load<Sprite>(Path.Combine("icons", "spot"));
                    icon.transform.parent.localScale = new Vector3(0.6f, 0.8f, 1f);
                    header.text = "Robot";
                    break;
                case object_type.BASE:
                    icon.sprite = Resources.Load<Sprite>(Path.Combine("icons", "app_icon"));
                    icon.transform.parent.localScale = new Vector3(0.6f, 0.8f, 1f);
                    header.text = "Scene";
                    break;
                case object_type.UNMOVABLE:
                    break;
            }

            body.text = new_body;
        }

    }
}