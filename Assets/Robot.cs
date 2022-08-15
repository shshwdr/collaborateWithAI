using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public Image currentIngredientImage;

    float pickupRange = 0.1f;
    public string idealTarget = null;


    public ChatBox chatObject;


    float speedupScale = 2;
    bool isSpeedUp;
    public float speedupTime = 3;
    float speedupTimer = 0;

    LineRenderer path;

    Animator animator;

    public List<string> currentWorkingIngredient()
    {
        List<string> res = new List<string>();
        if (holdIngredent)
        {
            res.Add( holdIngredent.GetComponent<Ingredient>().ingredientType);
        }
        if (idealTarget!=null && idealTarget!="")
        {
            res.Add( idealTarget);
        }
        return res;
    }

    // Start is called before the first frame update
    public void init()
    {
        path = GetComponentInChildren<LineRenderer>();
        rubishBin = GameObject.FindObjectOfType<RubishBin>().gameObject;
        animator = GetComponentInChildren<Animator>();
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

    // Update is called once per frame
    void Update()
    {

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {

                if (isClick())
                {
                    slap();
                }

            }

            if (Input.GetMouseButtonDown(1))
            {

                if (isClick())
                {
                    if (!isSpeedUp)
                    {

                        speedUpSlap();
                    }
                }

            }
        }

        if (isSpeedUp)
        {
            speedupTimer += Time.deltaTime;
            if (speedupTimer > speedupTime)
            {
                speedupTimer = 0;
                stopSpeedUp();
            }
        }
        
        //target can be ingredient or utencil(bin)
        if (target)
        {
            path.SetPosition(0, transform.position);
            path.SetPosition(1, target.transform.position);


            if (!holdIngredent) //target should be ingredient
            {
                if (!target.GetComponent<Ingredient>())
                {
                    Debug.LogError("when not holding, target is not ingredient either");
                    target = null;
                    return;
                }
                // if target is taken, complain and find next with same instruction
                if (!target.GetComponent<Ingredient>().canPick())
                {
                    chatObject.show("Go away, that's my stuff!");
                    decideNextIngredient();
                }
            }
            //if arrive
            if ((target.transform.position - transform.position).magnitude < pickupRange)
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
                        //if the utensil is busy, go to next one
                        if (!target.GetComponent<Utencil>())
                        {
                            Debug.LogError("target is not utencil");
                        }
                        if (!target.GetComponent<Utencil>().addIngredient(holdIngredent))
                        {
                            chatObject.show("This is in use");
                            updateToNextUtensil();
                            return;
                        }
                    }

                    IngredientManager.Instance.removeIngredient(holdIngredent);

                    holdIngredent = null;
                    target = null;


                    visitedObjects = new List<GameObject>();

                    smartDecideNextIngredientSelection();
                    startNextIngredientSelection();
                    decideNextIngredient();
                }
                else
                {
                    //pickup item
                    SFXManager.Instance.playCollectIngredient();
                    target.GetComponent<Ingredient>().pick();
                    holdIngredent = target;
                    target.transform.parent = transform;
                    target = null;
                    idealTarget = null;
                    target = IngredientManager.Instance.selectUtencil(this, visitedObjects);

                    //hide the chip on top of monster
                    currentInstructionImage.transform.parent.gameObject.SetActive(false);
                }
            }
            else
            {

                var dir = (target.transform.position - transform.position).normalized;
                transform.Translate(dir * moveSpeed * Time.deltaTime * (isSpeedUp?speedupScale:1));
            }
        }
        else
        {

            visitedObjects = new List<GameObject>();
            decideNextIngredient();
        }
    }

    public void smartSelectInstruction()
    {
        smartDecideNextIngredientSelection();
        startNextIngredientSelection();
    }

    void smartDecideNextIngredientSelection()
    {
        var nextIngredient = OrderManager.Instance.findNextIngredient_v2();
        if(nextIngredient == null)
        {
            return;
        }
        var nextInstruction = IngredientManager.ingredientToInstructions[nextIngredient][0];

        idealTarget = nextIngredient;
        instruction = nextInstruction;
        Sprite image = Resources.Load<Sprite>("instruction/" + nextInstruction);
        if (!image)
        {
            Debug.Log(" no instruction image " + nextInstruction);
        }
        currentInstructionImage.sprite = image;
        currentInstructionImage.transform.parent.gameObject.SetActive(true);
        currentIngredientImage.sprite = IngredientManager.getIngredientImage(nextIngredient);
    }

    //public void randomeSelectInstruction()
    //{

    //    selectInstruction(IngredientManager.InstructionTypes[Random.Range(0, IngredientManager.InstructionTypes.Count)]);
    //}

    public void startNextIngredientSelection()
    {

        visitedObjects = new List<GameObject>();
        decideNextIngredient();
    }
    void decideNextIngredient()
    {
        // find the closest item matches the instruction

        if (holdIngredent)
        {
            return;
        }

        var selectItem = IngredientManager.Instance.selectItem(instruction, this, visitedObjects);
        //if (selectItem)
        {
            target = selectItem;
        }

    }

    List<GameObject> visitedObjects = new List<GameObject>();

    void updateToNextUtensil()
    {
        if (target.GetComponent<RubishBin>())
        {
            //if is bin, revisit
            visitedObjects.Clear();
        }

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

    void slap()
    {


        SFXManager.Instance.playslap();
        SFXManager.Instance.playOuch();
        if (holdIngredent)
        {
            updateToNextUtensil();
        }
        else
        {
            if (instruction != null)
            {
                decideNextIngredient();
            }
            else
            {
                Debug.LogError("when slap, ingredient instruction is null");
            }
        }

        var go = Instantiate(slapObject, transform.position, Quaternion.identity, transform);


        List<string> hurryWords = new List<string>()
        {
            "Not this one? Ok..",
            "On my way!",
            "They look the same!",
            

        };
        if (Random.Range(0, talkPossibility) == 0)
        {

            var words = hurryWords[Random.Range(0, hurryWords.Count)];
            chatObject.show(words);
        }
    }

    void speedUpSlap()
    {

        SFXManager.Instance.playHurry();
        //SFXManager.Instance.playOuch();

        isSpeedUp = true;
        animator.speed = speedupScale;

        var go = Instantiate(slapObject,transform.position,Quaternion.identity,transform);
        go.GetComponent<Slap>().useHurry();

        List<string> hurryWords = new List<string>()
        {
            "I know I know",
            "Don't push me!",
            "I'm on fire!",

        };
        if (Random.Range(0, talkPossibility) ==0)
        {

            var words = hurryWords[Random.Range(0, hurryWords.Count)];
            chatObject.show(words);
        }

    }
    public int talkPossibility = 5;
    void stopSpeedUp()
    {

        isSpeedUp = false;
        animator.speed = 1;

    }

    public GameObject slapObject;
}
