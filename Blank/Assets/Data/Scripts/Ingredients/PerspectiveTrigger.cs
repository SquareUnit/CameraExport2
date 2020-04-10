using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveTrigger : MonoBehaviour
{
    public bool playerInTrigger;

    private void OnTriggerEnter(Collider other)
    {
        playerInTrigger = true;
    }
    private void OnTriggerStay(Collider other)
    {
        playerInTrigger = true;
        if (!GameManager.instance.currentAvatar.controller.enabled)
        {
            playerInTrigger = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        playerInTrigger = false;
    }
}
