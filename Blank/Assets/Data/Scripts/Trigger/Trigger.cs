/*Script cree le : 05-04-2019
 * Derniere modification:22-04-2019
 * Par:Steven Judge
 * Description: Affecte un objet lorsqu'il est active/desactive
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public enum TriggerActivation { OnEnter, OnExit, OnStay }
    public TriggerActivation activateur;
    [Tooltip("Tag de l'object pouvant declencher ce trigger")]
    public string tagActivateur;

    public enum TriggerFunction { Activate, Deactivate, Toggle }
    public TriggerFunction function;
    public Triggerable triggerable;
    delegate void Function();
    Function currentFunction;
    [HideInInspector] public bool jeSuisActiver = false;
    [HideInInspector] public float dot;// positif si le trigger est activer par derriere, et negatif pas devant----se base sur le sens du trigger
    [HideInInspector] public bool isUse = false;
    [HideInInspector] public GameObject activant;

    private void Start()
    {
        if (triggerable != null)
        {
            TriggerEvent();
            triggerable.myTriggers.Add(this);
        }
    }

    void TriggerEvent()
    {
        if (function == TriggerFunction.Activate)
            currentFunction = triggerable.Activate;
        else if (function == TriggerFunction.Deactivate)
            currentFunction = triggerable.Deactivate;
        else if (function == TriggerFunction.Toggle)
            currentFunction = triggerable.Toggle;
    }

    private void OnTriggerEnter(Collider other)
    {
        activant = other.gameObject;

        if (triggerable != null && activant.CompareTag(tagActivateur))
        {
            if (currentFunction == triggerable.Toggle)
            {
                isUse = false;
            }

            if (isUse == false)
            {

                if (activateur == TriggerActivation.OnEnter)
                {
                    isUse = true;
                    currentFunction();
                    if (jeSuisActiver == false)
                    {
                        jeSuisActiver = true;
                    }
                    else if (jeSuisActiver == true)
                    {
                        jeSuisActiver = false;
                    }
                }
                if (activateur == TriggerActivation.OnStay)
                {
                    triggerable.Activate();
                }
                if (activateur == TriggerActivation.OnExit)
                {
                    return;
                }
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        activant = other.gameObject;
        if (triggerable != null && other.CompareTag(tagActivateur))
        {
            if (currentFunction == triggerable.Toggle)
            {
                isUse = false;
            }

            if (isUse == false)
            {
                if (activateur == TriggerActivation.OnExit)
                {
                    isUse = true;
                    currentFunction();
                    if (jeSuisActiver == false)
                    {
                        jeSuisActiver = true;
                    }
                    else if (jeSuisActiver == true)
                    {
                        jeSuisActiver = false;
                    }
                }
                if (activateur == TriggerActivation.OnStay)
                {
                    triggerable.Deactivate();
                }
                if (activateur == TriggerActivation.OnEnter)
                {
                    return;
                }
            }
        }
    }



    public void CompareWithAvatar(Vector3 avatarDirection)
    {
        dot = Vector3.Dot(transform.forward, avatarDirection - transform.position);
    }
}

