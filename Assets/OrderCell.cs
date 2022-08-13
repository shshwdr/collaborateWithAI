using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderCell : MonoBehaviour
{

    public string dish;
    DishData dishData;
    public Text dishString;

    public ChatBox chatBox;
    public List<Image> ingredientImages;
    public Image utencilImage;

    public Image patienceBar;
    public Image customerImage;
    //public Text dishString;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void init(DishData data)
    {
        dish = data.name;
        dishData = data;
        dishString.text = dish;

        var recipe = IngredientManager.recipeByName[dish];
        int i = 0;
        for (; i < recipe.Count - 1; i++)
        {
            ingredientImages[i].gameObject.SetActive(true);
            ingredientImages[i].sprite =  IngredientManager.getIngredientImage(recipe[i]);
            //dishString.text += recipe[i] + " ";

        }
        for (; i < ingredientImages.Count; i++)
        {

            ingredientImages[i].gameObject.SetActive(false);
        }

           // dishString.text += "\n";
        utencilImage.sprite = IngredientManager.getUtencilImage(recipe[recipe.Count - 1]);

        customerImage.sprite = OrderManager.Instance.customerSprites[data.customerIndex];
        //dishString.text += recipe[recipe.Count - 1] + " ";
    }

    public void showWithFood(bool like)
    {
        if (like)
        {
            chatBox.show("give me give me!");
        }
        else
        {

            chatBox.show("not interested..");
        }
    }

    // Update is called once per frame
    void Update()
    {
        var timeDiff =  Time.time - dishData.time;
        if (timeDiff > dishData.patienceTime)
        {
            //this one need to leave now
            OrderManager.Instance.remove(dishData);
        }
        else
        {
            patienceBar.fillAmount = (dishData.patienceTime - timeDiff) / dishData.patienceTime;
        }
    }
}
