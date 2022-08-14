using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;



public class IngredientManager : Singleton<IngredientManager>
{
    public int maxIngredient = 20;
    public float fillTime = 2;
    float fillTimer = 0;
    struct RecipeData {
        public List<string> ingredient;
        public string utencil;
        
        public RecipeData(List<string> input)
        {
            utencil = input[0];
            input.RemoveAt(0);
            ingredient = new List<string>();
            foreach(var i in input)
            {
                ingredient.Add(i);
            }
        }
    }


    static public List<string> ingredientTypes = new List<string>() { "egg", "apple","broccoli", "meat","flour","pepper"/*"cheese", "chocolate","onion", "lettuce", "pepper", "potato"*/ };
    static public List<string> InstructionTypes = new List<string>() { /*"round",*/  "red", "white", "green" };
    static public List<string> UtencilTypes = new List<string>() { "pan","pot" };

    static public Dictionary<string, List<List<string>>> recipe = new Dictionary<string, List<List<string>>>()
    {
    };
    static public Dictionary<string, List<string>> recipeByName = new Dictionary<string, List<string>>()
    {
    };
    static public Dictionary<string, List<string>> ingredientToInstructions = new Dictionary<string, List<string>>()
    {
    };
    static public Dictionary<string, List<string>> instructionToIngredients;

    Dictionary<string, int> ingredientCount = new Dictionary<string, int>();

    public bool doesIngredientHasCount(string ingre)
    {
        return ingredientCount[ingre]>0;
    }

    public string findIngredientWithoutCount()
    {
        foreach(var pair in ingredientCount)
        {
            if (pair.Value == 0)
            {
                return pair.Key;
            }
        }
        return null;
    }

    public string findIngredientWithCount()
    {
        foreach (var pair in ingredientCount)
        {
            if (pair.Value != 0)
            {
                return pair.Key;
            }
        }
        return null;
    }


    public GameObject ingredientPrefab;
    List<GameObject> ingredients;
    Utencil[] utencils;
    GameObject parent;


    public Sprite square;

    // Start is called before the first frame update
    void Start()
    {

    }

    public static Sprite getIngredientImage(string str)
    {

        var sprite = Resources.Load<Sprite>("ingredient/" + str);
        if (!sprite)
        {
            Debug.LogError("failed to find ingredient image " + str);
        }
        return sprite;
    }
    public static Sprite getIngredientImageShadow(string str)
    {

        var sprite = Resources.Load<Sprite>("ingredient/" + str+ "-shadow");
        if (!sprite)
        {
            Debug.LogError("failed to find ingredient image shadow " + str);
        }
        return sprite;
    }

    public static Sprite getUtencilImage(string str)
    {

        var sprite = Resources.Load<Sprite>("utencil/" + str);
        if (!sprite)
        {
            Debug.LogError("failed to find utencil image " + str);
        }
        return sprite;
    }
    public static AnimatorController getUtencilAnimation(string str)
    {

        var sprite = Resources.Load<AnimatorController>("utencil/" + str);
        if (!sprite)
        {
            Debug.LogError("failed to find utencil image " + str);
        }
        return sprite;
    }

    public static string cook(string utencil, List<string> ingredient)
    {
        foreach(var reci in recipe[utencil])
        {
            bool failed = false;
            for(int i = 0; i < reci.Count - 1; i++)
            {
                var requiredIngredient = reci[i];
                if (!ingredient.Contains(requiredIngredient))
                {
                    failed = true;
                    break;
                }
            }
            if (!failed)
            {
                for (int i = 0; i < ingredient.Count; i++)
                {
                    var requiredIngredient = ingredient[i];
                    if (!reci.Contains(requiredIngredient))
                    {
                        failed = true;
                        break;
                    }
                }
            }
            if (!failed)
            {
                return reci[reci.Count - 1];
            }
        }
        return null;
    }

