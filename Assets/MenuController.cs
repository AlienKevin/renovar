using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    public List<GameObject> objects;
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
