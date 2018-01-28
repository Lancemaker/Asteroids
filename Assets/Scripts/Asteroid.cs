using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Asteroid : Entity {
    //delegates & events definition
    public delegate void AsteroidEventHandler(Type type ,Vector3 pos);
    public static event AsteroidEventHandler OnAsteroidDestroyed;

    //variables declaration
    public enum Type {big,medium,small}
    public Type type = Type.big;    
    
    
    // Use this for initialization
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision");
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
        switch (type)
        {
            case Type.big:                
                SetRotAndDir();
                break;
            case Type.medium:
                
                SetRotAndDir();
                break;
            case Type.small:
               
                SetRotAndDir();
                break; 
            default:
                break;
        }
    }

    private void OnDisable()
    {
        
    }

    void Start () {        
        ScreenBounds();
        gameObject.layer = 10;        
        //picking a random Sprite depending on the type.
        string spriteName = SpriteFilter(type.ToString(), Atlas)[Random.Range(0, 3)].name.Replace("(Clone)",string.Empty);
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
            Vector2 pos = new Vector2(Random.Range(camMin.x, camMax.x - camMax.x / 4), Random.Range(camMin.y, camMax.y - camMax.y / 4));
            if (pos.x >= -camMax.x /4 && pos.x < camMax.x/4)
            {
                pos.x += camMax.x/2 ;                
            }
            if (pos.y >= -camMax.y /4 && pos.y < camMax.y /4)
            {
                pos.y += camMax.y ;                
            }
            RB.position = pos;
        }
        
    }

    


    // Update is called once per frame
    private void FixedUpdate()
    {
        MirrorPosition(ESize);
    }
    void Update () {
		
	}
}
