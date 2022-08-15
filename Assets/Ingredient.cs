using DG.Tweening;
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

    Transform pot;
    public void throwToTrash()
    {
        startPosition = transform.position;
        startThrow = true;
        trashPosition = GameObject.FindObjectOfType<RubishBin>().transform.position;
    }
    public void throwToPot(Transform pot, int count)
    {
        this.pot = pot;
        startPosition = transform.position;
        startPutIntoPot = true;
        trashPosition = pot.position + (count == 1?new Vector3(-1,0,0): new Vector3(1, 0, 0));
    }

    public void startToFry()
    {
        startFry = true;
    }

    public bool canPick()
    {
        return _canPick;
    }

    public void pick()
    {
        _canPick = false;
    }

    bool startPutIntoPot;
    bool startFry;

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
                Debug.Log("throw");

                SFXManager.Instance.playthrowGarbage();
                Destroy(gameObject);
                GameObject.FindObjectOfType<RubishBin>().playAnim();
                time = 0;
                startThrow = false;
            }
        }
        if (startPutIntoPot && time <= 1)
        {

            time += Time.deltaTime * moveSpeed;
            Vector3 pos = Vector3.Lerp(startPosition, trashPosition, time);
            pos.y += curve.Evaluate(time);
            transform.position = pos;

            if (time >= 1)
            {
                Debug.Log("put in bin");
                SFXManager.Instance.playthrowPot();
                pot.GetComponentInParent<Utencil>().finishAddIngredient();
                time = 0;
                startPutIntoPot = false;
            }
        }

        if (startFry)
        {
            transform.DOLocalJump(Vector3.zero, Random.Range(1f, 2f), Random.Range(2, 4), 1);
        }
    }
}
