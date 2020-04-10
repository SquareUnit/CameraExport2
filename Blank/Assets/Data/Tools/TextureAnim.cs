using UnityEngine;
using System.Collections;

public class TextureAnim : MonoBehaviour {
	public Sprite[] materiauxAAnimer;
	public float vitesseAnimation;
	private int nbMat;
	private int index;
    private SpriteRenderer srd;
	// Use this for initialization
	void Start () {
		nbMat = materiauxAAnimer.Length; // sera une constante, alors on la calcule qu'une fois
        srd = this.GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void FixedUpdate () {
		index = (int)(Time.time*vitesseAnimation);
		
		index %=nbMat; //on veut pas depasser les limites du tableau
		
		srd.sprite = materiauxAAnimer[index];
		
	}
}
