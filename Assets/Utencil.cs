using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utencil : MonoBehaviour
{
    public string utencilType;

    public GameObject dishPrefab;

    List<GameObject> ingredients = new List<GameObject>();
    List<string> ingredientTypes = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addIngredient(GameObject go)
    {
        ingredients.Add(go);
        ingredientTypes.Add(go.GetComponent<Ingredient>().ingredientType);


        go.transform.parent = transform;
        go.transform.localPosition = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
    }

    void cook()
    {
        var res = IngredientManager.Instance.cook(utencilType, ingredientTypes);
        Debug.Log("cook "+res);
        foreach(var ingre in ingredients)
        {
            IngredientManager.Instance.removeIngredient(ingre);
            Destroy(ingre);
        }

        DishData orderTransform = new DishData();
        if (res!=null)
        {

            orderTransform = OrderManager.Instance.tryRemove(res);
        }
        var dish = Instantiate(dishPrefab,transform.position,Quaternion.identity);

        dish.GetComponent<Dish>().init(res, orderTransform);
    }

    private void OnMouseUpAsButton()
    {
        if (ingredients.Count > 0)
        {
            cook();
        }
    }
}
