using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimatedLine : MonoBehaviour
{
    LineRenderer lineRender;
    [SerializeField] Texture[] textures;
    int animationStep;
    public float timeToSwitch = 0.1f;
    float switchTimer;
    // Start is called before the first frame update
    void Start()
    {
        lineRender = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switchTimer += Time.deltaTime;
        if(switchTimer>= timeToSwitch)
        {
            lineRender.material.SetTexture("_MainTex", textures[animationStep]);
            switchTimer -= timeToSwitch;
            animationStep++;
            if(animationStep == textures.Length)
            {
                animationStep = 0;
            }
        }
    }
}
