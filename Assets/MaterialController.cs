using System.Collections.Generic;
using UnityEngine;
using System;

public class MaterialController : MonoBehaviour
{
    public List<Material> wallMaterials;
    int wallMaterialIndex = 0;
    public List<Material> floorMaterials;
    int floorMaterialIndex = 0;
    public List<Material> ceilingMaterials;
    int ceilingMaterialIndex = 0;

    public void selectNext()
    {
        
        string label = PrefabSpawner.instance.focusedLabel;
        Debug.Log("selectNext label: " + label);
        setMaterial(label);
    }

    private void setMaterial(string label)
    {
        Material material;
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
            case "CEILING":
                ceilingMaterialIndex = (ceilingMaterialIndex + 1) % ceilingMaterials.Count;
                material = ceilingMaterials[ceilingMaterialIndex];
                label = "Ceiling";
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
    }
}
