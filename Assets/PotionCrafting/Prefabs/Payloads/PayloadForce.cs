using UnityEngine;
using System.Collections;

public class PayloadForce : PayloadBase
{

    GameObject explosionVisuals;
    public Object explosionEffect;
    
    // Force explosions don't use status effects.


    // Use this for initialization
    void Start()
    {
        explosionEffect = Resources.Load("Effects/ForceExplosion");
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
                if (ply.tag.Contains("Player"))
                {
                    if (Vector3.Distance(ply.transform.position, this.transform.position) <= explosionVisuals.transform.localScale.x)
                    {
                        // Force explosions push players away.

                        // if(ply.GetComponent<PlayerMovementScript>() != null)
                        //{
                            //// Force player away
                            //ply.GetComponent<PlayerMovementScript>().Velocity = (this.transform.position - ply.transform.position);
                        //}

                        //if(ply.GetComponent<Rigidbody>() != null)
                        //{
                           
                            //ply.GetComponent<Rigidbody>().AddExplosionForce(15.0f, this.transform.position, explosionVisuals.transform.localScale.x);
                        //}

                        // Can't directly access character controller's speeds?
                        if(ply.GetComponent<PlayerControllerScript>() != null)
                        {
                            //ply.GetComponent<PlayerControllerScript>().m_MoveDir etc etc
                        }
                    }
                }

            }
        }

        if (explosionVisuals == null && Activated)
        {
            // explosion finished, remove self.
            Destroy(this.gameObject);
        }




    }

    public override void Activate()
    {


        // Create Explosion Visuals
        if (!Activated)
        {
            explosionVisuals = (GameObject)Instantiate(explosionEffect, this.transform.position, this.transform.rotation);
            Activated = true;
        }

    }
}
