using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using static SceneGRAPHS.ObjectNode;
using static LampPROMPT.LampPrompt;
using static ObjectINFORMATION.ObjectInformation;
using static SpotON.SpotonAPP;
using static SceneGRAPHS.object_type;

using SpotON;
using SceneGRAPHS;
using LampPROMPT;
using ObjectINFORMATION;

using System.IO;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TemporaryDIALOG;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Lamps
{
    public enum lamp_state
    {
        ON = 0,
        OFF = 1
    }

    public class Lamp : MonoBehaviour
    {
        private GameObject lampGameObject { get; set; }
        private BoxCollider lampBoxColider { get; set; }
        private MeshFilter lampMeshFilter { get; set; }
        private Mesh meshSgPoints { get; set; }
        private Mesh meshRealPoints { get; set; }
        private MeshRenderer lampMeshRenderer { get; set; }
        private Material lampMaterial { get; set; }
        private ObjectNode lampNode { get; set; }
        private XRSimpleInteractable lampInteractor { get; set; }
        private LampPrompt lampPrompt { get; set; }
        private ObjectInformation lampInformation { get; set; }
        private GameObject spotonApp { get; set; }
        private int lampId { get; set; }
        private lamp_state state { get; set; }

        public void Initialize(ObjectNode objectNode, int nodeId)
        {
            lampNode = objectNode;
            lampId = nodeId;
            lampGameObject = this.gameObject;

            state = lamp_state.OFF;

            lampBoxColider = lampGameObject.AddComponent<BoxCollider>();
            lampMeshFilter = lampGameObject.AddComponent<MeshFilter>();
            lampMeshRenderer = lampGameObject.AddComponent<MeshRenderer>();
            lampInteractor = lampGameObject.AddComponent<XRSimpleInteractable>();

            spotonApp = lampGameObject.transform.parent.transform.parent.transform.parent.transform.parent.gameObject;
            lampPrompt = spotonApp.transform.Find("lamp_prompt").gameObject.GetComponent<LampPrompt>();
            lampInformation = spotonApp.transform.Find("object_information").gameObject.GetComponent<ObjectInformation>();

            meshSgPoints = Resources.Load<Mesh>(Path.Combine("scene_graph_color", lampNode.Name));
            meshRealPoints = Resources.Load<Mesh>(Path.Combine("real_life_color", lampNode.Name));

            lampMaterial = new Material(Shader.Find("Point Cloud/Point"));
            lampMaterial.SetFloat("_PointSize", 0.09f);
            lampMaterial.SetInt("_ApplyDistance", 1);
            lampMeshRenderer.material = lampMaterial;

            lampGameObject.transform.localPosition = new Vector3(0f, 1.2498f, 3.1564f);
            lampGameObject.transform.localRotation = Quaternion.Euler(90, 0, 0);
            lampGameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            lampBoxColider.center = new Vector3(lampNode.Centroid[0], lampNode.Centroid[1], lampNode.Centroid[2]);
            lampBoxColider.size = new Vector3(0.3f, 0.3f, 0.3f);

            lampInteractor.selectEntered.AddListener(OnSelectEntered);
            lampInteractor.hoverEntered.AddListener(OnHoverEntered);
            lampInteractor.hoverExited.AddListener(OnHoverExited);

            show_color(true);
        }

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            if (args.interactorObject is XRGazeInteractor) return;
            if (args.interactorObject is XRRayInteractor)
            {
                lampInformation.GetComponent<ObjectInformation>().set_object_information(setBody(), object_type.LAMP);
                spotonApp.GetComponent<SpotonAPP>().show(show_object.OBJECT_INFORMATION, false);
            }
        }

        private void OnHoverExited(HoverExitEventArgs args)
        {
            if (args.interactorObject is XRGazeInteractor) return;
            if (args.interactorObject is XRRayInteractor)
            {
                spotonApp.GetComponent<SpotonAPP>().hide(show_object.OBJECT_INFORMATION);
            }
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            lampPrompt.setLamp(this);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.LAMP_PROMPT);
        }

        public void checkLampState()
        {
            /*
            spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Watch out!", "You've started the check of this lamp's state. Carfeul now, the robots might start moving.", show_object.MAIN_SCENE, 5f);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);

            Debug.Log("CHECK LAMP STATE LAMP: " + lampNode.Label);
            */
        }

        public void closeDialog()
        {
            spotonApp.GetComponent<SpotonAPP>().show(show_object.MAIN_SCENE);
            Debug.Log("CLOSE LAMP DIALOG: " + lampNode.Label);
        }

        public string setBody()
        {
            string state_string = (state == lamp_state.ON) ? "ON" : "OFF";
            return "Label: " + lampNode.Label + "\nState: " + state_string + "\n";
        }
        public void show_color(bool show_scene_graph)
        {
            lampMeshFilter.mesh = show_scene_graph ? meshSgPoints : meshRealPoints;
        }

        void Update()
        {

        }
    }
}

