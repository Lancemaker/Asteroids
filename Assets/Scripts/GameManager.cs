using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour {               
    GameObject player, asteroidContainer;
    public SpriteAtlas atlas;
    PlayerShip playerShip;
    List<GameObject> Enemies = new List<GameObject>();
    bool gameOver = false;
    int lives = 3;
    int points = 0;
    int enemiesInc, activeEnemies;
    //start
    void Start () {        
        InitPlayer();
        EnemiesPool();
        StartCoroutine(Waves());        
    }

    //initialize the game.
    void InitPlayer() {
        //setting the player
        player = new GameObject("Player");             
        playerShip = player.AddComponent<PlayerShip>();
        playerShip.Atlas = atlas;
        playerShip.Inputs = gameObject.GetComponent<Inputs>();         
    }
    void InitAsteroid(Asteroid.Type type) {        
        GameObject asteroid = new GameObject("asteroid");
        asteroid.transform.parent = asteroidContainer.transform;
        Asteroid asteroidScript = asteroid.AddComponent<Asteroid>();
        asteroidScript.Atlas = atlas;
        asteroidScript.type = type;
        Enemies.Add(asteroid);
        asteroid.SetActive(false);
    }
    //to 0-19 big, up to 39 medium, up to 79 small asteroids;
    void EnemiesPool()
    {
        asteroidContainer = new GameObject("Astedois");
        for (int i = 0; i < 140; i++)
        {
            if (i < 19)
            {
                InitAsteroid(Asteroid.Type.big);
            }
            else if(i < 39)
            {
                InitAsteroid(Asteroid.Type.medium);
            }
            else if(i < 80)
            {
                InitAsteroid(Asteroid.Type.small);
            }
        }
    }
        
    IEnumerator Waves() {        
        activeEnemies = 5;
        enemiesInc = 5;
        while (!gameOver)
        {
            for (int i = 0; i < enemiesInc; i++)
            {
                Enemies[i].SetActive(true);
            }
         yield return new WaitUntil(() => activeEnemies == 0);
            if(enemiesInc < 20)
            {
                enemiesInc++;
            }            
        }
        Debug.Log("Game Over");
        yield return null;
    }
}
