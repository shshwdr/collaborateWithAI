using DG.Tweening;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public List<Image> checkImages;

    AudioSource audioSource;


    public GameObject staticUtensil;
    public Animator animStaticUtensil;

    public void playCookingAnimation()
    {
        staticUtensil.SetActive(false);
        animStaticUtensil.gameObject.SetActive(true);
        audioSource.Play();
    }

    public void stopCookingAnimation()
    {
        staticUtensil.SetActive(true);
        animStaticUtensil.gameObject.SetActive(false);
        audioSource.Stop();
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

    void updateCheck()
    {
        int i = 0;
        for (; i < note.ingredients.Count && i< checkImages.Count; i++)
        {
            var ing = note.ingredients[i];
            if (ingredientTypes.Contains(ing) && checkImages[i] && checkImages[i].transform)
            {
                checkImages[i].transform.DOKill();
                checkImages[i].transform.localScale = Vector3.zero;
                checkImages[i].transform.DOScale(1, 0.3f);
            }
            else
            {

                checkImages[i].transform.localScale = Vector3.zero;
            }
        }
        for(;i< checkImages.Count; i++)
        {
            checkImages[i].transform.localScale = Vector3.zero;
        }
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
                //if (note.name == null)
                {

                    note = data;
                    break;
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

        //LayoutRebuilder.ForceRebuildLayoutImmediate(noteImages[0].transform.parent.GetComponent<RectTransform>());

        updateCheck();
    }


    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = IngredientManager.getUtencilImage(utencilType);
        EventPool.OptIn("updateNote", updateNote);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool isCooking = false;
    bool isInUse()
    {
        return isCooking;
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
        //go.transform.localPosition = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);

        go.GetComponent<Ingredient>().throwToPot(childParent,ingredients.Count);
        updateCheck();
        return true;
    }


    public void finishAddIngredient() {

        if (isInUse())
        {
            return;
        }

        if (canCook())
        {
            startCook();
            //cook();
        }
        else if (ingredients.Count >= 2)
        {
            OnMouseUpAsButton();

            TutorialManager.Instance.showTutorial("throw");
        }

        EventPool.Trigger("updateNote");

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

    //void cook()
    //{
    //    var res = IngredientManager.cook(utencilType, ingredientTypes);
    //    Debug.Log("cook "+res);
    //    //if (res!=null)
    //    //{

    //    //    chatObject.show("Cooked " + res);
    //    //}
    //    //else
    //    //{

    //    //    chatObject.show("Cook failed!");
    //    //}
    //    foreach (var ingre in ingredients)
    //    {
    //        //IngredientManager.Instance.removeIngredient(ingre);
    //        Destroy(ingre);
    //    }
    //    ingredientTypes.Clear();
    //    ingredients.Clear();
    //    DishData orderTransform = new DishData();
    //    if (res!=null)
    //    {

    //        orderTransform = OrderManager.Instance.tryRemove(res);
    //    }
    //    var dish = Instantiate(dishPrefab, childParent.position,Quaternion.identity, childParent);

    //    dish.GetComponent<Dish >().init(res, orderTransform);

    //    clearNote();
    //    updateNote();

    //}

    DishData currentCookingDish;
    void startCook()
    {
        isCooking = true;

        var res = IngredientManager.cook(utencilType, ingredientTypes);
        currentCookingDish = OrderManager.Instance.tryRemove(res);


        //if(utencilType == "pan")
        //{
        //    foreach(var ing in ingredients)
        //    {
        //        ing.GetComponent<Ingredient>().startToFry();
        //    }
        //}
        //else
        {

            foreach (var ing in ingredients)
            {
                ing.SetActive(false);
            }
        }

        clearNote();
        updateNote();
        Debug.Log("cooking " + res);

        Invoke("finishCook", cookTime);
    }
    float cookTime = 2;

    void finishCook()
    {
        isCooking = false;
        stopCookingAnimation();
        var res = IngredientManager.cook(utencilType, ingredientTypes);

        Debug.Log("cooked " + res);

        foreach (var ingre in ingredients)
        {
            //IngredientManager.Instance.removeIngredient(ingre);
            Destroy(ingre);
        }
        ingredientTypes.Clear();
        ingredients.Clear();
        var dish = Instantiate(dishPrefab, staticUtensil.transform.position, Quaternion.identity, staticUtensil.transform.parent);
        dish.transform.localScale = staticUtensil.transform.localScale;
        dish.GetComponent<Dish>().init(res, currentCookingDish);
        currentCookingDish = new DishData();
        Destroy(dish, cookTime);
        EventPool.Trigger("updateNote");
    }

    private void OnMouseUpAsButton()
    {
        //if (ingredients.Count > 0 && GetComponentInChildren<Dish>() == null)
        //{
        //    cook();
        //}
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
            //clear ingredients
            foreach (var ingre in ingredients)
        {
            IngredientManager.Instance.removeIngredient(ingre);
            ingre.GetComponent<Ingredient>().throwToTrash();
            //Destroy(ingre);
        }
        ingredients.Clear();
        ingredientTypes.Clear();

    }
}
