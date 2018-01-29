using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Background : MonoBehaviour {
    Camera cam;
    Vector3 camMin,camMax;
    public SpriteAtlas atlas;
	// Use this for initialization
	void Start () {                
        StartCoroutine(RollBG());
	}
   
    IEnumerator RollBG()
    {
        float incX = 0;
        float incY=0;
        while (true){
            incX += 0.01f;
            incY += 0.01f;
            gameObject.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(incX, incY));
            yield return new WaitForSeconds(0.5f);

        }
        
    }
    
}
