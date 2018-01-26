using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public bool shoot;
    void Update()
    {
    #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WEBGL
        //implementação standalone/web
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        shoot = Input.GetKeyDown(KeyCode.Space);
    #elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        //para implementar inputs mobile.
    #endif
    }
}
