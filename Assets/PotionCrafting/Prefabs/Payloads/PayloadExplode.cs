using UnityEngine;
using System.Collections;

public class PayloadExplode : PayloadBase
{

    GameObject explosionVisuals;
    public Object explosionEffect;
    
    

    // Use this for initialization
    void Start ()
    {
        explosionEffect = Resources.Load("Effects/Explosion");
    }
	
	// Update is called once per frame
	public override void Update()
    {
        base.Update();

        if(explosionVisuals != null && Activated)
        {
            // Based on scale of visuals (which equates to radial explosion)
            // find players inside radius.
            GameObject[] players = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject ply in players)
            {
                if (ply.tag.Contains("Player"))
                {
                    if (Vector3.Distance(ply.transform.position, this.transform.position) <= explosionVisuals.transform.localScale.x)
                    {
                        if (!StatusEffect)
                        {
                            // hurt players in radius
                            if(ply.GetComponent<HealthRespawn>() != null)
                            {
                                ply.GetComponent<HealthRespawn>().HurtPlayer();
                            }
                        }
                        else
                        {
                            // apply status effect instead
                            foreach(PotionIngredientScript.StatAlterStruct effect in StatusEffects)
                            {
                                if(ply.GetComponent<PlayerControllerScript>() != null)
                                {
                                    PayloadBase.ApplyStatusEffect(effect, ply.GetComponent<PlayerControllerScript>());
                                    StartCoroutine(PayloadBase.StatusEffectTimer(1.0f, effect, ply.GetComponent<PlayerControllerScript>()));
                                }
                            }
                        }
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
