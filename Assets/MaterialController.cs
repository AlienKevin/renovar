using System.Collections.Generic;
using UnityEngine;
using System;

public class MaterialController : MonoBehaviour
{
    public List<Material> wallMaterials;
    int wallMaterialIndex = 0;
    public List<Material> floorMaterials;
    int floorMaterialIndex = 0;

    //private OVRSceneManager ovrSceneManager;

    //private void Awake()
    //{
    //    ovrSceneManager = FindObjectOfType<OVRSceneManager>();
    //}

    public void selectNext()
    {
        Material material;
        string label = PrefabSpawner.instance.focusedLabel;
        Debug.Log("selectNext label: " + label);
        switch (label)
        {
            case "FLOOR":
                floorMaterialIndex = (floorMaterialIndex + 1) % floorMaterials.Count;
                material = floorMaterials[floorMaterialIndex];
                label = "Floor";
                break;
            case "WALL_FACE":
                wallMaterialIndex = (wallMaterialIndex + 1) % wallMaterials.Count;
                material = wallMaterials[wallMaterialIndex];
                label = "Wall";
                break;
            default:
                throw new ArgumentException("Invalid label: " + label);
        }

        List<OVRSceneAnchor> anchors = new();
        OVRSceneAnchor.GetSceneAnchors(anchors);
        foreach (var anchor in anchors)
        {
            Debug.Log("anchor.name: " + anchor.name);
            if (anchor.name.StartsWith(label + "Override"))
            {
                anchor.gameObject.GetComponent<MeshRenderer>().material = material;
            }
        }

        //foreach (var @override in ovrSceneManager.PrefabOverrides)
        //{
        //    if (@override.ClassificationLabel == label)
        //    {
        //        Debug.Log("@override.Prefab.gameObject:", @override.Prefab.gameObject);
        //        @override.Prefab.gameObject.GetComponent<MeshRenderer>().material = material;
        //        break;
        //    }
        //}
    }
}
