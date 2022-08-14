using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissFaceMenu : MonoBehaviour
{




    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("missOrderChanged", missOrderChanged);
    }

    void missOrderChanged()
    {
        for(int i = 0; i < GetComponentsInChildren<MissFace>().Length; i++)
        {
            if (i < GameObject.FindObjectOfType<GameManager>().missedOrder && i< GetComponentsInChildren<MissFace>().Length)
            {
                GetComponentsInChildren<MissFace>()[i].getSad();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
