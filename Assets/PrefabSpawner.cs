using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public static PrefabSpawner instance;
    public GameObject canvas;
    public string focusedLabel = "FLOOR";
    private GameObject objectPrefab;
    public float canvasHeightOffset = 0.1f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        objectPrefab = Instantiate(ObjectController.instance.GetSelectedObject("FLOOR"));
    }

    public void UpdateObjectPrefab(GameObject newObject)
    {
        Destroy(objectPrefab);
        objectPrefab = Instantiate(newObject);
    }

    //Always position canvas on top of the screen
    private void PositionCanvas()
    {
        if (canvas != null)
        {
            Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            Vector3 newPosition = controllerPosition;
            newPosition.y += canvasHeightOffset;
            canvas.transform.position = newPosition;

            // Align the canvas to face the camera, only rotating around the y-axis
            canvas.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        }
    }

    void Update()
    {
        PositionCanvas();


        Ray ray = new Ray(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            OVRSemanticClassification anchor = hit.collider.gameObject.GetComponentInParent<OVRSemanticClassification>();

            //Debug.Log("Hit GameObject: " + hit.collider.gameObject);
            //Debug.Log("Hit GameObject Name: " + hit.collider.gameObject.name);

            if (anchor != null)
            {
                string newLabel = anchor.Labels[0];
                Debug.Log("newLabel: " + newLabel);
                if (newLabel != focusedLabel) {
                    focusedLabel = newLabel;
                    UpdateObjectPrefab(ObjectController.instance.GetSelectedObject(newLabel));
                }
                //Debug.Log("Focused Label: " + focusedLabel);

                if (focusedLabel != "WALL_FACE" && focusedLabel != "FLOOR" && focusedLabel != "CEILING")
                {
                    return;
                }

                objectPrefab.transform.position = hit.point;

                if (focusedLabel == "WALL_FACE")
                {
                    objectPrefab.transform.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
                }
                else
                {
                    objectPrefab.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }

                Debug.Log("focusedLabel: " + focusedLabel);

                if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
                {
                    Debug.Log("hit.normal: " + hit.normal);
                    Debug.Log("hit.rotation: " + hit.transform.rotation);
                    Debug.Log("objectPrefab.transform.rotation: " + objectPrefab.transform.rotation);
                    Instantiate(objectPrefab, hit.point, objectPrefab.transform.rotation);
                }
            } else
            {
                Debug.Log("newLabel: null");
            }
        }
    }
}
