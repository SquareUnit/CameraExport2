/*Script cree le : 09-07-2019 : Gere le behaviore d'une souris selon la distance de l'avatar
 * Par: David Babin
 * Derniere modification: 
 * Par: 
 * Description: 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTrigger : MonoBehaviour
{
    public bool blankIsTooClose;
    public bool canReset;
    public float distance;
    public Animator animator;
    public Transform tr;
    public Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        tr = transform;
        startPosition = tr.position;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentAvatar != null)
        {
            if (animator.GetNextAnimatorStateInfo(0).IsName("resetPosition") && canReset)
            {
                canReset = false;
            }

            distance = Vector3.Distance(GameManager.instance.currentAvatar.tr.position, tr.position);

            if (distance <= 10f)
            {
                animator.SetTrigger("souris_stop");
            }
            else
            {
                animator.SetTrigger("souris_start");
                canReset = true;
            }
            
        }
    }
}
