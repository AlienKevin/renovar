using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public static PrefabSpawner instance;

    public string focusedLabel;
    private GameObject objectPrefab;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        objectPrefab = Instantiate(MenuController.instance.GetSelectedObject());
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

            if (anchor != null)
            {
                focusedLabel = anchor.Labels[0];
                //Debug.Log("Focused Label: " + focusedLabel);
                //Debug.Log(anchor.Labels);
            }

            objectPrefab.transform.position = hit.point;
            objectPrefab.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                Instantiate(objectPrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            }
        }
    }
}
