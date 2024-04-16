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
    private float selectedObjectRotationDegrees = 0f;
    private MeshRenderer anchorMeshRenderer;
    public Material redVolumePassthroughMaterial;
    public Material volumePassthroughMaterial;

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

        selectedObjectRotationDegrees = 0f;

        ObjectController.instance.SelectObjectButton(focusedLabel);
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

            //// Align the canvas to face the camera, only rotating around the y-axis and z-axis
            canvas.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y, 0);
            //canvas.transform.LookAt(Camera.main.transform);
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
                if (newLabel != focusedLabel) {
                    focusedLabel = newLabel;
                    Debug.Log("newLabel: " + newLabel);
                    if (anchorMeshRenderer != null)
                    {
                        anchorMeshRenderer.material = volumePassthroughMaterial;
                    }
                    if (focusedLabel != "WALL_FACE" && focusedLabel != "FLOOR" && focusedLabel != "CEILING")
                    {
                        anchorMeshRenderer = anchor.gameObject.GetComponentInChildren<MeshRenderer>();
                        if (anchorMeshRenderer != null)
                        {
                            anchorMeshRenderer.material = redVolumePassthroughMaterial;
                        }
                    }
                    else
                    {
                        UpdateObjectPrefab(ObjectController.instance.GetSelectedObject(newLabel));
                    }
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
                    objectPrefab.transform.rotation *= Quaternion.Euler(0f, selectedObjectRotationDegrees, 0f);
                }

                Debug.Log("focusedLabel: " + focusedLabel);

                if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                {
                    Debug.Log("hit.normal: " + hit.normal);
                    Debug.Log("hit.rotation: " + hit.transform.rotation);
                    Debug.Log("objectPrefab.transform.rotation: " + objectPrefab.transform.rotation);
                    Instantiate(objectPrefab, hit.point, objectPrefab.transform.rotation);
                }
                if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
                {
                    // Rotate counterclockwise to match the hand trigger press direction on the right hand.
                    selectedObjectRotationDegrees -= 1.0f;
                    selectedObjectRotationDegrees %= 360f;
                    Debug.Log("selectedObjectRotationDegrees: " + selectedObjectRotationDegrees);
                }
            } else
            {
                Debug.Log("newLabel: null");
            }
        }
    }
}
