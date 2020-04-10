using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FXTester : MonoBehaviour
{
    Button activation;
    public ParticleSystem fxSystem;
    public Animator animator;
    public string animParameterToTrigger;
    // Start is called before the first frame update
    void Start()
    {
        activation = GetComponentInChildren<Button>();
    }

    public void Toggle()
    {
        fxSystem?.Play();
        animator?.SetBool(animParameterToTrigger, !animator.GetBool(animParameterToTrigger));
        animator?.SetTrigger(animParameterToTrigger);
    }


}
