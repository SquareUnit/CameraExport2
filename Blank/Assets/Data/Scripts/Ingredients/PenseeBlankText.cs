using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Bibliotheque de Text Mesh Pro

public class PenseeBlankText : MonoBehaviour {

    /*Script cree le : 06-05-2019
     * Par: Chloe Rocheleau
     * Derniere modification: 14-05-2019
     * Par: Eve Gaboury, David Babin
     * Description de la modif: UI en TMP objet 3D ( pas besoin de canvas ) 
     *          enlever les debugs et changer les distances
     *          enlever tr.position dans le if qui teste la valeur du tr de l<Avatar
     * Par: Philippe Beauchemin
     * Description: Ajout du compte des decal collectible
     *          
     */

    //Proprietes
    public TMP_Text text;
    public TMP_Text monTexte;
    public TMP_Text myText;
    public bool playSound = true;
    public float fadeSpd = 0.005f;
    //[TextArea(8, 8)] public string textePensee;
    private Transform tr;
    private MeshRenderer monRenderer;
    private bool isActive = false;

    private Vector3 positionAvatar;
    private float distance;
 
    public enum DD { close, medium, far };
    [Header("Distance avant que la pensee disparaisse")]
    public DD dd;
    private int distanceFixe;
    private BoxCollider col;
    public SoundRelay soundRelay;

    private bool decrementIsPossible = false;

    public bool isCollectible;

    [HideInInspector] public SpecificLevelManager monSpecificLevelManager;

    // Start is called before the first frame update
    void Start()
    {
        myText.alpha = 0f;
        monTexte.alpha = 0f;
        myText.transform.position = monTexte.transform.position;
        myText.transform.rotation = monTexte.transform.rotation;

        if (GameManager.instance.langue == GameManager.langues.english)
            text = myText.GetComponentInChildren<TextMeshPro>();
        else
            text = monTexte.GetComponent<TextMeshPro>();

        tr = transform;
        //monTexte = GetComponentInChildren<TextMeshPro>();
        monRenderer = GetComponentInChildren<MeshRenderer>();
        col = GetComponent<BoxCollider>();
        soundRelay = GetComponent<SoundRelay>();


        text.alpha = 0;     
        distanceFixe = (int)dd;
        DistanceDeDisparition();

        monSpecificLevelManager = GameManager.instance.spcLvlMan;
        if (isCollectible) {
            monSpecificLevelManager.totalCollectible += 1;
        }
    }

    private void LateUpdate()
    {
        if (GameManager.instance.currentAvatar != null)
        {
            if (GameManager.instance.currentAvatar.tr != null)
            {
                positionAvatar = GameManager.instance.currentAvatar.tr.position;
                distance = Vector3.Distance(col.ClosestPoint(tr.position), positionAvatar);
                //Debug.Log(distance);
            }
        }

        //Faire disparaitre quand on est assez loin (selon la distance de disparition)
        if (decrementIsPossible && distance >= distanceFixe)
        {
            StartCoroutine("DecrementationAlpha", fadeSpd);
        }
    }

    public void OnTriggerEnter(Collider other){
        if (other) {
            if (monRenderer.isVisible)
            {
                //Debug.Log("is visible");
                StartCoroutine("IncrementationAlpha", fadeSpd);

                // Ajout du suivi des collectibles
                if (isCollectible)
                {
                    MusicPlayer.instance.AddLowPassFilters(1.5f, 400);
                    monSpecificLevelManager.collectibleCollected += 1;
                    if(playSound) soundRelay.PlayAudioClip(0);
                    UIManager.instance.EnableDecalPanel();
                    isCollectible = false;
                }
            }
            //else
                //Debug.Log("is not visible");
        }
        decrementIsPossible = false; 
    }

    public void OnTriggerExit(Collider other)
    {
        if (other)
        {
            decrementIsPossible = true;          
        }
    }

    private IEnumerator IncrementationAlpha(float fadeSpd) {
        while (text.alpha <= 1) {
            text.alpha += fadeSpd;
            yield return new WaitForSeconds(0.005f);
        }
        yield return new WaitForSeconds(0f);
    }

