using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraSwitch : MonoBehaviour
{
    public Transform switchPoint; // Set this to the position where you want to switch cameras
    public Camera newCamera; // Set this to the new camera you want to activate

    void Update()
    {
        // Check if the player has reached the switch point
        if (transform.position.x >= switchPoint.position.x)
        {
            // Deactivate the current main camera
            Camera.main.enabled = false;

            // Activate the new camera
            newCamera.enabled = true;
        }
    }
}

