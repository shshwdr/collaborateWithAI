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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<RubishBin>())
        {
            isAroundRubishBin = true;
        }
        if (collision.GetComponentInChildren<OrderCell>())
        {
            aroundCell = collision.GetComponentInChildren<OrderCell>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.GetComponent<RubishBin>())
        {
            isAroundRubishBin = false;
        }
    }
    private void OnMouseDown()
    {
        originalPositon = transform.position;
    }

    private void OnMouseEnter()
    {
        dishText.text = dishName;   
    }
    private void OnMouseExit()
    {
        dishText.text = "";
    }
    void OnMouseUp()
    {
        if (isAroundRubishBin)
        {
            Destroy(gameObject);
        }
        else if (aroundCell)
        {
            Destroy(gameObject);
            OrderManager.Instance.remove(aroundIndex);
        }
        else
        {
            transform.position = originalPositon;
        }
    }

    private void checkCells()
    {
        aroundCell = null;
        for (int i = 0;i< OrderManager.Instance.dishes.Count; i++)
        {
            Vector3 cellPosition = OrderManager.Instance.getCellPosition(i);
            var distance =((Vector2) (cellPosition - transform.position)).magnitude;
            if (distance< checkCellDistance)
            {

                if (OrderManager.Instance.dishes[i].name != dishName)
                {
                    OrderManager.Instance.cells[i].showWithFood(false);
                    Debug.Log("I don't like it");
                }
                else
                {

                    OrderManager.Instance.cells[i].showWithFood(true);
                    Debug.Log("I like it! distance to cell " + i + " is close");
                    aroundCell = OrderManager.Instance.cells[i];
                    aroundIndex = i;
                }
            }
        }
    }

    private void OnMouseDrag()
    {
        Vector2 MousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 objPosition = Camera.main.ScreenToWorldPoint(MousePosition);
        transform.position = objPosition;

        checkCells();
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
