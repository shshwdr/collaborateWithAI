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
    public float patienceTime;
    public int customerIndex;
    public DishData(string n)
    {
        name = n;
        isPreRemoved = false;
        time = Time.time;
        patienceTime = 50;
        customerIndex = Random.Range(0, OrderManager.Instance.customerSprites.Count);
    }
}
public class OrderManager : Singleton<OrderManager>
{
    public List<Sprite> customerSprites;

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
        var dishData = new DishData(dishString);
        dishes.Add(dishData);

        for(int i = 0;i< IngredientManager.recipeByName[dishString].Count - 1; i++)
        {
            var ingre = IngredientManager.recipeByName[dishString][i];
            ingredientToSelect.Add(new IngredientToSelectData(ingre, dishData));
        }


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

    //void remove(int index)
    //{

    //    EventPool.Trigger("finishOrder", dishes[index].name);
    //    dishes.RemoveAt(index);
    //    EventPool.Trigger("updateOrder");
    //}
    public void remove(DishData data)
    {
        var index = dishes.FindIndex(x => x.time == data.time);
        if(index == -1)
        {
            Debug.LogError("remove dish wrong");
            return;
        }

        var dishString = data.name;
        int selectionIndexToBeDecrease = 0;
            for (int i = 0; i < IngredientManager.recipeByName[dishString].Count - 1; i++)
        {
            var ingre = IngredientManager.recipeByName[dishString][i];
            var removeIndex = ingredientToSelect.FindIndex(x => x.ingredient == ingre&&x.dishData.time == data.time);
            if (removeIndex <= currentSelectIngredientIndex)
            {
                selectionIndexToBeDecrease++;
            }
            ingredientToSelect.RemoveAt(removeIndex);
        }
        currentSelectIngredientIndex -= selectionIndexToBeDecrease;


        dishes.RemoveAt(index);


        EventPool.Trigger("updateOrder");
        EventPool.Trigger("finishOrder", data.name);
    }

    

    int currentSelectIngredientIndex = 0;
    public string findNextIngredient()
    {
            int test = 10;
            while (test > 0)
        {


            if (ingredientToSelect.Count <= currentSelectIngredientIndex)
            {
                currentSelectIngredientIndex = 0;
            }
            if (currentSelectIngredientIndex < 0)
            {
                Debug.LogError("should not get lower than 0");
                currentSelectIngredientIndex = 0;
            }
            var selection = ingredientToSelect[currentSelectIngredientIndex].ingredient;

            currentSelectIngredientIndex++;

            var ingredientExist = IngredientManager.Instance.doesIngredientHasCount(selection);
            if (ingredientExist)
            {
                return selection;

            }
            test--;
        }


        return IngredientManager.Instance.findIngredientWithCount();
    }
    public Vector3 getDishCellPosition(DishData data)
    {
        var index = dishes.FindIndex(x => x.time == data.time);
        var transform = cells[index].transform;
        Vector3 orderPosition = Camera.main.ScreenToWorldPoint(transform.position);
        return orderPosition;
    }

    public Vector3 getCellPosition(int index)
    {

        var transform = cells[index].transform;
        Vector3 orderPosition = Camera.main.ScreenToWorldPoint(transform.position);
        return orderPosition;
    }

    public void init()
    {
        dishes = new List<DishData>();
        addOrder();
        addOrder();
        //cells = GameObject.FindObjectsOfType<OrderCell>(true);
    }

    public struct IngredientToSelectData
    {
       public  string ingredient;
        public bool isFinding;
       public  DishData dishData;

        public IngredientToSelectData(string name,DishData data)
        {
            ingredient = name;
            dishData = data;
            isFinding = false;
        }
    }

    public List<IngredientToSelectData> ingredientToSelect = new List<IngredientToSelectData>();

}
