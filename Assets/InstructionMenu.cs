using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var buttons = GetComponentsInChildren<InstructionButton>(true);
        var instructions = IngredientManager.InstructionTypes; 
        int i = 0;
        for (; i < instructions.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].init( instructions[i]);
        }
        for (; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
