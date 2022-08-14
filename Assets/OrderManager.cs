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
    public string utensilType;
    public List<string> ingredients;
    public List<GameObject> ingredientsOnHold;

    public void remove()
    {
        isPreRemoved = true;
    }
    public DishData(string n, List<string> ing)
    {
        name = n;
        this.utensilType = ing[ing.Count - 1];
        isPreRemoved = false;
        time = Time.time;
        patienceTime = 50;
        ingredients = new List<string>();
        ingredientsOnHold = new List<GameObject>();
        for (int i = 0; i < ing.Count - 1; i++)
        {
            ingredients.Add(ing[i]);
            ingredientsOnHold.Add(null);
        }

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
            int panDish = 0;
            int potDish = 0;
            foreach(var di in dishes)
            {
                if(di.utensilType == "pan")
                {
                    panDish++;
                }
                else
                {
                    potDish++;
                }
            }

            if (panDish == 0)
            {
                addOrderByUtensil("pan");
            }
            else if (potDish == 0)
            {

                addOrderByUtensil("pot");
            }
            else
            {
                var selectedRecipe = IngredientManager.recipeByName.ElementAt(Random.Range(0, IngredientManager.recipeByName.Count));
                addOrder(selectedRecipe.Key);
            }
        }
    }

    void addOrder(string dishString)
    {
        var dishData = new DishData(dishString, IngredientManager.recipeByName[dishString]);
        dishes.Add(dishData);

        for (int i = 0; i < IngredientManager.recipeByName[dishString].Count - 1; i++)
        {
            var ingre = IngredientManager.recipeByName[dishString][i];
            ingredientToSelect.Add(new IngredientToSelectData(ingre, dishData));
        }


        EventPool.Trigger("updateOrder");
        EventPool.Trigger("updateNote");
        SFXManager.Instance.playcustomerArrive();
    }

    void addOrderByUtensil(string utensil)
    {

        var selectedRecipe = IngredientManager.recipe[utensil].ElementAt(Random.Range(0, IngredientManager.recipe[utensil].Count));

        addOrder(selectedRecipe[selectedRecipe.Count - 1]);
    }

    Dictionary<string, Utencil> utencilByName = new Dictionary<string, Utencil>();
    //Dictionary<string, DishData> noteByUtensil = new Dictionary<string, DishData>();
    Robot[] robots;
    public string findNextIngredient_v2()
    {
        List<DishData> sortedDishes = new List<DishData>();
        List<Utencil> sortedUtensils = new List<Utencil>();

        List<string> ingredientHoldByAnother = new List<string>();
        foreach(var mon in robots)
        {
            var workon = mon.currentWorkingIngredient();
            if (workon != null && workon.Count>0)
            {
                foreach(var w in workon)
                {

                    ingredientHoldByAnother.Add(w);
                }
            }
        }

        // sort recipe
        for (int i = 0; i < dishes.Count; i++)
        {
            foreach (var ut in utencilByName.Values)
            {
                if (ut.note.time == dishes[i].time && ut.note.name == dishes[i].name)
                {
                    sortedDishes.Add(ut.note);
                    sortedUtensils.Add(ut);
                }
            }
        }


        for (int i = 0; i < dishes.Count; i++)
        {
            bool found = true;
            foreach (var ut in utencilByName.Values)
            {
                if (ut.note.time == dishes[i].time)
                {
                    found = false;
                    break;
                }
            }
            if (found)
            {
                sortedDishes.Add(dishes[i]);
            }
        }

        //Debug.Log("sorted dish:");
        //foreach (var dish in sortedDishes)
        //{

        //    Debug.Log(dish.name);
        //}

        // sort ingredient

        for (int i = 0; i < sortedDishes.Count; i++)
        {
            if (i < sortedUtensils.Count)
            {
                //if in note, check if the ingredient is in utensil
                var utensil = sortedUtensils[i];
                var note = utensil.note;
                for (int j = 0; j < note.ingredients.Count; j++)
                {
                    var ing = note.ingredients[j];
                    {
                        if (utensil.hasIngredient(ing))
                        {
                            continue;
                        }
                        if (ingredientHoldByAnother.Contains(ing))
                        {
                            ingredientHoldByAnother.Remove(ing);
                            continue;
                        }
                        return ing;
                    }
                }
            }
            else
            {
                for (int j = 0; j < sortedDishes[j].ingredients.Count; j++)
                {
                    var ing = sortedDishes[j].ingredients[j];
                    {
                        if (ingredientHoldByAnother.Contains(ing))
                        {
                            ingredientHoldByAnother.Remove(ing);
                            continue;
                        }
                        return ing;
                    }
                }
            }
        }
            return null;
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
                // dishes[i].isPreRemoved = true;
                dish.isPreRemoved = true;
                dishes[i] = dish;
                    playCookEffect(i);
                EventPool.Trigger("updateNote");
                return dishes[i];
                }
            }
            return new DishData();
        }

    void playCookEffect(int index)
    {
        var cell = cells[index];
        cell.playCookingAnimation();
        cell.success();
        utencilByName[cell.dishData.utensilType].playCookingAnimation();
    }
    void stopCellAnimation(int index)
    {
        var cell = cells[index];
        cell.stopCookingAnimation();
        //cell.success();
        utencilByName[cell.dishData.utensilType].stopCookingAnimation();
    }


        //void remove(int index)
        //{

    //    EventPool.Trigger("finishOrder", dishes[index].name);
    //    dishes.RemoveAt(index);
    //    EventPool.Trigger("updateOrder");
    //}
    public void remove(DishData data)
        {
            var index = dishes.FindIndex(x => x.time == data.time && x.name == data.name);
            if (index == -1)
            {
                Debug.LogError("remove dish wrong");
                return;
            }

        stopCellAnimation(index);

            var dishString = data.name;
            int selectionIndexToBeDecrease = 0;
            for (int i = 0; i < IngredientManager.recipeByName[dishString].Count - 1; i++)
            {
                var ingre = IngredientManager.recipeByName[dishString][i];
                var removeIndex = ingredientToSelect.FindIndex(x => x.ingredient == ingre && x.dishData.time == data.time);
                //if (removeIndex <= currentSelectIngredientIndex)
                //{
                //    selectionIndexToBeDecrease++;
                //}
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
        robots = GameObject.FindObjectsOfType<Robot>();
        addOrderByUtensil("pot");
        addOrderByUtensil("pan");
            var utensils = GameObject.FindObjectsOfType<Utencil>();
            foreach (var u in utensils)
            {

                utencilByName[u.utencilType] = u;
            }
            //cells = GameObject.FindObjectsOfType<OrderCell>(true);
        }

    public struct IngredientToSelectData
    {
        public string ingredient;
        public bool isFinding;
        public DishData dishData;

        public IngredientToSelectData(string name, DishData data)
        {
            ingredient = name;
            dishData = data;
            isFinding = false;
        }
    }

    public List<IngredientToSelectData> ingredientToSelect = new List<IngredientToSelectData>();

}
