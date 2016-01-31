using UnityEngine;
using System.Collections;

public class PayloadBomberman : PayloadBase
{

    GameObject explosionVisuals;
    public Object explosionEffect;
    


    // Use this for initialization
    void Start()
    {
        explosionEffect = Resources.Load("Effects/XPlosion");
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (explosionVisuals != null && Activated)
        {
            // Based on scale of visuals (which equates to radial explosion)
            // find players inside radius.
            GameObject[] players = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject ply in players)
            {
                if (ply.tag.Contains("Player") && ply.GetComponent<Collider>() != null)
                {
                    // Check the two capsule colliders.
                    CapsuleCollider[] colliders = explosionVisuals.GetComponentsInChildren<CapsuleCollider>();
                    foreach (CapsuleCollider collider in colliders)
                    {
                        if (collider.bounds.Intersects(ply.GetComponent<Collider>().bounds))
                        {
                            if (!StatusEffect)
                            {
                                // kill players in radius
                                
                            }
                            else
                            {
                                // apply status effect instead
                                
                            }
                        }
                    }

                }

            }
        }

        if (explosionVisuals == null && Activated)
        {
            // explosion finished, remove self.
            Destroy(this.transform.parent.gameObject);
        }




    }

    public override void Activate()
    {


        // Create Explosion Visuals
        if (!Activated)
        {
            explosionVisuals = (GameObject)Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
            Activated = true;
        }

    }
}
