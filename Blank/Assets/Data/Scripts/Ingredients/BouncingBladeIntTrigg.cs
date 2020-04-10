using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBladeIntTrigg : MonoBehaviour
{
    private Collider InteriorCollider;
    private BouncingBlade bouncingParent;

    // Start is called before the first frame update
    void Start()
    {
        InteriorCollider = this.GetComponent<Collider>();
        bouncingParent = this.GetComponentInParent<BouncingBlade>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bouncingParent.isInIntTrigger = true;
            bouncingParent.TriggerTrap();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bouncingParent.isInExtTrigger = true;
            bouncingParent.isInIntTrigger = false;
            bouncingParent.Detect();
        }
    }
}
