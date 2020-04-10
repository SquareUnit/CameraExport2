using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFirstInLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Headless"))
        {
            HeadlessNPC headless = other.GetComponentInParent<HeadlessNPC>();
            headless.isFirstInLine = true;
        }
    }
}
