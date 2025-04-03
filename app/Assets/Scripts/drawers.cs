using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using static SceneGRAPHS.ObjectNode;
using static DrawerPROMPT.DrawerPrompt;
using static ObjectINFORMATION.ObjectInformation;
using static SpotON.SpotonAPP;
using static SceneGRAPHS.object_type;

using SpotON;
using SceneGRAPHS;
using DrawerPROMPT;
using ObjectINFORMATION;

using System.IO;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using MixedReality.Toolkit.UX;
using TemporaryDIALOG;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


namespace Drawers
{
    public enum drawer_content 
    { 
        NOT_SEARCHED = 0,
        FULL = 1,
        EMPTY = 2
    }

    public enum drawer_state
    {
        OPEN = 0,
        CLOSED = 1
    }

    public class Drawer : MonoBehaviour
    {
        private GameObject drawerGameObject { get; set; }
        private BoxCollider drawerBoxColider { get; set; }
        private MeshFilter drawerMeshFilter { get; set; }
        private Mesh meshSgPoints { get; set; }
        private Mesh meshRealPoints { get; set; }
        private MeshRenderer drawerMeshRenderer { get; set; }
        private Material drawerMaterial { get; set; }
        private ObjectNode drawerNode { get; set; }
        private XRSimpleInteractable drawerInteractor { get; set; }
        private DrawerPrompt drawerPrompt { get; set; }
        private ObjectInformation drawerInformation { get; set; }
        private GameObject spotonApp { get; set; }
        private int drawerId { get; set; }
        private drawer_state state { get; set; }
        private drawer_content content { get; set; }

        public void Initialize(ObjectNode objectNode, int nodeId)
        {
            drawerNode = objectNode;
            drawerId = nodeId;
            drawerGameObject = this.gameObject;

            state = drawer_state.CLOSED;
            content = drawer_content.NOT_SEARCHED;

            drawerBoxColider = drawerGameObject.AddComponent<BoxCollider>();
            drawerMeshFilter = drawerGameObject.AddComponent<MeshFilter>();
            drawerMeshRenderer = drawerGameObject.AddComponent<MeshRenderer>();
            drawerInteractor = drawerGameObject.AddComponent<XRSimpleInteractable>();

            spotonApp = drawerGameObject.transform.parent.transform.parent.transform.parent.transform.parent.gameObject;
            drawerPrompt = spotonApp.transform.Find("drawer_prompt").gameObject.GetComponent<DrawerPrompt>();
            drawerInformation = spotonApp.transform.Find("object_information").gameObject.GetComponent<ObjectInformation>();

            meshSgPoints = Resources.Load<Mesh>(Path.Combine("scene_graph_color", drawerNode.Name));
            meshRealPoints = Resources.Load<Mesh>(Path.Combine("real_life_color", drawerNode.Name));

            drawerMaterial = new Material(Shader.Find("Point Cloud/Point"));
            drawerMaterial.SetFloat("_PointSize", 0.09f);            
            drawerMaterial.SetInt("_ApplyDistance", 1);
            drawerMeshRenderer.material = drawerMaterial;

            Quaternion rot = Quaternion.Euler(90, 0, 0);
            drawerGameObject.transform.localPosition = new Vector3(0f, 1.2498f, 3.1564f);
            drawerGameObject.transform.localRotation = rot;
            drawerGameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            drawerBoxColider.center = new Vector3(drawerNode.Centroid[0], drawerNode.Centroid[1], drawerNode.Centroid[2]);
            drawerBoxColider.size = new Vector3(0.3f, 0.3f, 0.3f);

            drawerInteractor.selectEntered.AddListener(OnSelectEntered);
            drawerInteractor.hoverEntered.AddListener(OnHoverEntered);
            drawerInteractor.hoverExited.AddListener(OnHoverExited);

            show_color(true);
        }

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            if (args.interactorObject is XRGazeInteractor) return;
            if (args.interactorObject is XRRayInteractor)
            {
                drawerInformation.GetComponent<ObjectInformation>().set_object_information(setBody(), object_type.DRAWER);
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
            drawerPrompt.setDrawer(this);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.DRAWER_PROMPT);
        }

        public void searchAndDrop()
        {
            spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Watch out!", "You've successfully started searching the drawer. Careful now, the robots might start moving.", show_object.MAIN_SCENE, 5f);

            string message = "?job_type=search_drawer&robot_id=0&parameters=" + drawerId.ToString() + "%2CTrue";
            spotonApp.GetComponent<SpotonAPP>().notifyAllServers(message, message_type.JOB_REQUEST);
            state = drawer_state.OPEN;
            content = drawer_content.FULL;

            spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);
            Debug.Log("SEEK AND DESTROY DRAWER: " + drawerNode.Label);
        }

        public void open()
        {
            
            if (state == drawer_state.OPEN)
            {
                spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Oh no!", "This drawer seem to be already open. Do you want to close it maybe?", show_object.DRAWER_PROMPT, 5f);
                spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);
            }
            else
            {


                spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Watch out!", "You've opened a drawer. Careful now, the robots might start moving.", show_object.MAIN_SCENE, 5f);

                string message = "?job_type=open_drawer&robot_id=0&parameters=" + drawerId.ToString();
                spotonApp.GetComponent<SpotonAPP>().notifyAllServers(message, message_type.JOB_REQUEST);

                spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);
                state = drawer_state.OPEN;
            }
            Debug.Log("OPEN DRAWER: " + drawerNode.Label);
            
        }

        public void close()
        {
            
            if (state == drawer_state.CLOSED)
            {
                spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Oh no!", "This drawer seem to be already closed. Do you want to open it maybe?", show_object.DRAWER_PROMPT, 5f);
                spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);
            }
            else
            {
                spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Watch out!", "You've closed a drawer. Careful now, the robots might start moving.", show_object.MAIN_SCENE, 5f);

                string message = "?job_type=close_drawer&robot_id=0&parameters=" + drawerId.ToString();
                spotonApp.GetComponent<SpotonAPP>().notifyAllServers(message, message_type.JOB_REQUEST);

                spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);
                state = drawer_state.CLOSED;
            }
            Debug.Log("CLOSE DRAWER: " + drawerNode.Label);
            
        }

        public void closeDialog()
        {
            Debug.Log("CLOSE DRAWER DIALOG: " + drawerNode.Label);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.MAIN_SCENE);
        }

        public string setBody()
        {
            string state_string = (state == drawer_state.OPEN) ? "OPEN" : "CLOSED";
            string content_string = stringifyContent();
            return "Label: " + drawerNode.Label + "\nState: " + state_string + "\nContent: " + content_string + "\n";
        }


        private string stringifyContent()
        {
            switch (content)
            {
                case drawer_content.EMPTY:
                    return "EMPTY";
                case drawer_content.FULL:
                    return "FULL";
                case drawer_content.NOT_SEARCHED:
                    return "NOT SEARCHED";
            }
            return "";
        }

        public void show_color(bool show_scene_graph)
        {
            drawerMeshFilter.mesh = show_scene_graph ? meshSgPoints : meshRealPoints;
        }

        void Update()
        {
           
        }
    }
}

