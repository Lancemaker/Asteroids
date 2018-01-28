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
        cam =Camera.main;
        ScreenBounds();
        FillBG();	
	}
    void ScreenBounds()
    {
        camMax = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, 0f));
        camMin = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
        
    }
    void FillBG()
    {
        gameObject.GetComponent<Renderer>().material.mainTexture=atlas.GetSprite("black").texture;
    }
    
}
