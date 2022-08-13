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

    private void OnMouseDrag()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (target.name!=null)
        //{
        //    var orderPosition = OrderManager.Instance. getDishCellPosition(target);
        //    var dir = orderPosition - transform.position;
        //    if (dir.magnitude < 0.1f)
        //    {
        //        Destroy(gameObject);
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
    }
}
