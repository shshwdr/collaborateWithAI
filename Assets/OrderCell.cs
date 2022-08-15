using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class OrderCell : MonoBehaviour
{

    public string dish;
    public DishData dishData;
    public Text dishString;

    public ChatBox chatBox;
    public List<Image> ingredientImages;
    public Image utencilImage;

    public Image patienceBar;
    public Sprite angryPatienceBar;
    public Sprite normalPatienceBar;
    public Image customerImage;


    public Sprite angryBackground;

    public Sprite happyBackground;
    public Sprite normalBackground;
    public Image background;


    public Image happyFace;
    public Image sadFace;

    public float angryPercent = 0.3f;


    public GameObject staticUtensil;

    bool isPreSuccess = false;


    public Animator animator;
    //public Text dishString;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void playCookingAnimation()
    {
       // staticUtensil.SetActive(false);
        //animator.gameObject.SetActive(true);
        isPreSuccess = true;

    }

    public void stopCookingAnimation()
    {
       // staticUtensil.SetActive(true);
       // animator.gameObject.SetActive(false);
    }

    public void init(DishData data)
    {

        chatBox.hide();
        GetComponent<UIView>().Show();
        dish = data.name;
        dishData = data;
        dishString.text = dish;

        happyFace.gameObject.SetActive(false);
        sadFace.gameObject.SetActive(false);
        background.sprite = normalBackground;
        patienceBar.sprite = normalPatienceBar;

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

       // animator.runtimeAnimatorController = IngredientManager.getUtencilAnimation(recipe[recipe.Count - 1]);
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

    void success()
    {
        sadFace.gameObject.SetActive(false);
        happyFace.gameObject.SetActive(true);

        background.sprite = happyBackground;

        List<string> succeedWords = new List<string>()
        {
            "Yummy Yummy!",
            "This looks tasty!",
            "Smells good!",

        };


        chatBox.show(succeedWords[Random.Range(0, succeedWords.Count)]);
    }

    void fail()
    {
        //sadFace.gameObject.SetActive(false);
        //happyFace.gameObject.SetActive(true);

        background.sprite = angryBackground;

        GameObject.FindObjectOfType<GameManager>().missAnOrder();

        List<string> failWords = new List<string>()
        {
            "WHERE IS MY FOOD???",
            "I'm starving!",
            "This is insanely slow!",

        };


        chatBox.show(failWords[Random.Range(0, failWords.Count)]);
    }

    public void removeCellWithAnim(bool succeed)
    {
        if (succeed)
        {
            success();
        }
        else
        {
            fail();
        }
        //GetComponent<UIView>().Hide();
        Invoke("fullyRemove", 2f);
    }

    void fullyRemove()
    {

        OrderManager.Instance.fullyRemoveDish(dishData);
    }

    // Update is called once per frame
    void Update()
    {
        if (dishData.isPreRemoved)
        {
            return;
        }
        var timeDiff =  Time.time - dishData.time;
        if (timeDiff > dishData.patienceTime)
        {
            //this one need to leave now
            OrderManager.Instance.removeDishFail(dishData);
            SFXManager.Instance.playcustomerLeave();

            dishData.isPreRemoved = true;
            OrderManager.Instance.setDishToBePreRemoved(dishData);
        }
        else
        {
            patienceBar.fillAmount = (dishData.patienceTime - timeDiff) / dishData.patienceTime;
        }

        if (patienceBar.fillAmount < angryPercent)
        {
            patienceBar.sprite = angryPatienceBar;
            sadFace.gameObject.SetActive(true);
        }
    }
}
