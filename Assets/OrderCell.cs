using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderCell : MonoBehaviour
{

    public string dish;
    public Text dishString;
    //public Text dishString;
    // Start is called before the first frame update
    void Start()
    {
        dishString.text += dish +"\n";

        var recipe = IngredientManager.recipeByName[dish];
        for(int i = 0; i < recipe.Count - 1; i++)
        {

            dishString.text += recipe[i] + " ";

        }

        dishString.text +=  "\n";

        dishString.text += recipe[recipe.Count - 1] + " ";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
