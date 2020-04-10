using UnityEngine;
using System.Collections;

public class UVScrolling : MonoBehaviour {
    [Range(-2f,2f)]
    public float speedX = 1f;
    [Range(-2f, 2f)]
    public float speedY = 1f;
    private Vector2 scrollSpeed = new Vector2(10f,10f);
	
    private Renderer rd;
	
	//contient le decalage voulu
	private Vector2 uvOffset = Vector2.zero;

    private void Start()
    {
        rd = GetComponent<Renderer>();
    }
    void LateUpdate () {
        scrollSpeed.x = speedX;
        scrollSpeed.y = speedY;
	
	 	uvOffset += (scrollSpeed*Time.deltaTime);
		
		rd.material.SetTextureOffset ("_MainTex", uvOffset);
		
		
	}
}
