using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utencil : MonoBehaviour
{
    public string utencilType;

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
    }

    private void OnMouseUpAsButton()
    {
        cook();
    }
}
