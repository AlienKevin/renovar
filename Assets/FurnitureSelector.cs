using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureSelector : MonoBehaviour
{
    //Distance of canvas to the camera
    public GameObject canvas;
    public float distanceFromCamera = 2.0f; 
    public float heightOffset = 0.3f;

    //Add more prefabs in this field
    public GameObject[] furniturePrefabs;

    //Initial index of the selected furniture
    private int selectedFurnitureIndex = -1;

    void Update()
    {
        // Keep the canvas at the top edge of the view
        PositionCanvas();

        //Input.GetMouseButtonDown(0) for Computer Test
        //OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) for Controller Test
        if (Input.GetMouseButtonDown(0))
        {
            CreateFurniture(selectedFurnitureIndex);
        }
    }


    //Always position canvas on top of the screen
    private void PositionCanvas()
    {
        if (canvas != null)
        {
            Vector3 newPosition = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera;
            newPosition.y += heightOffset;
            canvas.transform.position = newPosition;

            // Align the canvas to face the camera, only rotating around the y-axis
            canvas.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        }
    }

    public void CreateFurniture(int index)
    {
        Debug.Log("Trying to create furniture with index: " + index);
        if (index >= 0 && index < furniturePrefabs.Length)
        {
            Vector3 position = Camera.main.transform.position + Camera.main.transform.forward * distanceFromCamera;
            GameObject furniture = Instantiate(furniturePrefabs[index], position, Quaternion.identity);
            furniture.transform.LookAt(new Vector3(Camera.main.transform.position.x, furniture.transform.position.y, Camera.main.transform.position.z));
            Debug.Log("Furniture instantiated: " + furniturePrefabs[index].name);
        }

    }

    // Call this method from the UI button's OnClick event to set the selected furniture index
    public void SetSelectedFurnitureIndex(int index)
    {
        selectedFurnitureIndex = index;
        Debug.Log("Selected furniture index: " + selectedFurnitureIndex);
    }
}
