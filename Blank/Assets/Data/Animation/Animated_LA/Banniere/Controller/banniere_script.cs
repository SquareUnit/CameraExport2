using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class banniere_script : MonoBehaviour
{

    public Animator animatorusRex;

    public bool wind01;
    public bool wind02;

    // Start is called before the first frame update
    void Start()
    {
        animatorusRex = gameObject.GetComponent<Animator>();

        if (wind01)
        {
            animatorusRex.SetBool("wind01", true);
        }
        else if (wind02)
        {
            animatorusRex.SetBool("wind02", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
