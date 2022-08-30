using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubishBin : MonoBehaviour
{
    Vector3 originalScale;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void throwItem(GameObject ingredient)
    {
       // IngredientManager.Instance.removeIngredient(ingredient);
        ingredient.GetComponent<Ingredient>().throwToTrash();
        //Destroy(ingredient);
    }

    public void playAnim()
    {
        transform.DOKill();
        transform.localScale = originalScale;
        transform.DOShakeScale(0.5f);
    }
}
