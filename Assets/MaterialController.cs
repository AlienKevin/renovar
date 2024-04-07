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

    public void selectNext(string label)
    {
        Material material;
        switch (label)
        {
            case "Floor":
                floorMaterialIndex = (floorMaterialIndex + 1) % floorMaterials.Count;
                material = floorMaterials[floorMaterialIndex];
                break;
            case "Wall":
                wallMaterialIndex = (wallMaterialIndex + 1) % wallMaterials.Count;
                material = wallMaterials[wallMaterialIndex];
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
