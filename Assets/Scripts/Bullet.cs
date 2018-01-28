using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Bullet : Entity {
    Sprite[] sprites;
    float lifetime = 0.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision");
        if (collision.collider.gameObject.tag == "asteroid")
        {
            gameObject.SetActive(false);
        }
    }


    private void OnEnable()
    {        
        StartCoroutine(LifeClock());
        
    }

    private void OnDisable()
    {
        
    }
    private void Start()
    {
        transform.localScale =new Vector3(0.5F, 0.5f, 0.5f);        
    }

    public void Setsprites(List<Sprite> Spritelist) {
        sprites = Spritelist.ToArray();
        ERenderer.sprite = sprites[0];
        ECollider = gameObject.AddComponent<PolygonCollider2D>();        
        ESize = new Vector2(ECollider.bounds.size.y, ECollider.bounds.size.x);
        ERenderer.enabled = false;
    }
    
   IEnumerator Animate() {
        int increment = 0;
        yield return new WaitForSeconds(0.1f);
        ERenderer.enabled = true;
        //while moving : animate.
        while (RB.velocity.x+RB.velocity.y!=0)
        {
            yield return new WaitForSeconds(0.005f);
            if (increment< sprites.Length)
            {
                ERenderer.sprite = sprites[increment];
                increment++;                
            }
            else
            {
                increment = 0;
            }
        }
        
    }
    IEnumerator LifeClock() {
        yield return new WaitForSeconds(lifetime);        
        ERenderer.enabled = false;
        ECollider.enabled = false;
        gameObject.SetActive(false);
        RB.MovePosition(new Vector2(100, 100));
    }

    //positions, rotate and offsets the beam .
    public void SetBullet(Vector3 pos,float rot,float offset)
    {
        float targetX = Mathf.Cos((rot+90)*Mathf.PI/180)*offset+pos.x;
        float targetY = Mathf.Sin((rot+90)*Mathf.PI/180)*offset+pos.y;        
        RB.MovePosition(new Vector2(targetX, targetY));
        RB.MoveRotation(rot);
        ECollider.enabled = true;
        StartCoroutine(Animate());
    }

    private void FixedUpdate()
    {
        RB.AddForce(transform.up * 70);
        MirrorPosition(ESize);
    }
}
