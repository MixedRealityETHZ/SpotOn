using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using static SceneGRAPHS.ObjectNode;
using static DraggablePROMPT.DraggablePrompt;
using static ObjectINFORMATION.ObjectInformation;
using static SpotON.SpotonAPP;
using static SceneGRAPHS.object_type;

using SpotON;
using SceneGRAPHS;
using DraggablePROMPT;
using ObjectINFORMATION;

using System.IO;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TemporaryDIALOG;
using System.Threading;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using Drawers;
using System.Security.Cryptography;
using UnityEditor;
using Unity.VisualScripting;


namespace Draggables
{
    public class Draggable : MonoBehaviour
    {
        private GameObject draggableGameObject { get; set; }
        private BoxCollider draggableBoxColider { get; set; }
        private MeshFilter draggableMeshFilter { get; set; }
        private Mesh meshSgPoints { get; set; }
        private Mesh meshRealPoints { get; set; }
        private MeshRenderer draggableMeshRenderer { get; set; }
        private Material draggableMaterial { get; set; }
        private ObjectNode draggableNode { get; set; }
        private XRSimpleInteractable draggableInteractor { get; set; }
        private DraggablePrompt draggablePrompt { get; set; }
        private ObjectInformation draggableInformation { get; set; }
        private GameObject spotonApp { get; set; }
        private int draggableId { get; set; }
        private bool thrown { get; set; }

        public void Initialize(ObjectNode objectNode, int nodeId)
        {
            draggableNode = objectNode;
            draggableId = nodeId;
            draggableGameObject = this.gameObject;
            thrown = false;

            draggableBoxColider = draggableGameObject.AddComponent<BoxCollider>();

            draggableMeshFilter = draggableGameObject.AddComponent<MeshFilter>();
            draggableMeshRenderer = draggableGameObject.AddComponent<MeshRenderer>();
            draggableInteractor = draggableGameObject.AddComponent<XRSimpleInteractable>();

            spotonApp = draggableGameObject.transform.parent.transform.parent.transform.parent.transform.parent.gameObject;
            draggablePrompt = spotonApp.transform.Find("drag_and_drop_prompt").gameObject.GetComponent<DraggablePrompt>();
            draggableInformation = spotonApp.transform.Find("object_information").gameObject.GetComponent<ObjectInformation>();

            meshSgPoints = Resources.Load<Mesh>(Path.Combine("scene_graph_color", draggableNode.Name));
            meshRealPoints = Resources.Load<Mesh>(Path.Combine("real_life_color", draggableNode.Name));

            draggableMaterial = new Material(Shader.Find("Point Cloud/Point"));
            draggableMaterial.SetFloat("_PointSize", 0.09f);
            draggableMaterial.SetInt("_ApplyDistance", 1);
            draggableMeshRenderer.material = draggableMaterial;

            draggableGameObject.transform.localPosition = new Vector3(0f, 1.2498f, 3.1564f);
            draggableGameObject.transform.localRotation = Quaternion.Euler(90, 0, 0);
            draggableGameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            draggableBoxColider.center = new Vector3(draggableNode.Centroid[0], draggableNode.Centroid[1], draggableNode.Centroid[2]);
            draggableBoxColider.size = new Vector3(0.3f, 0.3f, 0.3f);

            draggableInteractor.selectEntered.AddListener(OnSelectEntered);
            draggableInteractor.hoverEntered.AddListener(OnHoverEntered);
            draggableInteractor.hoverExited.AddListener(OnHoverExited);

            show_color(true);
        }

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            if (args.interactorObject is XRGazeInteractor) return;

            if (args.interactorObject is XRRayInteractor)
            {
                draggableInformation.GetComponent<ObjectInformation>().set_object_information("This is draggable object " + draggableNode.Label + " with id " + (draggableId-1).ToString() + ".\n", object_type.DRAGGABLE);
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
            draggablePrompt.setDraggable(this);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.DRAGGABLE_PROMPT);
        }

        public void dragAndDrop()
        {
            spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Watch out!", "You've fetched the object succesfully. Careful now, the robots might start moving.", show_object.MAIN_SCENE, 5f);

            string message = "?job_type=drag_and_drop&robot_id=0&parameters=" + draggableId.ToString() + "%2CNone%2CNone";
            spotonApp.GetComponent<SpotonAPP>().notifyAllServers(message, message_type.JOB_REQUEST);
            thrown = true;

            spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);

            Debug.Log("DRAG AND DROP INITIATED: " + draggableNode.Label);
        }

        public void OnEnable()
        {
            if (thrown)
            {
                this.gameObject.SetActive(false);
            }
        }

        public void goBack()
        {
            spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Alright!", "We will put objact back in place!", show_object.MAIN_SCENE, 5f);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);
            Debug.Log("DRAG AND DROP STOPPED: " + draggableNode.Label);
        }


        public void closeDialog()
        {
            Debug.Log("CLOSE DRAWER DIALOG: " + draggableNode.Label);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.MAIN_SCENE);
        }

        public string setBody()
        {
            return "You have dragged " + draggableNode.Label + " with id " + draggableId.ToString() + ". Would you like to continue with the fetch? \n";
        }

        public void show_color(bool show_scene_graph)
        {
            draggableMeshFilter.mesh = show_scene_graph ? meshSgPoints : meshRealPoints;
        }

        void Update()
        {

        }
    }
}

