using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class material_controller : MonoBehaviour
{
    public void select_floor_material(Material material)
    {
        Debug.Log("select_floor_material: " + material);
        var floor_mesh_render = GameObject.Find("FloorOverride").GetComponent<MeshRenderer>();
        floor_mesh_render.materials[0] = material;
    }
}