    private IEnumerator DecrementationAlpha(float fadeSpd)
    {
        while (text.alpha >= 0) {
            text.alpha -= fadeSpd;
            yield return new WaitForSeconds(0.07f);
        }
        yield return new WaitForSeconds(0f);
    }

    private int DistanceDeDisparition () {
        switch (dd){
            case DD.close:
                distanceFixe = 8;
                break;

            case DD.medium:
                distanceFixe = 13;
                break;           

            case DD.far:
                distanceFixe = 20;
                break;
        }
        return distanceFixe;
    }
}
    //private IEnumerator DefilementMot() //fonction pour faire apparaitre les lettres une a une (fonctionne)
    //{
    //    for (int i = 0; i < totalVisibleCharacter + 1; i++)
    //    {
    //        monTexte.maxVisibleCharacters = i;
    //        yield return new WaitForSeconds(0.1f);
    //    }

       
    //    yield return new WaitForSeconds(0f);
    //}

//while (!cestFini)
//{
//    visibleCount = counter % (totalVisibleCharacter + 1);
//    monTexte.maxVisibleCharacters = visibleCount;
//    counter += 1;
//    yield return new WaitForSeconds(5f);

//    if (counter >= totalVisibleCharacter)
//    {
//        Debug.Log("allo");
//        yield return new WaitForSeconds(1f);
//        cestFini = true;
//    }
//}        


//CHLOE=================EN PARAMÈTRE======================================================================

/*Script cree le : 06-05-2019
 * Par: Chloe Rocheleau
 * Derniere modification: 06-05-2019
 * Par: Chloe Rocheleau
 * Description: Apparition de decals indiquant les pensees de Blank en UI dans l'environnement. 
 */


////// POUR LA DISTANCE DU DECAL PAR RAPPORT A L'AVATAR
////private int distanceFar = 20;
////private int distanceMedium = 10;
////private int distanceClose = 5 ; // Distance de l'avatar (ou de la cam?) par rapport au canvas. 
////private Vector3 positionAvatar;
////bool fading;

/////*public enum distanceApparition { Far, Medium, Close};
////[Header("Distance d'Apparition de la pensee")]
////public distanceApparition apparitionDistance;*/

/////*public enum tailleDuTexte { Small, Medium, Large };
////[Header("Taille de la police de Caractère")]
////public tailleDuTexte scaleText;*/

////    // ***Marche pas*******************************************
////public enum vitesseApparition { Fast, Normal, Slow };
////[Header("Vitesse d'Apparition de la pensee")]
////public vitesseApparition apparitionSpeed;
//=============================================================================================


//CHLOE====================================================================================================
/* // Update is called once per frame

void Update() 
{
    positionAvatar = GameManager.instance.currentAvatar.tr.position;

    // FADE IN FADE OUT DU TEXTE
    if (fading == true)
    {
        //Fully fade in Image (1) with the duration of 2
        monTexte.CrossFadeAlpha(1, 5.0f, false);
    }
    //If the toggle is false, fade out to nothing (0) the Image with a duration of 2
    if (fading == false)
    {
        monTexte.CrossFadeAlpha(0, 5.0f, false);
    }

    // Calcul distance Blank 

    float distance = Vector3.Distance(tr.position, positionAvatar);


}

private void OnBecameVisible()
{
    //Debug.Log("On voit le texte (OnBecame)");
    SetDecal();
    isVisible = true;
    fading = true;
}
private void OnBecameInvisible()
{
    //Debug.Log("Texte Invisible (OnBecame)");

    isVisible = false;
    fading = false;
}

// Switch case pour la distance de l'avatar *********************************** MARCHE PAS *******************************************
private void SetDecal (){
    switch (apparitionSpeed)
    {
        case vitesseApparition.Fast:
            monTexte.CrossFadeAlpha(0, 0.5f, false);
            break;
        case vitesseApparition.Normal:
            monTexte.CrossFadeAlpha(0, 50, false);
            break;
        case vitesseApparition.Slow:
            monTexte.CrossFadeAlpha(0, 300, false);
            break;

    }
}
}
// *********** TO DO************* 
*/

