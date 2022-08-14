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

    public Text dishText;

    public float checkCellDistance = 1;
    void Update()
    {
        //if (target.name != null)
        //{
        //    var orderPosition = OrderManager.Instance.getDishCellPosition(target);
        //    var dir = orderPosition - transform.position;
        //    if (dir.magnitude < 0.1f)
        //    {
        //        Destroy(gameObject);

        //        SFXManager.Instance.playfoodDeliver();
        //        OrderManager.Instance.remove(target);
        //    }
        //    else
        //    {

        //        dir.Normalize();
        //        transform.Translate(dir * moveSpeed * Time.deltaTime);
        //    }
        //}
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

        Destroy(gameObject, 1);
    }
}
