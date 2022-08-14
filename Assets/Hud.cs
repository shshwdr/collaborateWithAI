using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    int score = 0;
    public Text scoreText;
    public Button pauseButton;

    public GameObject scoreAnim;
    // Start is called before the first frame update
    void Awake()
    {
        EventPool.OptIn<string>("finishOrder", finishOrder);
        pauseButton.onClick.AddListener(delegate
        {

            GameObject.FindObjectOfType<Popup>(true).Init("Resume");

            GameObject.FindObjectOfType<Popup>(true).showView();
        });
    }

    void finishOrder(string dishName)
    {
        score += 10;
        scoreText.text = score.ToString();

        var go = Instantiate(scoreAnim, scoreText.transform.position, Quaternion.identity, scoreText.transform.parent);
        Destroy(go, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
