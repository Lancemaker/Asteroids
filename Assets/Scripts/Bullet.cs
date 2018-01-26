using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Bullet : Entity {
    Sprite[] sprites;
    float lifetime = 0.5f;

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
    }

    public IEnumerator Animate() {
        int increment = 0;       
        while (true)
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
        gameObject.SetActive(false);
    }
    //positions, rotate and offsets the beam .
    public void SetBullet(Vector3 pos,float rot,float offset)
    {
        float TargetX = Mathf.Cos((rot+90)*Mathf.PI/180)*offset+pos.x;
        float targetY = Mathf.Sin((rot+90)*Mathf.PI/180)*offset+pos.y;
        RB.MovePosition(new Vector2(TargetX, targetY));
        RB.MoveRotation(rot);       
    }
    private void FixedUpdate()
    {
        RB.AddForce(transform.up * 70);
    }
}
