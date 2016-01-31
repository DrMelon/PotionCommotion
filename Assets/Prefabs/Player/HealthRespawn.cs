using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthRespawn : MonoBehaviour {

    public int playerHealth;
    private GameObject playerCanvas;
    private Image[] heartImages;

	// Use this for initialization
	void Start () {

        playerCanvas = this.transform.Find("Canvas").gameObject;
        heartImages = playerCanvas.GetComponentsInChildren<Image>();

        //3-strike system
        playerHealth = 3;

        foreach (Image heart in heartImages)
        {
            heart.enabled = false;
        }
	
	}
	
	// Update is called once per frame
	void Update () {

        //update the health UI
        foreach (Image heart in heartImages)
        {
            heart.enabled = false;
        }

        for (int i = 0; i < playerHealth; i++)
        {
            heartImages[i].enabled = true;
        }


        //respawn
        if (playerHealth <= 0)
        {
            Respawn();
        }
	}

    void Respawn()
    {
        //move the player back to the cauldron
        GameObject cauldron = GameObject.Find("PlayerCauldron " + this.gameObject.name.Substring(7));

        this.gameObject.transform.position = new Vector3(cauldron.transform.position.x, 1.5f, cauldron.transform.position.z);
        playerHealth = 3;
    }
}
