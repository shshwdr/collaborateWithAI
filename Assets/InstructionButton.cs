using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionButton : MonoBehaviour
{
    public string instruction;
    Image instructionImage;
    public void onClick()
    {
       // GetComponentInParent<Robot>().selectInstruction(instruction);
    }

   public void init(string ins)
    {
        instruction = ins;

        Sprite image = Resources.Load<Sprite>("instruction/" + ins);
        if (!image)
        {
            Debug.Log(" no instruction image " + ins);
        }
        GetComponent<Image>().sprite = image;

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
