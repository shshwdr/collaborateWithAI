using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    public bool shoulShowTutorial = true;
    Dictionary<string,string> tutorialString = new Dictionary<string, string>()
    {
        {"first_0","You are preparing meal to children\nYou are assisted by monsters who collect the ingredients for the dishes and carry them to the kitchen." },
        {"first_00","The little guys are not exactly the smartest.\nThey only recognize an ingredient by its color - and so they can sometimes mess things up. "},
       {"first_000", "When a monster try to collect the wrong ingredient, you can correct him with a Slap \n(left click)"},


        {"second_0","Congratulation! \nYou cooked your first dish! \nCooked food would send to the customer automatically."},
        {"second_00","You can slap(left click) to make monster move to next utensil.\n Different dish requires different utensil!"},
        {"second_000","If you picked an ingredient that you don't need, you can slap(left click) to skip both utensil and drop it to the bin."},


        {"third_0","Since the little guys are quite leisurely on the move, you can also drive them for a short time by yelling Run at them \n(right mouse button)."},


        {"second2_0","The icons on monster show what ingredient he should find and the color he is finding."},


        {"second3_0","To make it a little easier to prepare the right ingredients in the right place, there is a small note on each utensil with ingredients that match one of the current orders."},
        {"second3_00","But you don't have to follow it, do dishes any order you want! Just make sure each customer has a patience bar..."},



        {"throw","When there are two ingredients in a utensil and not able to cook for an order, they would throw to the garbage bin. \nYou can click the utensil to manually remove ingredients too."},

    };

    Dictionary<string, bool> isTutorialFinished = new Dictionary<string, bool>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void showTutorial(string name)
    {
        if (!shoulShowTutorial)
        {
            return;
        }

        if (isTutorialFinished.ContainsKey(name))
        {
            return;
        }
        if (!tutorialString.ContainsKey(name))
        {
            Debug.LogError("no " + name);
            return;
        }
            GameObject.FindObjectOfType<Popup>(true).Init(tutorialString[name],()=>{
                isTutorialFinished[name] = true;
                StartCoroutine(show(name));
        });

            GameObject.FindObjectOfType<Popup>(true).showView();
    }

    IEnumerator show(string name)
    {
        yield return new WaitForSeconds(0.3f);
        if (tutorialString.ContainsKey(name + "0"))
        {
            showTutorial(name + "0");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
