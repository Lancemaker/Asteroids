﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerShip : Entity {    
    Inputs inputs;
    int currentBullet=0;
    float Shipsize;
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
    int speedIncrement = 15;
    public float torque = 0.6f;
    List<GameObject> bullets= new List<GameObject>();
    // Use this for initialization
    void Start () {       
        ERenderer.sprite = Atlas.GetSprite("playerShip1_blue");
        ECollider = gameObject.AddComponent<PolygonCollider2D>();
        Shipsize = ECollider.bounds.size.y;
        RB.drag = 1.5f;
        RB.angularDrag = 1f;
        gameObject.layer = 8;
        PopulateBullets();        
	}

    void Move() {        
        if (Inputs.vertical > 0)
        {            
            RB.AddForce(transform.up * speedIncrement);
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

    void Shoot() {     

        if (Inputs.shoot && currentBullet < bullets.Count && !bullets[currentBullet].activeSelf)
        {
            //Debug.Log("shoot");
            bullets[currentBullet].SetActive(true);
            Bullet bulletscript = bullets[currentBullet].GetComponent<Bullet>();
            bulletscript.SetBullet(gameObject.transform.position,RB.rotation,Shipsize);
            StartCoroutine(bulletscript.Animate());
            currentBullet++;
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
            bullets[i].GetComponent<Bullet>().SetBullet(gameObject.transform.position, RB.rotation, Shipsize);
            bullets[i].SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        Move();
        
    }
    void Update () {
        Shoot();
    }
}