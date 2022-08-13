using Pool;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct DishData
{
    public string name;
    public bool isPreRemoved;
    public float time;
    public DishData(string n)
    {
        name = n;
        isPreRemoved = false;
        time = Time.time;
    }
}
public class OrderManager : Singleton<OrderManager>
{
    public List<DishData> dishes;
    public OrderCell[] cells;
    float orderTime = 3f;
    float currentOrderTimer = 0f;
    public int maxOrder = 4;

    //private void Awake()
    //{

    //    //EventPool.OptIn<string>("finishOrder", removeOrder);
    //}
    private void Update()
    {
        currentOrderTimer += Time.deltaTime;
        if (currentOrderTimer >= orderTime)
        {
            currentOrderTimer = 0;
            addOrder();
        }

    }
    void addOrder()
    {
        if (maxOrder > dishes.Count)
        {
            var selectedRecipe = IngredientManager.recipeByName.ElementAt(Random.Range(0, IngredientManager.recipeByName.Count));
            addOrder(selectedRecipe.Key);
        }
    }

    void addOrder(string dishString)
    {
        dishes.Add(new DishData(dishString));
        EventPool.Trigger("updateOrder");
    }
    //public void removeOrder(string dishString)
    //{
    //    dishes.Remove(new DishData(dishString));
    //    EventPool.Trigger("updateOrder");
    //}
    //public void removeOrder(int dishString)
    //{
    //    dishes.RemoveAt(dishString);
    //    EventPool.Trigger("updateOrder");
    //}

    public DishData tryRemove(string dishString)
    {
        for (int i = 0; i < dishes.Count; i++)
        {
            var dish = dishes[i];
            if (dish.name == dishString && !dish.isPreRemoved)
            {
                dish.isPreRemoved = false;
                return dishes[i];
            }
        }
        return new DishData();
    }
    public void remove(DishData data)
    {
        var index = dishes.FindIndex(x => x.time == data.time);
        dishes.RemoveAt(index);
        EventPool.Trigger("updateOrder");
    }
    public Vector3 getDishCellPosition(DishData data)
    {
        var index = dishes.FindIndex(x => x.time == data.time);
        var transform = cells[index].transform;
            Vector3 orderPosition = Camera.main.ScreenToWorldPoint(transform.position);
        return orderPosition;
    }

    public void init()
    {
        dishes = new List<DishData>();
        addOrder();
        //cells = GameObject.FindObjectsOfType<OrderCell>(true);
    }


}
