using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {
    //delegates & events declaration
    public delegate void GameManagerDifficultyEventHandler(int difficultyLevel);
    public static event GameManagerDifficultyEventHandler OnDifficultyChange;
    //variables
    GameObject player, asteroidContainer;
    public GameObject mainMenu,gameOverMenu,LivesAndPoints;    
    public SpriteAtlas atlas;
    PlayerShip playerShip;
    public List<GameObject> Enemies = new List<GameObject>();
    bool gameOver = true;
    int lives = 3;
    int score = 0;
    int ScoreGoalToLife = 10000;
    int enemiesInc,  EnemyStartingWave;
    public int activeEnemies;
    Inputs inputs;    
    //start
    void Start () {
        inputs = gameObject.GetComponent<Inputs>();
        //events subscription
        Asteroid.OnAsteroidDestroyed += AsteroidDestroyedReact;
        PlayerShip.OnShipHit += ShipHitReact;
    }
    private void OnDisable()
    {       
        //events unsubscription
        Asteroid.OnAsteroidDestroyed -= AsteroidDestroyedReact;
        PlayerShip.OnShipHit -= ShipHitReact;
    }

    public void StartGame() {
        if(gameOver == true)
        {            
            gameOver = false;
            InitPlayer();
            EnemiesPool();
            StartCoroutine(Waves());
            LivesAndPoints.SetActive(true);
        }
        if (Time.timeScale == 0)
        {
            Unpause();
        }
    }

    void AsteroidDestroyedReact(Asteroid.Type type, Vector3 pos)
    {        
        activeEnemies--;
        //if its not a small asteroid enable 2 smaller asteroids from the pool.
        if (type != Asteroid.Type.small)
        {
            enableNextSubAsteroidInPool(type,pos);            
        }
        SetPoints(type);
    }

    void ShipHitReact(bool b)
    {
        if (lives<=0)
        {
            gameOver = true;
            player.SetActive(false);
            LivesAndPoints.SetActive(false);
            gameOverMenu.SetActive(true);
            gameOverMenu.GetComponent<Highscores>().SetScore(score);
        }
        else
        {
            lives--;
            RefreshUI();
            PlayerRevive();
        }
    }

    void PlayerRevive()
    {
        StartCoroutine(playerShip.RessurectionAnim());
    }

    void SetPoints(Asteroid.Type type)
    {
        switch (type)
        {
            case Asteroid.Type.big:
                score += 200;
                break;
            case Asteroid.Type.med:
                score += 250;
                break;
            case Asteroid.Type.small:
                score += 300;
                break;
            default:
                break;
        }
        if (score>=ScoreGoalToLife)
        {            
            lives++;
            ScoreGoalToLife += 10000;
        }
        RefreshUI();
    }

    void RefreshUI()
    {
        LivesAndPointsUI lp = LivesAndPoints.GetComponent<LivesAndPointsUI>();
        lp.RenderScore(score);
        lp.RenderLives(lives);
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
                        Enemies[i].transform.position = pos;
                        Enemies[i].SetActive(true);                       
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
                        Enemies[i].transform.position = pos;
                        Enemies[i].SetActive(true);                       
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
        playerShip.Inputs = inputs;         
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
                //event Trigger OnDifficultyChange
                if(OnDifficultyChange != null)
                {
                    OnDifficultyChange(EnemyStartingWave);
                }
            }

        }
        Debug.Log("Game Over");
        yield return null;
    }

    void Pause(){
        Time.timeScale = 0;
    }
    void Unpause() {
        Time.timeScale = 1;
    } 


    void SwitchMenuPause()
    {
        if (inputs.menu)
        {
            if (mainMenu.activeSelf==false && !gameOver)
            {
                mainMenu.SetActive(true);
                Pause();
            }
            else if(!gameOver)
            {
                mainMenu.SetActive(false);
                Unpause();
            }
        }
    }

    public void Reload()
    {
        SceneManager.LoadScene(0);
    }
    private void Update()
    {
        SwitchMenuPause();
    }
}
