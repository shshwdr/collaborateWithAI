using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderManager : Singleton<OrderManager>
{
    List<string> dishes;

    void addOrder()
    {
        var selectedRecipe = IngredientManager.recipe.ElementAt(Random.Range(0, IngredientManager.recipe.Count));
        addOrder(selectedRecipe.Key);
    }

    void addOrder(string dishString)
    {
        dishes.Add(dishString);
    }
    public void removeOrder(string dishString)
    {
        dishes.Remove(dishString);
    }

    public void init()
    {
        dishes = new List<string>();
        addOrder();
        addOrder();
        addOrder();
        addOrder();
    }
}
