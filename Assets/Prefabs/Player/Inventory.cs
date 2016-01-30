using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    //player can only have one item at a time
    public string selectedItem;
    private bool menuActive;
    private int menuPos;
    private bool leftPressed;
    private bool rightPressed;

    public List<string> potionInventory;
    public List<string> ingredientInventory;
    private bool potionInvTrue;

    //get the player's canvas
    private GameObject playerCanvas;
    public Image[] itemImages;
    public Text[] itemText;

	// Use this for initialization
	void Start () {

        menuActive = false;
        menuPos = 0;
        leftPressed = false;
        rightPressed = false;

        //Initalise the two inventories
        potionInventory = new List<string>();
        ingredientInventory = new List<string>();
        potionInvTrue = false;

        //get the canvas
        playerCanvas = GameObject.Find("InventoryCanvas") as GameObject;
        //get the images
        itemImages = playerCanvas.GetComponentsInChildren<Image>();
        itemText = playerCanvas.GetComponentsInChildren<Text>();

        playerCanvas.SetActive(false);

        //Inventories start off empty
        for (int i = 0; i < 4; i++)
        {
            ingredientInventory.Add("EMPTY");
            potionInventory.Add("EMPTY");
        }


        //TEMP -- fill the player inventory with ingredients
        ingredientInventory[0] = "Rocks";
        ingredientInventory[1] = "Pizza";
        ingredientInventory[2] = "Herbs";

        potionInventory[0] = "Explosive";
        potionInventory[1] = "Gas";
        potionInventory[2] = "Proximity";



	}

    // Update is called once per frame
    void Update()
    {
        //Potions - Left Bumper
        if (Input.GetButtonDown("P1_Button_Potion_Inventory"))
        {
            //print("potions");
            potionInvTrue = true;
            ShowMenu(potionInventory);
            itemText[3].text = "POTIONS";
        }
        //Ingredients - Right Bumper
        if (Input.GetButtonDown("P1_Button_Ingredient_Inventory"))
        {
            //print("ingredients");
            potionInvTrue = false;
            ShowMenu(ingredientInventory);
            itemText[3].text = "INGREDIENTS";
        }

        //Hide the menu again
        if ((Input.GetButtonUp("P1_Button_Potion_Inventory")) || (Input.GetButtonUp("P1_Button_Ingredient_Inventory")))
        {
            HideMenu();
        }



        //scrolling
        if (menuActive == true)
        {
            //scroll right
            if (Input.GetAxis("P1_Move_X") > 0)
            {
                if (rightPressed == false)
                {
                    //increase menu position
                    menuPos += 1;

                    //check which menu is currently active
                    if (potionInvTrue == true)
                    {
                        WriteMenu(potionInventory);
                    }
                    else if (potionInvTrue == false)
                    {
                        WriteMenu(ingredientInventory);
                    }
                    
                    //deactivate the axis stuff
                    rightPressed = true;
                    leftPressed = false;
                }
            }
            //scroll left
            if (Input.GetAxis("P1_Move_X") < 0)
            {
                if (leftPressed == false)
                {
                    //decrease menu position
                    menuPos -= 1;

                    //check which menu is currently active
                    if (potionInvTrue == true)
                    {
                        WriteMenu(potionInventory);
                    }
                    else if (potionInvTrue == false)
                    {
                        WriteMenu(ingredientInventory);
                    }

                    //deactivate the axis stuff
                    leftPressed = true;
                    rightPressed = false;
                }
            }
            if (Input.GetAxis("P1_Move_X") == 0)
            {
                //deactivate axis stuff
                leftPressed = false;
                rightPressed = false;
            }
        }
	
	}

    void ShowMenu(List<string> inventory)
    {
        menuActive = true;
        menuPos = 0;
        playerCanvas.SetActive(true);
        WriteMenu(inventory);
        InstantiateMenuItem(1);
    }

    void HideMenu()
    {
        menuActive = false;
        menuPos = 0;
        playerCanvas.SetActive(false);

        GameObject[] itemModels = playerCanvas.GetComponentsInChildren<GameObject>();
        foreach (GameObject item in itemModels)
        {
            Destroy(item);
        }

    }

    
    void WriteMenu(List<string> inventory)
    {
        //handle exceptions
        if (menuPos > 3)
        {
            menuPos = 0;
        }
        if (menuPos < 0)
        {
            menuPos = 3;
        }

        //write the first item
        itemText[0].text = inventory[menuPos];

        //different cases for looping thru the list. Sorry this sucks lol
        if (menuPos == 3)
        {
            itemText[1].text = inventory[menuPos - 1];
            itemText[2].text = inventory[menuPos - 2];
        }
        else if (menuPos == 2)
        {
            itemText[1].text = inventory[menuPos + 1];
            itemText[2].text = inventory[menuPos - 2];
        }
        else
        {
            itemText[1].text = inventory[menuPos + 1];
            itemText[2].text = inventory[menuPos + 2];
        }
    }

    void InstantiateMenuItem(int menuPos)
    {

        //instantiates an object
        //TODO -------------------------------------------------<<<<<<<<<<<<<<<<<<<
        //make this a child of the image
        Vector3 itemPos = new Vector3(0, -70, 30);
        Vector3 itemScale = new Vector3(3000, 3000, 3000);

        GameObject displayItem = (GameObject)Instantiate(Resources.Load(itemText[menuPos].text), itemPos, Quaternion.identity);

        displayItem.transform.localScale.Set(itemScale.x, itemScale.y, itemScale.z);
    }

    void SelectItem()
    {
        //place the item in the player's hands
    }

}
