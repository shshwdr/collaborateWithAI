using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissFace : MonoBehaviour
{

    public Sprite sadFace;
    public Image faceImage;
    bool isSad = false;
    public void getSad()
    {

        if (!isSad)
        {

            isSad = true;
            faceImage.sprite = sadFace;

            faceImage.transform.DOScale(2,0.3f).SetLoops(2,LoopType.Yoyo);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
