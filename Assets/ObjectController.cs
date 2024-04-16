using System;
using System.Collections.Generic;
using UnityEngine;

enum GridDirection
{
    Left,
    Right,
    Up,
    Down,
};

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

    private void Update()
    {
        if (PrefabSpawner.instance.focusedLabel == "FLOOR")
        {
            if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickLeft))
            {
                selectBasedOnGridDirection(GridDirection.Left);
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickRight))
            {
                selectBasedOnGridDirection(GridDirection.Right);
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp))
            {
                selectBasedOnGridDirection(GridDirection.Up);
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown))
            {
                selectBasedOnGridDirection(GridDirection.Down);
            }
        }
    }

    // Source: https://stackoverflow.com/a/1082938/6798201
    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    // Move in direction on a 3x3 grid
    private void selectBasedOnGridDirection(GridDirection direction)
    {
        var label = PrefabSpawner.instance.focusedLabel;
        switch (direction)
        {
            case GridDirection.Left:
                if (selectedIndex[label] == (selectedIndex[label] / 3 * 3)) {
                    selectedIndex[label] = ((selectedIndex[label] / 3 + 1) * 3) - 1;
                } else {
                    selectedIndex[label] = selectedIndex[label] - 1;
                }
                break;
            case GridDirection.Right:
                if (selectedIndex[label] == ((selectedIndex[label] / 3 + 1) * 3) - 1)
                {
                    selectedIndex[label] = (selectedIndex[label] / 3) * 3;
                }
                else
                {
                    selectedIndex[label] = selectedIndex[label] + 1;
                }
                break;
            case GridDirection.Up:
                selectedIndex[label] = mod(selectedIndex[label] - 3, 9);
                break;
            case GridDirection.Down:
                selectedIndex[label] = mod(selectedIndex[label] + 3, 9);
                break;
        }
        SelectObjectButton(label);
        PrefabSpawner.instance.UpdateObjectPrefab(objects(label)[selectedIndex[label]]);
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
        if (label == "FLOOR")
        {
            SelectObjectButton(label);
        }
        PrefabSpawner.instance.UpdateObjectPrefab(objects(label)[selectedIndex[label]]);
    }

    public void SelectObjectButton(String label)
    {
        var buttonName = "Button" + (selectedIndex[label] + 1);
        var uiButton = GameObject.Find(buttonName).GetComponent<UnityEngine.UI.Button>(); ;
        uiButton.Select();
    }

    public GameObject GetSelectedObject(string label)
    {
        Debug.Log("GetSelectedObject(" + label + "): " + objects(label)[selectedIndex[label]]);
        return objects(label)[selectedIndex[label]];
    }
}
