using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Asteroid : Entity {
    //delegates & events declaration
    public delegate void AsteroidEventHandler(Type type ,Vector3 pos);
    public static event AsteroidEventHandler OnAsteroidDestroyed;

    //variables declaration
    public enum Type {big,med,small}
    public Type type = Type.big;    
    public int difInc=5;
    
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
        SetRotAndDir(difInc);       
    }
    private void OnDisable()
    {
        GameManager.OnDifficultyChange -= SetDifficulty;
    }


    void Start () {        
        ScreenBounds();
        gameObject.layer = 10;        
        //picking a random Sprite depending on the type.
        string spriteName = SpriteFilter(type.ToString(), Atlas)[Random.Range(0, 3)].name.Replace("(Clone)",string.Empty);
        ERenderer.sprite = Atlas.GetSprite(spriteName);
        //Setting colliders and initial position        
        ECollider = gameObject.AddComponent<PolygonCollider2D>();
        ESize = new Vector2(ECollider.bounds.size.y, ECollider.bounds.size.x);
        gameObject.tag = "asteroid";
        SetInitialPos();
        //events subscription
        GameManager.OnDifficultyChange += SetDifficulty;

    }
    
    void SetDifficulty(int d)
    {
        difInc = d;
    }


    void SetRotAndDir(int d) {        
        RB.AddTorque(Random.Range(0, 10));
        RB.AddForce( new Vector2(Random.Range(-20, 20)*d, Random.Range(-20, 20)*d));        
    }  
    //!!!create a coroutine to push the objects in the right time.
    void SetInitialPos()
    {
        //random position and trying to avoid putting the asteroid on top of the ship.
        if(type == Type.big)
        {
            Vector2 pos = new Vector2(Random.Range(camMin.x-5, camMin.x), Random.Range(camMin.y-5, camMin.y));
            RB.position = pos;
        }      
    }
    IEnumerator WaitAndMove()
    {
        yield return new WaitForFixedUpdate();        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MirrorPosition(ESize);
    }
    void Update () {
		

	}
}
