using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menu;

    void Update()
    {
        menu.transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);        
        menu.transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
    }
}
