using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionButton : MonoBehaviour
{
    public string instruction;

    public void onClick()
    {
        GetComponentInParent<Robot>().selectInstruction(instruction);
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
