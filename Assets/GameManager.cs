using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int maxMissedOrder = 4;
    public int missedOrder = 0;
    public bool isGameOver = false;
    public void missAnOrder()
    {
        missedOrder++;
        Debug.Log("miss one order " + missedOrder);
        EventPool.Trigger("missOrderChanged");

        if (maxMissedOrder <= missedOrder && !isGameOver)
        {
            isGameOver = true;
            SFXManager.Instance.playgameover();
            Invoke("showGameOver", 1);
        }
    }

    void showGameOver()
    {
        var gameoverString = "Game Over\n";
        gameoverString += $"You served {OrderManager.Instance.finishedOrder} dishes and earned {OrderManager.Instance.finishedOrder * 10}. \nBut you wasted {OrderManager.Instance.throwedToGarbage} food.\n";
        gameoverString += "Hope you enjoyed it and good luck next time!";

        GameObject.FindObjectOfType<Popup>(true).Init(gameoverString, () => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            restartGame();

        }, "Restart");

        GameObject.FindObjectOfType<Popup>(true).showView();
    }

    void restartGame()
    {

       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        missedOrder = 0;
        isGameOver = false;
        IngredientManager.Instance.initBoard();
        OrderManager.Instance.init();

        TutorialManager.Instance.showTutorial("first_0");
    }

    // Start is called before the first frame update
    void Start()
    {
        restartGame();

        //GameObject.FindObjectOfType<Popup>(true).Init("test", () =>
        //{
        //});

        //GameObject.FindObjectOfType<Popup>(true).showView();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            restartGame();
        }
    }
}
