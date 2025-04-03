using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

using static SceneGRAPHS.ObjectNode;
using static ObjectINFORMATION.ObjectInformation;
using static SpotON.SpotonAPP;
using static SceneGRAPHS.object_type;

using SpotON;
using SceneGRAPHS;
using ObjectINFORMATION;

using System.IO;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


namespace Unmovables
{
    public class Unmovable : MonoBehaviour
    {
        private GameObject unmovableGameObject { get; set; }
        private MeshFilter unmovableMeshFilter { get; set; }
        private Mesh meshSgPoints { get; set; }
        private Mesh meshRealPoints { get; set; }
        private MeshRenderer unmovableMeshRenderer { get; set; }
        private Material unmovableMaterial { get; set; }
        private ObjectNode unmovableNode { get; set; }
        private XRSimpleInteractable unmovableInteractor { get; set; }
        private ObjectInformation unmovableInformation { get; set; }
        private GameObject spotonApp { get; set; }
        private int unmovableId { get; set; }
        public void Initialize(ObjectNode objectNode, int nodeId)
        {
            unmovableNode = objectNode;
            unmovableId = nodeId;
            unmovableGameObject = this.gameObject;

            unmovableMeshFilter = unmovableGameObject.AddComponent<MeshFilter>();
            unmovableMeshRenderer = unmovableGameObject.AddComponent<MeshRenderer>();
            unmovableInteractor = unmovableGameObject.AddComponent<XRSimpleInteractable>();

            spotonApp = unmovableGameObject.transform.parent.transform.parent.transform.parent.transform.parent.gameObject;
            unmovableInformation = spotonApp.transform.Find("object_information").gameObject.GetComponent<ObjectInformation>();

            meshSgPoints = Resources.Load<Mesh>(Path.Combine("scene_graph_color", unmovableNode.Name));
            meshRealPoints = Resources.Load<Mesh>(Path.Combine("real_life_color", unmovableNode.Name));

            unmovableMaterial = new Material(Shader.Find("Point Cloud/Point"));
            unmovableMaterial.SetFloat("_PointSize", 0.09f);
            unmovableMaterial.SetInt("_ApplyDistance", 1);
            unmovableMeshRenderer.material = unmovableMaterial;

            unmovableGameObject.transform.localPosition = new Vector3(0f, 1.2498f, 3.1564f);
            unmovableGameObject.transform.localRotation = Quaternion.Euler(90, 0, 0);
            unmovableGameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            show_color(true);
        }
        public void show_color(bool show_scene_graph)
        {
            unmovableMeshFilter.mesh = show_scene_graph ? meshSgPoints : meshRealPoints;
        }

        void Update()
        {

        }
    }
}

