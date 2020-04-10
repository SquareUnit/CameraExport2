using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBladeExtTrigg : MonoBehaviour
{
    private Collider ExteriorCollider;
    private BouncingBlade bouncingParent;

    // Start is called before the first frame update
    void Start()
    {
        ExteriorCollider = this.GetComponent<Collider>();
        bouncingParent = this.GetComponentInParent<BouncingBlade>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bouncingParent.isInExtTrigger = true;
            bouncingParent.Detect();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bouncingParent.isInExtTrigger = false;
            bouncingParent.animator.SetBool("detect", false);
        }
    }
}
