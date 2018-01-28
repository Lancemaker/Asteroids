using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Asteroid : Entity {
    //delegates & events definition
    public delegate void AsteroidEventHandler(Type type ,Vector3 pos);
    public static event AsteroidEventHandler OnAsteroidDestroyed;

    //variables declaration
    public enum Type {big,med,small}
    public Type type = Type.big;    
    
    
    // Use this for initialization
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag == "PlayerBullet")
        {
            //OnAsteroidDestroyedTrigger;
            if (OnAsteroidDestroyed != null)
            {
                OnAsteroidDestroyed(type, transform.position);
            }            
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (type==Type.big)
        {
            SetInitialPos();
        }
        SetRotAndDir();       
    }

    

    void Start () {        
        ScreenBounds();
        gameObject.layer = 10;        
        //picking a random Sprite depending on the type.
        string spriteName = SpriteFilter(type.ToString(), Atlas)[Random.Range(0, 3)].name.Replace("(Clone)",string.Empty);
        Debug.Log(spriteName);
        ERenderer.sprite = Atlas.GetSprite(spriteName);
        //Setting colliders and initial position
        SetInitialPos();
        ECollider = gameObject.AddComponent<PolygonCollider2D>();
        ESize = new Vector2(ECollider.bounds.size.y, ECollider.bounds.size.x);
        gameObject.tag = "asteroid";        
    }

    void SetRotAndDir() {
        RB.AddTorque(Random.Range(0, 10));
        RB.AddForce( new Vector2(Random.Range(-90, 90), Random.Range(-90, 90)));
    }  

    void SetInitialPos()
    {
        //random position and trying to avoid to put the asteroid on top of the ship.
        if(type == Type.big)
        {
            Vector2 pos = new Vector2(Random.Range(camMin.x-5, camMin.x), Random.Range(camMin.y-5, camMin.y));
            RB.position = pos;
        }      
    }

    public void SetPos(Vector3 position)
    {
        RB.position = position;
    }
    


    // Update is called once per frame
    private void FixedUpdate()
    {
        MirrorPosition(ESize);
    }
    void Update () {
		
	}
}
