using UnityEngine;
using System.Collections;

public class PayloadExplode : PayloadBase
{

    GameObject explosionVisuals = null;
    public Object explosionEffect = null;
    public bool StatusEffect = false; // can change this to turn on or off status effect mode
    

    // Use this for initialization
    void Start ()
    {
        // temp
        //Activate(); 
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(explosionVisuals != null && Activated)
        {
            // Based on scale of visuals (which equates to radial explosion)
            // find players inside radius.
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject ply in players)
            {
                if (Vector3.Distance(ply.transform.position, this.transform.position) <= explosionVisuals.transform.localScale.x)
                {
                    if (!StatusEffect)
                    {
                        // kill players in radius
                        Debug.Log("Player " + ply.GetComponent<PlayerMovementScript>().ControllerID + " killed!");
                    }
                    else
                    {
                        // apply status effect instead
                        Debug.Log("Player affected by status ailment!");
                    }
                }

            }
        }

        if(explosionVisuals == null && Activated)
        {
            // explosion finished, remove self.
            Destroy(this.gameObject);
        }




    }

    public override void Activate()
    {
       

        // Create Explosion Visuals
        if(!Activated)
        {
            explosionVisuals = (GameObject)Instantiate(explosionEffect, this.transform.position, this.transform.rotation);
            Activated = true;
        }
        
        
        

       
    }
}
