//Creer par Valentin. 
//Date de création [2019-04-08].
//Modification Valentin le [2019-04-09]

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
    public MenuBoutonController menuButtonController;
    public bool disable;

    //[valentin 2019-04-05] fonction pour jouer un son une fois.
    void PlayFeedBackSound (AudioClip feedBackSound)
    {
        if (!disable)
            menuButtonController.audioSource.PlayOneShot(feedBackSound);
        else
            disable = false;
    }
}
