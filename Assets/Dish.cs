using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dish : MonoBehaviour
{
    DishData target;
    string dishName;
    public float moveSpeed = 4;
    bool isAroundRubishBin = false;
    Vector3 originalPositon;
    OrderCell aroundCell;
    int aroundIndex = -1;

    public SpriteRenderer Pan;
    public Sprite Pot;

    public Text dishText;

    public float checkCellDistance = 1;
    void Update()
    {
    }

    public void init(string dishName, DishData target)
    {
        this.target = target;
        this.dishName = dishName;

        if (dishName == null)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.black;
        }

        SFXManager.Instance.playfoodDeliver();
        OrderManager.Instance.removeDishSuccess(target);

        if(target.utensilType != "pan")
        {
            Pan.sprite = Pot;
        }
        //Destroy(gameObject, 1);
    }
}
