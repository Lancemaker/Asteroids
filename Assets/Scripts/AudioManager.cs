using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static bool AudioBegin = false;
    public AudioSource explosionShip;
    public AudioSource explosionAsteroid;
    void Awake()
    {
        if (!AudioBegin)
        {
            GetComponent<AudioSource>().Play();
            DontDestroyOnLoad(gameObject);
            AudioBegin = true;
        }
    }
    private void Start()
    {
        //events subscription
        PlayerShip.OnShipHit += PlayShipExplosion;
        Asteroid.OnAsteroidDestroyed += PlayAsteroidExplosion;
    }

    private void OnDisable()
    {
        //events unsubscription
        PlayerShip.OnShipHit -= PlayShipExplosion;
        Asteroid.OnAsteroidDestroyed -= PlayAsteroidExplosion;
    }

    void PlayShipExplosion(bool i)
    {
        explosionShip.Play();
    }
    void PlayAsteroidExplosion(Asteroid.Type type, Vector3 pos)
    {
        explosionAsteroid.Play();
    }
}
