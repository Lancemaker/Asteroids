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
    private void Awake()
    {
        ERenderer = gameObject.AddComponent<SpriteRenderer>();
        RB = gameObject.AddComponent<Rigidbody2D>();
    }
    
    //to search(s String) and return a filtered list(sprites)
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
}