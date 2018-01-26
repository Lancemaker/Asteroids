using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour {
    // Use this for initialization    
    Camera cam;
    public float screenWidth;
    public float screenHeight;
    GameObject player;
    public SpriteAtlas atlas;
    PlayerShip playerShip;
    List<Asteroid> asteroids = new List<Asteroid>();
    void Start () {
        cam =Camera.main;
        InitPlayer();
    }

    //initialize the game.
    void InitPlayer() {
        //setting the player
        player = new GameObject();
        player.name = "Player";        
        playerShip = player.AddComponent<PlayerShip>();
        playerShip.Atlas = atlas;
        playerShip.Inputs = gameObject.GetComponent<Inputs>();        
    }

    void ScreenBounds() {
        screenWidth = cam.pixelWidth;
        screenHeight = cam.pixelHeight;
    }
	
	// Update is called once per frame
	void Update () {
        ScreenBounds();
	}
}
