using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : MonoBehaviour
{
    DishData target;
    string dishName;
    public float moveSpeed = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target.name!=null)
        {
            var orderPosition = OrderManager.Instance. getDishCellPosition(target);
            var dir = orderPosition - transform.position;
            if (dir.magnitude < 0.1f)
            {
                OrderManager.Instance.remove(target);
                Destroy(gameObject);
            }
            else
            {

                dir.Normalize();
                transform.Translate(dir * moveSpeed * Time.deltaTime);
            }
        }
    }

    public void init(string dishName, DishData target)
    {
        this.target = target;
        this.dishName = dishName;

        if (dishName == null)
        {
            GetComponentInChildren<SpriteRenderer>().color = Color.black;
        }
    }
}
