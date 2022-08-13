using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderMenu : MonoBehaviour
{
    private void Awake()
    {
        EventPool.OptIn("updateOrder", updateOrder);
    }

    void updateOrder()
    {
        Debug.Log("update order");
        int i = 0;
        var dishes = OrderManager.Instance.dishes;
        foreach(var cell in OrderManager.Instance.cells)
        {
            if (i < dishes.Count)
            {

                cell.init(dishes[i]);
                cell.gameObject.SetActive(true);
            }
            else
            {
                cell.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
