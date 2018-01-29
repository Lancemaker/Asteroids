using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesAndPointsUI : MonoBehaviour {
    public Text scoreText,livesText;
    private void Start()
    {
        
    }
    public void RenderScore(int s)
    {
        scoreText.text = s.ToString();
    }
    public void RenderLives(int l)
    {
        livesText.text = l.ToString();
    }
}
