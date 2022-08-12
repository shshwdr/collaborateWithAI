using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    public GameObject instructionSelectionPanel;
    string instruction;
    GameObject target;
    public float moveSpeed;
    GameObject holdIngredent;

    float pickupRange = 0.1f;

    bool isSelecting = false;

    LineRenderer path;
    // Start is called before the first frame update
    void Start()
    {
        path = GetComponentInChildren<LineRenderer>();
        instructionSelectionPanel.SetActive(isSelecting);
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
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isClick())
            {
               // Debug.Log("slap");
                slap();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (isClick())
            {
                //Debug.Log("right");
                isSelecting = !isSelecting;
                instructionSelectionPanel.SetActive(isSelecting);
            }
        }

        if (isSelecting)
        {
            return;
        }

        if (target)
        {
            path.SetPosition(0, transform.position);
            path.SetPosition(1, target.transform.position);

            if ((target.transform.position - transform.position).magnitude<pickupRange)
            {
                if (holdIngredent)
                {
                    //drop ingredient and move to next

                    target.GetComponent<Utencil>().addIngredient(holdIngredent);

                    holdIngredent = null;
                    target = null;
                    decideNextTarget();
                }
                else
                {
                    //pickup item
                    target.GetComponent<Ingredient>().pick();
                    holdIngredent = target;
                    target.transform.parent = transform;
                    target = IngredientManager.Instance.selectUtencil(this,visitedObjects);
                }
            }
            else
            {

                var dir = (target.transform.position - transform.position).normalized;
                transform.Translate(dir * moveSpeed * Time.deltaTime);
            }
        }
    }

    public void selectInstruction(string ins)
    {
        instruction = ins;
        visitedObjects = new List<GameObject>();
        decideNextTarget();


        isSelecting = !isSelecting;
        instructionSelectionPanel.SetActive(isSelecting);
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

    List<GameObject> visitedObjects;

    void slap()
    {
        if (holdIngredent)
        {
            var nextOption = IngredientManager.Instance.selectUtencil(this, visitedObjects);
            if (nextOption)
            {

                target = nextOption;
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
