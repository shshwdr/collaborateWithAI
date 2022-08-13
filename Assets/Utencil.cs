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


    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = IngredientManager.getUtencilImage(utencilType);
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
            chatObject.show("Is In Use");
            return false;
        }
        ingredients.Add(go);
        ingredientTypes.Add(go.GetComponent<Ingredient>().ingredientType);


        go.transform.parent = childParent;
        go.transform.localPosition = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
        return true;
    }

    void cook()
    {
        var res = IngredientManager.Instance.cook(utencilType, ingredientTypes);
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
            IngredientManager.Instance.removeIngredient(ingre);
            Destroy(ingre);
        }
        ingredientTypes.Clear();
        DishData orderTransform = new DishData();
        //if (res!=null)
        //{

        //    orderTransform = OrderManager.Instance.tryRemove(res);
        //}
        var dish = Instantiate(dishPrefab, childParent.position,Quaternion.identity, childParent);

        dish.GetComponent<Dish>().init(res, orderTransform);
    }

    private void OnMouseUpAsButton()
    {
        if (ingredients.Count > 0 && GetComponentInChildren<Dish>()==null)
        {
            cook();
        }
    }
}
