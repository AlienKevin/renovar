using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public static PrefabSpawner instance;

    public string focusedLabel = "FLOOR";
    private GameObject objectPrefab;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        objectPrefab = Instantiate(MenuController.instance.GetSelectedObject("FLOOR"));
    }

    public void UpdateObjectPrefab(GameObject newObject)
    {
        Destroy(objectPrefab);
        objectPrefab = Instantiate(newObject);
    }

    void Update()
    {
        Ray ray = new Ray(OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            OVRSemanticClassification anchor = hit.collider.gameObject.GetComponentInParent<OVRSemanticClassification>();

            Debug.Log("Hit GameObject: " + hit.collider.gameObject);
            Debug.Log("Hit GameObject Name: " + hit.collider.gameObject.name);

            if (anchor != null)
            {
                string newLabel = anchor.Labels[0];
                if (newLabel != focusedLabel) {
                    focusedLabel = newLabel;
                    UpdateObjectPrefab(MenuController.instance.GetSelectedObject(newLabel));
                }
                Debug.Log("Focused Label: " + focusedLabel);
                //Debug.Log(anchor.Labels);
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

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                Debug.Log("hit.normal: " + hit.normal);
                Debug.Log("hit.rotation: " + hit.transform.rotation);
                Debug.Log("objectPrefab.transform.rotation: " + objectPrefab.transform.rotation);
                Instantiate(objectPrefab, hit.point, objectPrefab.transform.rotation);
            }
        }
    }
}
