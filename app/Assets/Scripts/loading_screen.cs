using UnityEngine;
using System.Collections;
using Microsoft.MixedReality.GraphicsTools;
using TMPro;
using MixedReality.Toolkit.UX;

using static SpotON.SpotonAPP;
using static TemporaryDIALOG.TemporaryDialog;

using SpotON;
using TemporaryDIALOG;
using ObjectINFORMATION;
using SceneGRAPHS;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit;

namespace ScreenLOADER
{
    public class ScreenLoader : MonoBehaviour
    {
        private CanvasGroup logo;
        private CanvasElementBeveledRect bar;
        private TextMeshProUGUI text;

        private Dialog tutorial_prompt;
        private GameObject main_scene;
        private GameObject loadingScreen;

        private bool finished_loading = true;
        private int counter = 0;
        private const int max_width = 490;
        private float deltaAlpha = 0;
        private float deltaTime = 0;
        private int current_width = 0;

        private bool load = false;

        public void Start()
        {
            loadingScreen = this.gameObject;

            if (loadingScreen != null)
            {
                logo = loadingScreen.transform.Find("app_logo").gameObject.GetComponent<CanvasGroup>();
                bar = loadingScreen.transform.Find("bar").Find("loading_bar").Find("bg").Find("fg").gameObject.GetComponent<CanvasElementBeveledRect>();
                text = loadingScreen.transform.Find("bar").Find("message").Find("text").gameObject.GetComponent<TextMeshProUGUI>();

                tutorial_prompt = loadingScreen.transform.parent.Find("tutorial_prompt").gameObject.GetComponent<Dialog>();
                main_scene = loadingScreen.transform.parent.Find("main_scene").gameObject;

            }

            logo.alpha = 0;
            text.text = "Loading scene ...";
            bar.rectTransform.sizeDelta = new Vector2(0, 40);
        }

    IEnumerator wait()
        {
            if (!finished_loading)
            {
                yield return new WaitForSeconds(1);
                loadingScreen.transform.parent.GetComponent<SpotonAPP>().show(show_object.TUTORIAL_PROMPT);
            }
        }

        public bool finished()
        {
            return !finished_loading;
        }

        private void updateMessage()
        {
            if (finished_loading)
            {
                if (current_width < 300)
                {
                    text.text = "Loading scene ...";
                }

                if (current_width > 300 && current_width < 450)
                {
                    text.text = "Conecting to a server ...";
                }

                if (current_width > 450 && current_width < max_width)
                {
                    text.text = "Loading finished.";
                }
            }

        }

        private void updateLogo()
        {
            if (counter % 15 == 0)
            {
                if (finished_loading)
                {
                    if (logo.alpha < 1)
                    {
                        logo.alpha += deltaAlpha;
                        deltaAlpha += deltaTime;
                    }
                    else
                    {
                        logo.alpha = 1;
                    }
                }
                counter = 1;
            }
        }

        private void updateBar()
        {
            if (finished_loading)
            {
                if (current_width >= max_width)
                {
                    current_width = max_width;
                    finished_loading = false;
                }
                else
                {
                    current_width += (int)(deltaTime * 200);
                }
                bar.rectTransform.sizeDelta = new Vector2(current_width, 40);
            }
        }

        private void OnEnable()
        {
            load = true;
        }

        private void Update()
        {
            if (load)
            {
                deltaTime = Time.deltaTime;
                counter++;
                updateBar();
                updateLogo();
                updateMessage();
                StartCoroutine(wait());
            }
        }
    }
}

