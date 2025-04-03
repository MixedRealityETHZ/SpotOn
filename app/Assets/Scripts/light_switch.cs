using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using static SceneGRAPHS.ObjectNode;
using static LightSwitchPROMPT.LightSwitchPrompt;
using static ObjectINFORMATION.ObjectInformation;
using static SpotON.SpotonAPP;
using static SceneGRAPHS.object_type;

using SpotON;
using SceneGRAPHS;
using LightSwitchPROMPT;
using ObjectINFORMATION;

using System.IO;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TemporaryDIALOG;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using Draggables;


namespace LightSWITCHES
{
    public class LightSwitch : MonoBehaviour
    {
        private GameObject lightSwitchGameObject { get; set; }
        private BoxCollider lightSwitchBoxColider { get; set; }
        private MeshFilter lightSwitchMeshFilter { get; set; }
        private Mesh meshSgPoints { get; set; }
        private Mesh meshRealPoints { get; set; }
        private MeshRenderer lightSwitchMeshRenderer { get; set; }
        private Material lightSwitchMaterial { get; set; }
        private ObjectNode lightSwitchNode { get; set; }
        private XRSimpleInteractable lightSwitchInteractor { get; set; }
        private LightSwitchPrompt lightSwitchPrompt { get; set; }
        private ObjectInformation lightSwitchInformation { get; set; }
        private GameObject spotonApp { get; set; }
        private int lightSwitchId { get; set; }
        private List<string> lampList { get; set; }

        public void Initialize(ObjectNode objectNode, int nodeId)
        {
            lightSwitchNode = objectNode;
            lightSwitchId = nodeId;
            lightSwitchGameObject = this.gameObject;
            lampList = new List<string>();

            lightSwitchBoxColider = lightSwitchGameObject.AddComponent<BoxCollider>();
            lightSwitchMeshFilter = lightSwitchGameObject.AddComponent<MeshFilter>();
            lightSwitchMeshRenderer = lightSwitchGameObject.AddComponent<MeshRenderer>();
            lightSwitchInteractor = lightSwitchGameObject.AddComponent<XRSimpleInteractable>();

            spotonApp = lightSwitchGameObject.transform.parent.transform.parent.transform.parent.transform.parent.gameObject;
            lightSwitchPrompt = spotonApp.transform.Find("light_switch_prompt").gameObject.GetComponent<LightSwitchPrompt>();
            lightSwitchInformation = spotonApp.transform.Find("object_information").gameObject.GetComponent<ObjectInformation>();

            meshSgPoints = Resources.Load<Mesh>(Path.Combine("scene_graph_color", lightSwitchNode.Name));
            meshRealPoints = Resources.Load<Mesh>(Path.Combine("real_life_color", lightSwitchNode.Name));

            lightSwitchMaterial = new Material(Shader.Find("Point Cloud/Point"));
            lightSwitchMaterial.SetFloat("_PointSize", 0.09f);
            lightSwitchMaterial.SetInt("_ApplyDistance", 1);
            lightSwitchMeshRenderer.material = lightSwitchMaterial;

            lightSwitchGameObject.transform.localPosition = new Vector3(0f, 1.2498f, 3.1564f);
            lightSwitchGameObject.transform.localRotation = Quaternion.Euler(90, 0, 0);
            lightSwitchGameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            lightSwitchBoxColider.center = new Vector3(lightSwitchNode.Centroid[0], lightSwitchNode.Centroid[1], lightSwitchNode.Centroid[2]);
            lightSwitchBoxColider.size = new Vector3(0.3f, 0.3f, 0.3f);

            lightSwitchInteractor.selectEntered.AddListener(OnSelectEntered);
            lightSwitchInteractor.hoverEntered.AddListener(OnHoverEntered);
            lightSwitchInteractor.hoverExited.AddListener(OnHoverExited);

            show_color(true);
        }

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            if (args.interactorObject is XRGazeInteractor) return;
            if (args.interactorObject is XRRayInteractor)
            {
                lightSwitchInformation.GetComponent<ObjectInformation>().set_object_information(setBody(), object_type.LIGHT_SWITCH);
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
            lightSwitchPrompt.setLightSwitch(this);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.LIGHT_SWITCH_PROMPT);
        }

        public void operateLightSwitch()
        {
            
            spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Watch out!", "You've clicked a light switch. Careful now, the robots might start moving now.", show_object.MAIN_SCENE, 5f);

            string message = "?job_type=click_light_switch&robot_id=0&parameters=" + lightSwitchId.ToString();
            spotonApp.GetComponent<SpotonAPP>().notifyAllServers(message, message_type.JOB_REQUEST);

            spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);

            Debug.Log("OPERATE LIGHT SWITCH: " + lightSwitchNode.Label);
            
        }

        public void checkConnection()
        {
            spotonApp.GetComponent<SpotonAPP>().getTemporaryDialog().GetComponent<TemporaryDialog>().set_temporary_dialog("Watch out!", "You've started a connection check betwen light switch and lamps. Careful now, the robots might start moving.", show_object.MAIN_SCENE, 5f);

            string message = "?job_type=check_lamp_lightswitch_connection_multi_robot&robot_id=0&parameters=" + lightSwitchId.ToString();
            spotonApp.GetComponent<SpotonAPP>().notifyAllServers(message, message_type.JOB_REQUEST);

            spotonApp.GetComponent<SpotonAPP>().show(show_object.TEMPORARY_DIALOG);

            Debug.Log("CHECK CONNECTION LIGHT SWITCH: " + lightSwitchNode.Label);
        }

        public void closeDialog()
        {
            Debug.Log("CLOSE LIGHT SWITCH DIALOG: " + lightSwitchNode.Label);
            spotonApp.GetComponent<SpotonAPP>().show(show_object.MAIN_SCENE);
        }

        public string setBody()
        {
            return "Label: " + lightSwitchNode.Label + "\n Lamps: " + stringifyLampList() + "\n";
        }


        private string stringifyLampList()
        {
            string lamp_list = "";

            if (lampList.Count == 0)
            {
                lamp_list = "There are currently no lamps connected to this light switch";
            }
            else
            {
                foreach(string lamp in lampList)
                {
                    lamp_list += lamp + ", ";
                }
                lamp_list = lamp_list.Remove(lamp_list.Length - 1, 1);
                lamp_list = lamp_list.Remove(lamp_list.Length - 1, 1);
            }
            return lamp_list;
        }

        public void show_color(bool show_scene_graph)
        {
            lightSwitchMeshFilter.mesh = show_scene_graph ? meshSgPoints : meshRealPoints;
        }

        void Update()
        {

        }
    }
}

