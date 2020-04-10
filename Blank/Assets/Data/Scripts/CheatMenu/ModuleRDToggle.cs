/*Script cree le : 16-05-2019
 * Par: David Babin
 * Derniere modification: 
 * Par: 
 * Description: 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModuleRDToggle : MonoBehaviour { 

    public Renderer rd;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponentInChildren<Renderer>();
        InputsManager.instance.ToggleRD.AddListener(DeactivateRD);
    }

    void DeactivateRD()
    {
        rd.enabled = !rd.enabled;
    }
}
