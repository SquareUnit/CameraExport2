using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadlessAlly : MonoBehaviour
{
    // rond bool
    // carre trigger
    private bool animationOn = false;
    private Animator headAllyAnimator;
    // Start is called before the first frame update
    void Start()
    {
        headAllyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animationOn)
        {
            headAllyAnimator.SetBool("isGrabing", false);
            headAllyAnimator.SetTrigger("isPatrolling");
            headAllyAnimator.SetTrigger("isScared");
            headAllyAnimator.SetTrigger("isZap");

            headAllyAnimator.SetTrigger("blankiIsClose");
            headAllyAnimator.SetTrigger("blankInRange");

            headAllyAnimator.SetTrigger("gotThrowable"); // desuet ?
            headAllyAnimator.SetBool("attack", false); // desuet ?
            headAllyAnimator.SetTrigger("isRunning");
            headAllyAnimator.SetTrigger("blankIsDetected");
            headAllyAnimator.SetTrigger("NME"); // detect si un Headhunter est present ?

            // Ce qui est pertinent pour Headless Neutre
            headAllyAnimator.SetBool("reaction1", false);
            headAllyAnimator.SetBool("reaction2", false);

        }

    }
}
