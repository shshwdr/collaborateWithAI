using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slap : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Sprite hurryImage;
    public void useHurry()
    {
        sprite.sprite = hurryImage;
    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
