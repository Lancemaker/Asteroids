using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour {
    // Use this for initialization           
    GameObject player;
    public SpriteAtlas atlas;
    PlayerShip playerShip;
    void Start () {        
        InitPlayer();
        InitAsteroid();
    }

    //initialize the game.
    void InitPlayer() {
        //setting the player
        player = new GameObject("Player");             
        playerShip = player.AddComponent<PlayerShip>();
        playerShip.Atlas = atlas;
        playerShip.Inputs = gameObject.GetComponent<Inputs>();        
    }
    void InitAsteroid() {
        GameObject asteroid = new GameObject("asteroid");
        Asteroid asteroidScript = asteroid.AddComponent<Asteroid>();
        asteroidScript.Atlas = atlas;
    }
    

    
	// Update is called once per frame
	void Update () {
        
	}
}
