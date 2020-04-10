/*Script cree le : 05-04-2019
 * Par: Steven Judge
 * Derniere modification:22-04-2019
 * Par:Steven Judge
 * Description: Represente l'ensemble des objects activable contient les fonction a implementer
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public abstract class Triggerable : MonoBehaviour
{
    [HideInInspector] public bool startActivated = false;
    [HideInInspector] public int state = 0;
    [HideInInspector] public List<Trigger> myTriggers;

    public abstract void Activate();
    public abstract void Deactivate();
    public abstract void Toggle();


}