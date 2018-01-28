using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Entity : MonoBehaviour {
    SpriteAtlas atlas;
    public SpriteAtlas Atlas
    {
        get
        {
            return atlas;
        }

        set
        {
            atlas = value;
        }
    }
    Collider2D eCollider;    
    SpriteRenderer eRenderer;
    Rigidbody2D rB;
    Vector3 eSize;
    public Camera cam;
    public Vector3 camMax;
    public Vector3 camMin;
    protected Collider2D ECollider
    {
        get
        {
            return eCollider;
        }

        set
        {
            eCollider = value;
        }
    }
    protected SpriteRenderer ERenderer
    {
        get
        {
            return eRenderer;
        }

        set
        {
            eRenderer = value;
        }
    }
    protected Rigidbody2D RB
    {
        get
        {
            return rB;
        }

        set
        {
            rB = value;
        }
    }
    protected Vector3 ESize
    {
        get
        {
            return eSize;
        }

        set
        {
            eSize = value;
        }
    }

    private void Awake()
    {
        ERenderer = gameObject.AddComponent<SpriteRenderer>();
        RB = gameObject.AddComponent<Rigidbody2D>();
        cam = Camera.main;
    }
    
    //to search "s", in the SpriteAtlas a and return a filtered list<sprites>
    protected List<Sprite> SpriteFilter(string s,SpriteAtlas a)
    {
        Sprite[] spriteArr = new Sprite[a.spriteCount];
        List<Sprite> returnedSpriteList = new List<Sprite>();
        a.GetSprites(spriteArr);
        foreach (Sprite item in spriteArr)
        {
            if (item.name.Contains(s))
            {                
                returnedSpriteList.Add(item);
            }
        }        
        return returnedSpriteList;
    }    
    protected void ScreenBounds()
    {
        camMax = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, 0f));
        camMin = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
    }
    //this method makes the object appears in the other side of the screen.
    protected void MirrorPosition(Vector2 offset) {
        ScreenBounds();
        if (transform.position.x - offset.x> camMax.x)            
        {
            RB.MovePosition(new Vector2(camMin.x, transform.position.y));
        }
        if (transform.position.x + offset.x < camMin.x)
        {
            RB.MovePosition(new Vector2(camMax.x, transform.position.y));
        }
        if (transform.position.y -offset.y > camMax.y)
        {
            RB.MovePosition(new Vector2(transform.position.x,camMin.y ));
        }
        if (transform.position.y +offset.y < camMin.y)
        {
            RB.MovePosition(new Vector2(transform.position.x,camMax.y ));
        }
    }

}