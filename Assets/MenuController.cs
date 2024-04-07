using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    public List<GameObject> objects;
    public GameObject menu;
    private int selectedObjectIndex = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    void Update()
    {
        menu.transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);        
        menu.transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
    }

    public void selectNext()
    {
        selectedObjectIndex = (selectedObjectIndex + 1) % objects.Count;
        PrefabSpawner.instance.UpdateObjectPrefab(objects[selectedObjectIndex]);
    }

    public GameObject GetSelectedObject()
    {
        return objects[selectedObjectIndex];
    }
}
