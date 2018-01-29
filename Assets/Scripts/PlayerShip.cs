using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerShip : Entity {
    Inputs inputs;
    int currentBullet = 0;
    int numberOfBullets = 20;
    public Inputs Inputs
    {
        get
        {
            return inputs;
        }

        set
        {
            inputs = value;
        }
    }
    int speedIncrement = 6;
    float torque = 0.5f;
    GameObject turbinePrefab;
    GameObject turbine;
    AudioSource pewpew;    
    AudioSource turbineAudio;
    List<GameObject> bullets = new List<GameObject>();

    //delegates & events definition
    public delegate void ShipEventHandler(bool col);
    public static event ShipEventHandler OnShipHit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision");
        if (collision.collider.gameObject.tag == "asteroid")
        {
            //OnshipHit Event Trigger
            if(OnShipHit != null)
            {
                OnShipHit(true);
                
            }            
            gameObject.SetActive(false);
        }
    }
    

    // Use this for initialization
    void Start () {       
        ERenderer.sprite = Atlas.GetSprite("playerShip1_blue");
        ECollider = gameObject.AddComponent<PolygonCollider2D>();
        ESize = new Vector2(ECollider.bounds.size.y,ECollider.bounds.size.x);
        RB.drag = 0.5f;
        RB.angularDrag = 1.5f;
        gameObject.layer = 8;
        PopulateBullets();
        //particles
        turbinePrefab = (GameObject)Resources.Load("prefabs/TurbineParticle", typeof(GameObject));
        turbine = Instantiate(turbinePrefab,gameObject.transform);
        turbine.transform.position = new Vector3(0,-0.35f,0);
        //audio setup
        pewpew = gameObject.AddComponent<AudioSource>();
        pewpew.playOnAwake = false;
        pewpew.clip = (AudioClip)Resources.Load("audio/laser", typeof(AudioClip));
        turbineAudio = gameObject.AddComponent < AudioSource>();
        turbineAudio.playOnAwake = false;
        turbineAudio.clip = (AudioClip)Resources.Load("audio/rocketTrusters",typeof(AudioClip));
    }

    void Move() {        
        if (Inputs.vertical > 0)
        {            
            RB.AddForce(transform.up * speedIncrement);
            PlayTurbineSound();
        }
        else
        {
            turbineAudio.Stop();
        }
        if (Inputs.horizontal > 0 )
        {
            RB.AddTorque(-torque);
            
        }
        else if(Inputs.horizontal <0)
        {
            RB.AddTorque(+torque);
            
        }
        
    }

    void PlayTurbineSound()
    {
        if (!turbineAudio.isPlaying)
        {
            turbineAudio.Play();
        }
    }

    void Shoot() {     

        if (Inputs.shoot && currentBullet < bullets.Count && !bullets[currentBullet].activeSelf)
        {
            //Debug.Log("shoot");
            bullets[currentBullet].SetActive(true);
            Bullet bulletscript = bullets[currentBullet].GetComponent<Bullet>();            
            bulletscript.SetBullet(gameObject.transform.position,RB.rotation,ESize.y/2);            
            currentBullet++;
            pewpew.Play();
        }
        else if (currentBullet == bullets.Count)
        {
            currentBullet = 0;
        }        
    }

    //fills the bullet pool.
    void PopulateBullets() {
        GameObject container = new GameObject();
        container.name = "playerProjectiles";
        List<Sprite> laserSprites = SpriteFilter("laserBlue", Atlas);
        
        for (int i=0; i < numberOfBullets; i++)
        {
            //Debug.Log(bullets[i]);            
            bullets.Add( new GameObject("beam"));            
            bullets[i].transform.SetParent(container.transform);
            bullets[i].layer = 9;
            bullets[i].tag ="PlayerBullet";
            Bullet bulletScript = bullets[i].AddComponent<Bullet>();
            bulletScript.Setsprites(laserSprites);
            //bullets[i].GetComponent<Bullet>().SetBullet(gameObject.transform.position, RB.rotation, ESize.y/2);
            bullets[i].SetActive(false);
        }
    } 

    private void FixedUpdate()
    {
        Move();
        MirrorPosition(ESize);
    }
    void Update () {
        Shoot();        
    }
}
