using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour {               
    GameObject player, asteroidContainer;
    public SpriteAtlas atlas;
    PlayerShip playerShip;
    public List<GameObject> Enemies = new List<GameObject>();
    bool gameOver = false;
    int lives = 3;
    int points = 0;
    int enemiesInc,  EnemyStartingWave;
    public int activeEnemies;


    //start
    void Start () {        
        InitPlayer();
        EnemiesPool();
        StartCoroutine(Waves());
        //events subscription
        Asteroid.OnAsteroidDestroyed += AsteroidDestroyedReact;
    }
    private void OnDisable()
    {
        //events unsubscription
        Asteroid.OnAsteroidDestroyed -= AsteroidDestroyedReact;
    }

    void AsteroidDestroyedReact(Asteroid.Type type, Vector3 pos)
    {
        activeEnemies--;
        //if its not a small asteroid enable 2 smaller asteroids from the pool.
        if (type != Asteroid.Type.small)
        {
            enableNextSubAsteroidInPool(type,pos);            
        }
    }

    void enableNextSubAsteroidInPool(Asteroid.Type type,Vector3 pos)
    {
        //since the destruction of a asteroid spawns 2 smaller ones(except for the small type).
        int enabledCount = 0;
        switch (type)
        {
            case Asteroid.Type.big:
                for (int i = 20; i < 60; i++)
                {                    
                    if (Enemies[i].activeSelf == false)
                    {
                        Enemies[i].SetActive(true);
                        Enemies[i].GetComponent<Asteroid>().SetPos(pos);
                        enabledCount++;
                        activeEnemies++;
                    }
                    if(enabledCount >= 2)
                    {
                        break;
                    }
                }
                break;
            case Asteroid.Type.med:
                for (int i = 60; i < 140; i++)
                {
                    if (Enemies[i].activeSelf == false)
                    {
                        Enemies[i].SetActive(true);
                        Enemies[i].GetComponent<Asteroid>().SetPos(pos);
                        enabledCount++;
                        activeEnemies++;
                    }
                    if (enabledCount >= 2)
                    {
                        break;
                    }
                }
                break;            
            default:
                break;
        }
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
    //to 0-19 big, up to 59 medium, up to 139 small asteroids;
    void EnemiesPool(){
        asteroidContainer = new GameObject("Astedois");
        for (int i = 0; i < 140; i++)
        {
            if (i < 20)
            {
                InitAsteroid(Asteroid.Type.big);
            }
            else if(i < 60)
            {
                InitAsteroid(Asteroid.Type.med);
            }
            else if(i < 140)
            {
                InitAsteroid(Asteroid.Type.small);
            }
        }
    }
        
    IEnumerator Waves() {
        EnemyStartingWave = 5;
        activeEnemies = 0;
        enemiesInc = 5;
        while (!gameOver)
        {
            for (int i = 0; i < EnemyStartingWave; i++)
            {
                Enemies[i].SetActive(true);
                activeEnemies++;
            }
         yield return new WaitUntil(() => activeEnemies == 0);
            if(enemiesInc < 20)
            {
                enemiesInc++;
                EnemyStartingWave = enemiesInc;
            }            
        }
        Debug.Log("Game Over");
        yield return null;
    }
}
