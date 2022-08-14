using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utencil : MonoBehaviour
{
    public string utencilType;

    public GameObject dishPrefab;

    List<GameObject> ingredients = new List<GameObject>();
    List<string> ingredientTypes = new List<string>();

    public Transform childParent;

    public ChatBox chatObject;

    public DishData note;

    public List<Image> noteImages;




    public GameObject staticUtensil;
    public Animator animStaticUtensil;

    public void playCookingAnimation()
    {
        staticUtensil.SetActive(false);
        animStaticUtensil.gameObject.SetActive(true);
    }

    public void stopCookingAnimation()
    {
        staticUtensil.SetActive(true);
        animStaticUtensil.gameObject.SetActive(false);
    }

    public bool hasIngredient(string ingredient)
    {
        foreach (Transform c in childParent.transform)
        {
            if (!c.GetComponent<Ingredient>())
            {
                continue;
            }
            if (ingredient == c.GetComponent<Ingredient>().ingredientType)
            {
                return true;
            }
        }
        return false;
    }

    void clearNote()
    {
        note = new DishData();
    }

    void updateNote()
    {
        int alreadyFinishedIngredient = 0;
        foreach(var data in OrderManager.Instance.dishes)
        {
            if (data.isPreRemoved)
            {
                continue;
            }
            if(data.utensilType == utencilType)
            {
                if (note.name == null)
                {

                    note = data;
                }
                //else
                //{
                //    int tempFinishedIngredient = 0;
                //    foreach (Transform c in childParent.transform)
                //    {
                //        if (data.ingredients.Contains(c.GetComponent<Ingredient>().ingredientType))
                //        {
                //            tempFinishedIngredient++;
                //        }
                //    }
                //    if (tempFinishedIngredient > alreadyFinishedIngredient)
                //    {
                //        note = data;
                //    }
                //}

            }
        }
        //update note image
        int i = 0;
        if (note.name == null)
        {
            return;
        }
        for (; i < note.ingredients.Count; i++)
        {
            noteImages[i].gameObject.SetActive(true);
            noteImages[i].sprite = IngredientManager.getIngredientImage(note.ingredients[i]);
        }
        for (; i < noteImages.Count; i++)
        {
            noteImages[i].gameObject.SetActive(false);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(noteImages[0].transform.parent.GetComponent<RectTransform>());
    }


    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = IngredientManager.getUtencilImage(utencilType);
        EventPool.OptIn("updateNote", updateNote);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool isInUse()
    {
        return GetComponentInChildren<Dish>();
    }

    public bool addIngredient(GameObject go)
    {
        if (isInUse())
        {
            //chatObject.show("Is In Use");
            return false;
        }
        ingredients.Add(go);
        ingredientTypes.Add(go.GetComponent<Ingredient>().ingredientType);

        IngredientManager.Instance.removeIngredient(go);

        go.transform.parent = childParent;
        go.transform.localPosition = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);

        if (canCook())
        {
            cook();
        }


        return true;
    }


    bool canCook()
    {
        var res = IngredientManager.cook(utencilType, ingredientTypes);
        if (res != null)
        {
            for (int i = 0; i < OrderManager.Instance.dishes.Count; i++)
            {
                //Vector3 cellPosition = OrderManager.Instance.getCellPosition(i);
                //var distance = ((Vector2)(cellPosition - transform.position)).magnitude;
                //if (distance < checkCellDistance)
                //{

                    if (OrderManager.Instance.dishes[i].name == res)
                    {
                    return true;
                    }
                //}
            }
        }
        return false;
    }

    void cook()
    {
        var res = IngredientManager.cook(utencilType, ingredientTypes);
        Debug.Log("cook "+res);
        if (res!=null)
        {

            chatObject.show("Cooked " + res);
        }
        else
        {

            chatObject.show("Cook failed!");
        }
        foreach (var ingre in ingredients)
        {
            //IngredientManager.Instance.removeIngredient(ingre);
            Destroy(ingre);
        }
        ingredientTypes.Clear();
        ingredients.Clear();
        DishData orderTransform = new DishData();
        if (res!=null)
        {

            orderTransform = OrderManager.Instance.tryRemove(res);
        }
        var dish = Instantiate(dishPrefab, childParent.position,Quaternion.identity, childParent);

        dish.GetComponent<Dish>().init(res, orderTransform);

        clearNote();
        updateNote();

    }

    private void OnMouseUpAsButton()
    {
        //if (ingredients.Count > 0 && GetComponentInChildren<Dish>() == null)
        //{
        //    cook();
        //}

        //clear ingredients
        foreach (var ingre in ingredients)
        {
            IngredientManager.Instance.removeIngredient(ingre);

            Destroy(ingre);
        }
        ingredients.Clear();
        ingredientTypes.Clear();
    }
}
