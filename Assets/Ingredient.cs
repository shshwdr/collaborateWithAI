using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public string ingredientType;
    public bool _canPick = true;

    public SpriteRenderer ingredientRender;
    public SpriteRenderer shadowRender;

    public void init(string type)
    {
        ingredientType = type;
        var sprite = IngredientManager.getIngredientImage(type);
        ingredientRender.sprite = sprite;
        shadowRender.sprite = IngredientManager.getIngredientImageShadow(type);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool startThrow = false;
    public AnimationCurve curve;
    Vector3 trashPosition;
    float time;
    Vector3 startPosition;
    public float moveSpeed = 2;
    public void throwToTrash()
    {
        startPosition = transform.position;
        startThrow = true;
        trashPosition = GameObject.FindObjectOfType<RubishBin>().transform.position;
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
        if (startThrow)
        {

            time += Time.deltaTime* moveSpeed;
            Vector3 pos = Vector3.Lerp(startPosition, trashPosition, time);
            pos.y += curve.Evaluate(time);
            transform.position = pos;
            if (time >= 1)
            {

                SFXManager.Instance.playthrowGarbage();
                Destroy(gameObject);
            }
        }
    }
}
