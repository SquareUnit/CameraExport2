using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mannequin : MonoBehaviour
{
    private Transform tr;
    public GameObject mannequinHead;
    public GameObject newMannequinHead;
    public Rigidbody newMannequinHeadRb;

    private void Start()
    {
        tr = transform;
    }

    public void LoseHead()
    {
        newMannequinHead = Instantiate(newMannequinHead, tr.position, tr.rotation);
        mannequinHead.SetActive(false);
        newMannequinHeadRb = newMannequinHead.GetComponent<Rigidbody>();
        newMannequinHeadRb.AddForce(newMannequinHead.transform.up);
        newMannequinHeadRb.AddTorque(newMannequinHead.transform.up);
    }
}
