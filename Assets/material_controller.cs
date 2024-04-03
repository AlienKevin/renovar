using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    //private OVRSceneManager ovrSceneManager;

    //private void Awake()
    //{
    //    ovrSceneManager = FindObjectOfType<OVRSceneManager>();
    //}

    private void set_material(string label, Material material)
    {
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

    public void set_floor_material(Material material)
    {
        set_material("Floor", material);
    }
}
