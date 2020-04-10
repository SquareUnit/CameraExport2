using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrandePorte : MonoBehaviour
{
    public enum StatePorte { close, open}
    public StatePorte stateDepart;
    private Animator anim;
    public Collider closedDoor;
    public Collider doorLeft;
    public Collider doorRight;

    private void Start()
    {
        anim = GetComponent<Animator>();

        switch (stateDepart)
        {
            case StatePorte.close:
                anim.SetBool("doorClose", true);
                closedDoor.enabled = true;
                doorLeft.enabled = false;
                doorRight.enabled = false;
                break;
            case StatePorte.open:
                anim.SetBool("doorOpen", true);
                closedDoor.enabled = false;
                doorLeft.enabled = true;
                doorRight.enabled = true;
                break;
        }
    }

    public void ColliderToOpenState()
    {
        closedDoor.enabled = false;
        doorLeft.enabled = true;
        doorRight.enabled = true;
    }

    public void ColliderToClosedState()
    {
        closedDoor.enabled = true;
        doorLeft.enabled = false;
        doorRight.enabled = false;
    }
}
