using SpotON;
using TMPro;
using UnityEngine;

using static SpotON.SpotonAPP;

namespace TemporaryDIALOG
{
    public class TemporaryDialog : MonoBehaviour
    {
        private float displayTime = 3f;

        private show_object next_object = show_object.MAIN_SCENE;
        private TextMeshProUGUI header { get; set; }
        private TextMeshProUGUI body { get; set; }
        private GameObject temporaryDialog { get; set; }


        public void Awake()
        {
            temporaryDialog = this.gameObject;
            header = temporaryDialog.transform.Find("Canvas").Find("Header").gameObject.GetComponent<TextMeshProUGUI>();
            body = temporaryDialog.transform.Find("Canvas").Find("Main Text").gameObject.GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            Invoke("HideDialog", displayTime);
        }

        public void set_temporary_dialog(string new_header, string new_body, show_object next_object_to_show, float time)
        {
            if (temporaryDialog == null)
            {
                temporaryDialog = this.gameObject;
                header = temporaryDialog.transform.Find("Canvas").Find("Header").gameObject.GetComponent<TextMeshProUGUI>();
                body = temporaryDialog.transform.Find("Canvas").Find("Main Text").gameObject.GetComponent<TextMeshProUGUI>();
            }

            next_object = next_object_to_show;
            displayTime = time;
            header.text = new_header;
            body.text = new_body;
        }

        private void HideDialog()
        {
            gameObject.SetActive(false);
            temporaryDialog.transform.parent.GetComponent<SpotonAPP>().show(next_object);
        }

        private void OnDisable()
        {
            CancelInvoke("HideDialog");
        }
    }
}