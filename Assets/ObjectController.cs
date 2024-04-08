using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public static ObjectController instance;

    public List<GameObject> floorObjects;
    public List<GameObject> wallObjects;
    public List<GameObject> ceilingObjects;
    private Dictionary<string, int> selectedIndex = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;

        selectedIndex["FLOOR"] = 0;
        selectedIndex["WALL_FACE"] = 0;
        selectedIndex["CEILING"] = 0;
    }

    private List<GameObject>objects(string label)
    {
        switch (label)
        {
            case "FLOOR":
                return floorObjects;
            case "WALL_FACE":
                return wallObjects;
            case "CEILING":
                return ceilingObjects;
            default:
                throw new ArgumentException("Invalid label: " + label);
        }
    }

    public void selectNextObject()
    {
        var label = PrefabSpawner.instance.focusedLabel;
        selectedIndex[label] = (selectedIndex[label] + 1) % objects(label).Count;
        PrefabSpawner.instance.UpdateObjectPrefab(objects(label)[selectedIndex[label]]);
    }

    public GameObject GetSelectedObject(string label)
    {
        Debug.Log("GetSelectedObject(" + label + "): " + objects(label)[selectedIndex[label]]);
        return objects(label)[selectedIndex[label]];
    }
}
