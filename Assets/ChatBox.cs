using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    public float textStayTime = 2f;
    float textTimer = 0;
    public Text text;
    bool alwaysShow;
    // Start is called before the first frame update
    void Start()
    {

        gameObject.SetActive(false);
    }

    public void show(string t, bool alwaysShow = false)
    {
        this.alwaysShow = alwaysShow;
        text.text = t;
        textTimer = textStayTime;
        gameObject.SetActive(true);
    }

    public void hide()
    {

        gameObject.SetActive(false);
        textTimer = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (alwaysShow)
        {
            gameObject.SetActive(true);
        }
        if (textTimer > 0)
        {
            textTimer -= Time.deltaTime;
            if (textTimer <= 0)
            {
                text.text = "";
                gameObject.SetActive(false);
            }
        }
    }
}
