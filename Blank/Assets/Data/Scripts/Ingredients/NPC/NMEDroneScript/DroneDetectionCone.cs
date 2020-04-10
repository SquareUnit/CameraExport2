using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneDetectionCone : MonoBehaviour
{
    public bool playerInRange;

    private void OnTriggerEnter(Collider other)
    {
        playerInRange = true;
    }
    private void OnTriggerStay(Collider other)
    {
        playerInRange = true;
        if(!GameManager.instance.currentAvatar.controller.enabled)
        {
            playerInRange = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
    }
}