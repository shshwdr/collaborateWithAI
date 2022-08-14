using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        IngredientManager.Instance.initBoard();
        OrderManager.Instance.init();


        GameObject.FindObjectOfType<Popup>(true).Init("test", () =>
        {
        });

        GameObject.FindObjectOfType<Popup>(true).showView();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
