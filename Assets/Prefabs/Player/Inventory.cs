using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    //player can only have one item at a time
    public GameObject selectedItem;
    public GameObject selectedSlot;
    private bool menuActive;
    private int menuPos;
    private bool leftPressed;
    private bool rightPressed;
	// Reference to the player's camera
	public GameObject LinkedCamera;

    public List<string> potionInventory;
    public List<string> ingredientInventory;
    private bool potionInvTrue;

    //get the player's canvas
    private GameObject playerCanvas;
    public Image[] itemImages;
    public Text[] itemText;
    public Transform[] itemModels;
	// For lerping the player's camera towards and away from the inventory menu
	private Vector3 CameraZoomStop;

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
        itemModels = playerCanvas.GetComponentsInChildren<Transform>();

        playerCanvas.SetActive(false);

        //Inventories start off empty
        for (int i = 0; i < 4; i++)
        {
            ingredientInventory.Add("EMPTY");
            potionInventory.Add("EMPTY");
        }


        //TEMP -- fill the player inventory with ingredients
        ingredientInventory[0] = "Tomato";
        ingredientInventory[1] = "Tin Can";
        ingredientInventory[2] = "Newt Eye";
        ingredientInventory[3] = "Vial_Explosive";

        potionInventory[0] = "Explosive";
        potionInventory[1] = "Proximity";
        potionInventory[2] = "Special";



	}

    // Update is called once per frame
    void Update()
    {
		int playerid = GetComponent<PlayerMovementScript>().ControllerToPlayerID;

        //make sure the menu always faces the camera
		if ( LinkedCamera == null ) return;
		playerCanvas.transform.rotation = Quaternion.LookRotation( -LinkedCamera.transform.GetChild( 0 ).forward );

        if (menuActive == true)
        {
            RotateItems();
        }

        //rotate selected item
        if (selectedItem != null)
        {
            selectedItem.transform.localEulerAngles = new Vector3(selectedItem.transform.localEulerAngles.x, selectedItem.transform.localEulerAngles.y + (20.0f * Time.deltaTime), selectedItem.transform.localEulerAngles.z);
        }


        //Potions - Left Bumper
		if ( Input.GetKeyDown( "joystick " + playerid + " button 4" ) )
        {
            //print("potions");
            potionInvTrue = true;
            ShowMenu(potionInventory);
            itemText[3].text = "POTIONS";
        }
		//Ingredients - Right Bumper
		if ( Input.GetKeyDown( "joystick " + playerid + " button 5" ) )
        {
            //print("ingredients");
            potionInvTrue = false;
            ShowMenu(ingredientInventory);
			itemText[3].text = "INGREDIENTS";
        }

        //Hide the menu again
		if ( ( Input.GetKeyUp( "joystick " + playerid + " button 4" ) ) || ( Input.GetKeyUp( "joystick " + playerid + " button 5" ) ) )
        {
			HideMenu();
        }



        //scrolling
        if (menuActive == true)
        {
			//scroll right
			if ( Input.GetAxis( "P" + playerid + "_Move_X" ) > 0 )
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
			if ( Input.GetAxis( "P" + playerid + "_Move_X" ) < 0 )
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
			if ( Input.GetAxis( "P" + playerid + "_Move_X" ) == 0 )
            {
                //deactivate axis stuff
                leftPressed = false;
                rightPressed = false;
            }
        }


        //item selection
        if (menuActive == true)
        {
			if ( Input.GetKeyDown( "joystick " + playerid + " button 0" ) )
            {
                SelectItem();
			}
			if ( Input.GetKeyDown( "joystick " + playerid + " button 1" ) )
            {
                DeselectAllItems();
            }
        }
	}

    void ShowMenu(List<string> inventory)
    {
        menuActive = true;
        menuPos = 0;
        playerCanvas.SetActive(true);

		// Disable movement for inventory selection
		GetComponent<PlayerMovementScript>().enabled = false;
        GetComponent<PlayerObjectThrowScript>().enabled = false;
        if ( selectedItem != null )
        {
            selectedItem.SetActive( false );
        }

        // Zoom the camera towards the menu
        LinkedCamera.GetComponent<PlayerCameraFollowScript>().Height = 7.5f;
		//LinkedCamera.GetComponent<PlayerCameraFollowScript>().Angle = -10;

        //write the text
        WriteMenu(inventory);
    }

    void HideMenu()
    {
        DeleteMenuItems();
        menuActive = false;
        menuPos = 0;
        playerCanvas.SetActive(false);

		// Reenable player's movement
		GetComponent<PlayerMovementScript>().enabled = true;
        GetComponent<PlayerObjectThrowScript>().enabled = true;
        if ( selectedItem != null )
        {
            selectedItem.SetActive( true );
        }

        // Zoom the camera back out
        LinkedCamera.GetComponent<PlayerCameraFollowScript>().Height = 0;
		LinkedCamera.GetComponent<PlayerCameraFollowScript>().Angle = 0;
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
            itemText[1].text = inventory[menuPos - 3];
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


        //display the models
        DeleteMenuItems();
        for (int i = 0; i < 3; i++)
        {
            InstantiateMenuItem(i);
        }
    }

    void InstantiateMenuItem(int pos)
    {
        //instantiates an object
        //TODO -------------------------------------------------<<<<<<<<<<<<<<<<<<<
        //make this a child of the image
        Vector3 itemPos = new Vector3(0, -45, 40);
        Vector3 itemScale = new Vector3(3000, 3000, 3000);

        GameObject displayItem = (GameObject)Instantiate(Resources.Load(itemText[pos].text), new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

        displayItem.transform.SetParent(itemImages[pos].transform);

        displayItem.transform.localPosition = new Vector3(itemPos.x, itemPos.y, itemPos.z);
        displayItem.transform.localScale = new Vector3(itemScale.x, itemScale.y, itemScale.z);
        displayItem.transform.localRotation = new Quaternion (Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z, Quaternion.identity.w);
		char player = gameObject.name.ToCharArray()[gameObject.name.Length - 1];
		foreach ( Transform child in displayItem.GetComponentsInChildren<Transform>() )
		{
			child.gameObject.layer = LayerMask.NameToLayer( "Player" + (char) player );
		}
    }

    void DeleteMenuItems()
    {
        //delete the menu items
        itemModels = playerCanvas.GetComponentsInChildren<Transform>();
        foreach (Transform item in itemModels)
        {
            if (item != null)
            {
                if ((item.gameObject.tag == "Potion") || (item.gameObject.tag == "Ingredient"))
                {
                    Destroy(item.gameObject);
                }
            }
        }
    }

    void SelectItem()
    {
        //delete old object
        DeselectAllItems();
        

        //place the item in the player's hands
        selectedItem = (GameObject)Instantiate(Resources.Load(itemText[1].text), new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

        selectedItem.transform.SetParent(selectedSlot.transform);
        selectedItem.transform.localPosition = new Vector3(0.0f, 0.3f, 0.0f);
        selectedItem.transform.localScale = new Vector3(30, 30, 30);
        selectedItem.transform.localRotation = new Quaternion(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z, Quaternion.identity.w);
    }

    void DeselectAllItems()
    {
        //delete old object
        if (selectedSlot.transform.childCount > 0)
        {
            Destroy(selectedSlot.transform.GetChild(0).gameObject);
        }
    }

    void RotateItems()
    {
        //delete the menu items
        itemModels = playerCanvas.GetComponentsInChildren<Transform>();
        foreach (Transform item in itemModels)
        {
            if (item != null)
            {
                if ((item.gameObject.tag == "Potion") || (item.gameObject.tag == "Ingredient"))
                {
                    item.localEulerAngles = new Vector3(item.localEulerAngles.x, item.localEulerAngles.y + (20.0f * Time.deltaTime), item.localEulerAngles.z);
                }
            }
        }
    }

    //TODO ------------------------------------------<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Ingredient")
        {
            print ("cool");
            //add item to inventory if there's space
            foreach (string item in ingredientInventory)
            {
                if (item == "EMPTY")
                {
                    //item = collider.gameObject.name;
                }
            }
        }
    }

}
