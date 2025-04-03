using MixedReality.Toolkit.SpatialManipulation;
using MixedReality.Toolkit.UX;
using SpotON;
using System.Collections;
using System.Collections.Generic;
using TemporaryDIALOG;
using UnityEngine;
using static SpotON.SpotonAPP;

namespace Tutorials
{
    public class Tutorial : MonoBehaviour
    {
        private GameObject spotonApp { get; set; }
        private GameObject tutorial { get; set; }

        public void Awake()
        {
            tutorial = this.gameObject;
            spotonApp = tutorial.transform.parent.gameObject;
        }

        public void OnEnable()
        {
            if ( spotonApp != null)
            {
                if ( tutorial != null)
                {
                    spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("We are so sorry!", "Tutorial will be ready shortly. For now you may have a look at our helper window.", show_object.HELPER_WINDOW, 5f);
                    spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);

                    spotonApp.GetComponent<SpotonAPP>().enableProcessesOnStart(true);
                }
            }

        }
    }
}

