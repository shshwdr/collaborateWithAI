using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    int score = 0;
    public Text scoreText;
    // Start is called before the first frame update
    void Awake()
    {
        EventPool.OptIn<string>("finishOrder", finishOrder);
    }

    void finishOrder(string dishName)
    {
        score += 10;
        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
