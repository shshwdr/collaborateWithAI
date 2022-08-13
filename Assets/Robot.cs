using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Robot : MonoBehaviour
{
    public GameObject instructionSelectionPanel;
    string instruction;
    GameObject target;
    public float moveSpeed;
    GameObject holdIngredent;
    GameObject rubishBin;

    public Image currentInstructionImage;

    float pickupRange = 0.1f;

    bool isSelecting = false;

    public ChatBox chatObject;

    LineRenderer path;
    // Start is called before the first frame update
    void Start()
    {
        path = GetComponentInChildren<LineRenderer>();
        instructionSelectionPanel.SetActive(isSelecting);
        rubishBin = GameObject.FindObjectOfType<RubishBin>().gameObject;
        smartSelectInstruction();
    }
    bool isClick()
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider == GetComponent<Collider2D>())
        {
            return true;
        }
        return false;
    }

    IEnumerator test()
    {

            if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
            {
                yield return new WaitForSeconds(0.2f);
                isSelecting = false;
                instructionSelectionPanel.SetActive(isSelecting);
            }
    }
    // Update is called once per frame
    void Update()
    {

    if (isSelecting)
        {
            StartCoroutine(test());
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!isSelecting && isClick())
            {
               // Debug.Log("slap");
                slap();
            }

        }
        //if (Input.GetMouseButtonDown(1))
        //{
        //    if (!isSelecting && isClick())
        //    {
        //        //Debug.Log("right");
        //        isSelecting = true;
        //        instructionSelectionPanel.SetActive(isSelecting);
        //    }
        //}

        //if (isDeciding)
        //{
        //    decidingTimer -= Time.deltaTime;
        //    if (decidingTimer <= 0)
        //    {
        //        isDeciding = false;
        //    }
        //    else
        //    {
        //        return;
        //    }
        //}

        if (target)
        {
            path.SetPosition(0, transform.position);
            path.SetPosition(1, target.transform.position);


            if (!holdIngredent)
            {
                if (!target.GetComponent<Ingredient>().canPick())
                {
                    Debug.Log("already picked by another");
                    chatObject.show("Don't take my stuff!");
                    decideNextTarget();
                }
            }

            if ((target.transform.position - transform.position).magnitude<pickupRange)
            {
                if (holdIngredent)
                {
                    //drop ingredient and move to next

                    if (target.GetComponent<RubishBin>())
                    {
                        target.GetComponent<RubishBin>().throwItem(holdIngredent);
                    }
                    else
                    {

                        if (!target.GetComponent<Utencil>().addIngredient(holdIngredent))
                        {
                            slap();
                            return;
                        }
                    }

                    holdIngredent = null;
                    target = null;


                    visitedObjects = new List<GameObject>();
                    // startDeciding();
                    startNextIngredientSelection();
                    //smartSelectInstruction();
                    decideNextTarget();
                }
                else
                {
                    //pickup item
                    target.GetComponent<Ingredient>().pick();
                    holdIngredent = target;
                    target.transform.parent = transform;
                    target = IngredientManager.Instance.selectUtencil(this,visitedObjects);
                    smartDecideNextIngredientSelection();
                }
            }
            else
            {

                var dir = (target.transform.position - transform.position).normalized;
                transform.Translate(dir * moveSpeed * Time.deltaTime);
            }
        }
        else
        {

            decideNextTarget();
        }
    }

    public void smartSelectInstruction()
    {
        var nextIngredient = OrderManager.Instance.findNextIngredient();
        var nextInstruction = IngredientManager.ingredientToInstructions[nextIngredient][0];
        decideNextIngredientSelection(nextInstruction);
        startNextIngredientSelection();
        //selectInstruction(nextInstruction);
    }

    void smartDecideNextIngredientSelection()
    {
        var nextIngredient = OrderManager.Instance.findNextIngredient();
        var nextInstruction = IngredientManager.ingredientToInstructions[nextIngredient][0];
        decideNextIngredientSelection(nextInstruction);
    }

    public void randomeSelectInstruction()
    {

        selectInstruction(IngredientManager.InstructionTypes[Random.Range(0, IngredientManager.InstructionTypes.Count)]);
    }

    public void decideNextIngredientSelection(string ins)
    {

        instruction = ins;
        Sprite image = Resources.Load<Sprite>("instruction/" + ins);
        if (!image)
        {
            Debug.Log(" no instruction image " + ins);
        }
        currentInstructionImage.sprite = image;
        currentInstructionImage.gameObject.SetActive(true);
    }

    public void startNextIngredientSelection()
    {

        visitedObjects = new List<GameObject>();
        decideNextTarget();
    }

    public void selectInstruction(string ins)
    {
        instruction = ins;
        visitedObjects = new List<GameObject>();

        //startDeciding();


        decideNextTarget();

        isSelecting = false;
        instructionSelectionPanel.SetActive(isSelecting);
        Sprite image = Resources.Load<Sprite>("instruction/" + ins);
        if (!image)
        {
            Debug.Log(" no instruction image " + ins);
        }
        currentInstructionImage.sprite = image;
        currentInstructionImage.gameObject.SetActive(true);
    }

    void decideNextTarget()
    {
        // take some time to decide
        // find the closest item matches the instruction

        if (holdIngredent)
        {
            return;
        }

        var selectItem = IngredientManager.Instance. selectItem(instruction, this,visitedObjects);
        target = selectItem;

    }

    bool isDeciding = false;
    float decidingTime = 2f;
    float decidingTimer = 0;
    //void startDeciding()
    //{
    //    isDeciding = true;
    //    chatObject.show("What to take");
    //    decidingTimer = decidingTime;
    //}

    List<GameObject> visitedObjects = new List<GameObject>();

    void slap()
    {
        if (holdIngredent)
        {
            var nextOption = IngredientManager.Instance.selectUtencil(this, visitedObjects);
            if (nextOption)
            {

                target = nextOption;
            }
            else
            {
                //goto bin
                target = rubishBin;
            }
        }
        else
        {
            if (instruction != null)
            {
                var nextOption = IngredientManager.Instance.selectItem(instruction, this, visitedObjects);
                if (nextOption)
                {
                    target = nextOption;
                }
            }
        }
    }
}
