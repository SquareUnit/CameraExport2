using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorneSave : Triggerable
{
    public bool activate = false, menuUI = false;

    public override void Activate()
    {
        //appel la fonction qui save 
        activate = true;
    }

    public override void Deactivate()
    {
        activate = false;
    }

    public override void Toggle()
    {
        //Pas Besoin
    }

    private void Update()
    {
        if(menuUI == true)
        {
            //montre le menu UI
        }
    }

    private void LateUpdate()
    {
        if (activate == true)
        {
            if (Input.GetButton("X"))
            {
                menuUI = true;
            }
        }
        if (activate == false)
        {
            if (menuUI == true)
            {
                menuUI = false;
            }
        }
    }

}