    GameObject initIngredient(string ing)
    {
        //
        var position = new Vector3(Random.Range(-11, 16), Random.Range(-5, 8), 0);



        int test = 100;
        while (test > 0)
        {
            bool isWork = true;
            foreach (var ingre in ingredients)
            {
                if (ingre.GetComponent<Ingredient>().canPick() && ingre.transform.position == position)
                {
                    position = new Vector3(Random.Range(-11, 16), Random.Range(-5, 8), 0);
                    isWork = false;
                    break;
                }
            }
            if (position.x >= -4 && position.x <= 9 && position.y <=0)
            {
                position = new Vector3(Random.Range(-11, 16), Random.Range(-5, 8), 0);
                isWork = false;
            }
            if (isWork)
            {
                break;
            }
            test--;
        }
        if(test == 0)
        {
            return null;
        }
        var go = Instantiate(ingredientPrefab, position, Quaternion.identity,parent.transform);

        go.GetComponent<Ingredient>().init(ing);

        

        return go;
    }
    public void initBoard()
    {
        utencils = GameObject.FindObjectsOfType<Utencil>();
        ingredientToInstructions["egg"] = new List<string>() { "white" };
        ingredientToInstructions["apple"] = new List<string>() { "red" };
        ingredientToInstructions["meat"] = new List<string>() { "red" };
       // ingredientToInstructions["chocolate"] = new List<string>() { "notRound"};
        ingredientToInstructions["broccoli"] = new List<string>() {"green" };
        ingredientToInstructions["flour"] = new List<string>() { "white" };
        ingredientToInstructions["pepper"] = new List<string>() { "green" };

        recipe["pan"] = new List<List<string>>()
        {
            new List<string>(){ "egg","friedEgg" },
            new List<string>(){ "egg","meat","Ham and Egg" },
            new List<string>(){ "broccoli", "meat", "beef and broccoli" },



            new List<string>(){ "pepper", "meat", "beef and pepper" },
            new List<string>(){ "apple", "flour", "apple pie" },
            new List<string>(){ "flour", "egg", "cake" },
            new List<string>(){ "pepper", "egg", "Pepper Egg-in-a-Hole" },


            //new List<string>(){ "flour", "meat", "pizza" },

        };


        recipe["pot"] = new List<List<string>>()
        {
            new List<string>(){ "egg","boilingEgg" },
            new List<string>(){ "apple","apple jam" },

            new List<string>(){ "meat", "broccoli", "stew" },


            new List<string>(){ "pepper", "apple","wowie jam" },
            new List<string>(){ "flour", "broccoli", "vegetable Spaghetti" },
            new List<string>(){ "flour", "meat", "meat Spaghetti" },

        };

        foreach(var pair in recipe)
        {
            foreach(var actualRecipe in pair.Value)
            {
                var recipe2 = new List<string>();
                for (int i = 0; i < actualRecipe.Count - 1; i++)
                {
                    recipe2.Add(actualRecipe[i]);
                }
                recipe2.Add(pair.Key);
                recipeByName[actualRecipe[actualRecipe.Count - 1]] = recipe2;
            }
        }

        foreach(var ingre in ingredientTypes)
        {
            ingredientCount[ingre] = 0;
        }

        if (parent)
        {
            Destroy(parent);
        }
        parent = new GameObject();
            ingredients = new List<GameObject>();
        foreach(var ing in ingredientTypes)
        {
            addIngredient(initIngredient(ing));
            addIngredient(initIngredient(ing));
        }
    }

    void addIngredient(GameObject go)
    {
        if (go)
        {
            ingredients.Add(go);
            ingredientCount[(go.GetComponent<Ingredient>().ingredientType)]++ ;
        }
    }

    void addRandomIngredient()
    {
        var emptyIngredient = findIngredientWithoutCount();
        if (emptyIngredient != null)
        {
            addIngredient(initIngredient(emptyIngredient));
        }
        else
        {
            addIngredient(initIngredient(ingredientTypes[Random.Range(0, ingredientTypes.Count)]));
        }

    }

    public GameObject selectUtencil(Robot robot,List<GameObject> visited)
    {
        float shortestDistance = 100000;
        GameObject res = null;
        var robotPosition = robot.transform.position;
        foreach (var ingre in utencils)
        {
            if (visited.Contains(ingre.gameObject))
            {
                continue;
            }
            //if (ingredientToInstructions[ingre.GetComponent<Ingredient>().ingredientType].Contains(instruction))
            {
                var distance = (robotPosition - ingre.transform.position).magnitude;
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    res = ingre.gameObject;
                }
            }
        }
        visited.Add(res);
        return res;
    }

    public GameObject selectItem(string instruction, Robot robot, List<GameObject> visited)
    {
        float shortestDistance = 100000;
        GameObject res = null;
        var robotPosition = robot.transform.position;
        foreach (var ingre in ingredients)
        {
            if (visited.Contains(ingre))
            {
                continue;
            }
            if (!ingre || !ingre.GetComponent<Ingredient>())
            {
                Debug.LogError("it is destroyed "+ !ingre+" "+ !ingre.GetComponent<Ingredient>());
            }
            var ingreType = ingre.GetComponent<Ingredient>().ingredientType;
            if (!ingredientToInstructions.ContainsKey(ingreType))
            {
                Debug.LogError("ingre type not exist " + ingreType);
            }
            if (ingre.GetComponent<Ingredient>().canPick())
            {
                if (ingredientToInstructions[ingreType].Contains(instruction))
                {
                    var distance = (robotPosition - ingre.transform.position).magnitude;
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        res = ingre;
                    }
                }
            }
        }
        if (res != null)
        {
            visited.Add(res);
        }
        return res;
    }

    public void removeIngredient(GameObject go)
    {
        
        ingredientCount[go.GetComponent<Ingredient>().ingredientType]--;
        ingredients.Remove(go);
    }


    // Update is called once per frame
    void Update()
    {
        fillTimer += Time.deltaTime;
        if (fillTimer >= fillTime)
        {
            fillTimer = 0;
            if (ingredients.Count < maxIngredient)
            {

                addRandomIngredient();
            }
        }
    }
}
