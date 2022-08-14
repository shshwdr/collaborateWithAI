using Doozy.Engine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : BaseView
{

    public Text text;
    public Button yesButton;

    public Button noButton;
    public override void showView()
    {
        base.showView();
        GetComponent<UIView>().Show();
    }

    public override void hideView()
    {
        base.hideView();

        GetComponent<UIView>().Hide();
        //panel.SetActive(false);
    }

    void clearButton()
    {

        yesButton.onClick.RemoveAllListeners();

        noButton.onClick.RemoveAllListeners();
    }
    public void Init(string t, Action y = null, string yesString = "Yes", bool hideNo = false)
    {
        // group.alpha = 1;
        //// group.interactable = true;
        // group.blocksRaycasts = true;
        text.text = t;

        clearButton();

        if (y == null)
        {
            noButton.gameObject.SetActive(false);
            yesButton.onClick.AddListener(
                hideView
            );
        }
        else
        {

            noButton.gameObject.SetActive(hideNo);

            yesButton.onClick.AddListener(delegate
            {
                y(); hideView();
            });
            noButton.onClick.AddListener(delegate
            {
                hideView();
            });
        }

        yesButton.GetComponentInChildren<Text>().text =  yesString;
        //Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
