using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class apparitionTitreNiveau : MonoBehaviour {

    private Image titreNiveauImage;
    private float cpt = 4f;
    private Color opaqueColor = new Color(255, 255, 255, 1);
    private Canvas canvasOverlay;
    private BoxCollider bC;

    Color curColor =  new Color(255, 255, 255, 0);


    // Start is called before the first frame update
    void Start()
    {
        titreNiveauImage = GetComponentInChildren<Image>();
        canvasOverlay = GetComponentInChildren<Canvas>();
        bC = GetComponent<BoxCollider>();
        titreNiveauImage.color = curColor;
        canvasOverlay.gameObject.SetActive(false);
        titreNiveauImage.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (titreNiveauImage.gameObject.activeInHierarchy) titreNiveauImage.color = curColor; 
    }

    public void OnTriggerEnter(Collider other)
    {
        canvasOverlay.gameObject.SetActive(true);
        titreNiveauImage.gameObject.SetActive(true);
        StartCoroutine("IncrementationAlpha");       
    }


    public IEnumerator IncrementationAlpha()
    {
        while (curColor.a <=1)
        {
            curColor.a += 0.05f;
            yield return new WaitForSeconds(0.1f);

        }
        StartCoroutine("DecrementationAlpha");
        yield return new WaitForSeconds(2f);
    }

    public IEnumerator DecrementationAlpha()
    {
        while (curColor.a >= 0)
        {
            curColor.a -= 0.05f;
            yield return new WaitForSeconds(0.1f);

        }

        canvasOverlay.gameObject.SetActive(false);
        titreNiveauImage.gameObject.SetActive(false);
        bC.gameObject.SetActive(false);


        yield return new WaitForSeconds(1f);
    }
}
