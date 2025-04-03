using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup myUIGroup;
    [SerializeField] private bool fadeIn =  false;
    [SerializeField] private bool fadeOut = false;
    private int counter = 1;

    public void ShowUi()
    {
        fadeIn = true;
    }

    public void HideUi() 
    {
        fadeOut = true; 
    }

    public void Start()
    {
        myUIGroup.alpha = 0;
    }

    private void Update()
    {
        if (counter % 15 == 0)
        {
            if (fadeIn)
            {
                if (myUIGroup.alpha < 1)
                {
                    myUIGroup.alpha += Time.deltaTime;
                    if (myUIGroup.alpha >= 1)
                    {
                        fadeIn = false;
                    }
                }
            }

            if (fadeOut)
            {
                if (myUIGroup.alpha >= 0)
                {
                    myUIGroup.alpha += Time.deltaTime;
                    if (myUIGroup.alpha == 0)
                    {
                        fadeOut = false;
                    }
                }
            }
            counter = 1;
        }
        else
        {
            counter++;
        }

        
    }
}

