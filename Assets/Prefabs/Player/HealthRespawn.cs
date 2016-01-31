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

        foreach (Image heart in heartImages)
        {
            heart.enabled = false;
        }

        for (int i = 0; i < playerHealth; i++)
        {
            heartImages[i].enabled = true;
        }
	
	}
}
