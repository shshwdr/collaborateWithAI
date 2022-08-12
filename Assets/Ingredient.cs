using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public string ingredientType;
    public bool _canPick = true;

    public void init(string type)
    {
        ingredientType = type;

        GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("ingredient/"+type);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool canPick()
    {
        return _canPick;
    }

    public void pick()
    {
        _canPick = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
