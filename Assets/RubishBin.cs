using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubishBin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void throwItem(GameObject ingredient)
    {
        IngredientManager.Instance.removeIngredient(ingredient);
        Destroy(ingredient);
    }
}
