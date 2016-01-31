using UnityEngine;
using System.Collections;

// Gaseous payload. Produces lil' skulls!

public class PayloadGas : PayloadBase
{

    public Object SkullCloudParticles;
    GameObject MyParticles;
    float TimeAlive = 0.0f;
    public float GasRadius = 6.0f;

    // Status effect is implied with gas explosion. If none specified, will be poison.

	// Use this for initialization
	void Start ()
    {
        SkullCloudParticles = Resources.Load("Effects/Poison Cloud");
	}
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();

        if (Activated)
        {
            // particles are active!
            if (TimeAlive > MyParticles.GetComponent<ParticleSystem>().duration)
            {
                // If time is up, destroy self.
                MyParticles.GetComponent<SelfDestroy>().InitSelfDestruct(5.0f);
                Destroy(this.gameObject);
            }

            TimeAlive += Time.deltaTime;

            // Anything within the gas cloud is affected by status effects (but only while inside it?)
            GameObject[] players = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject ply in players)
            {
                if (ply.tag.Contains("Player"))
                {
                    if (Vector3.Distance(ply.transform.position, this.transform.position) <= GasRadius)
                    {

                        Debug.Log("Player hit with status-altering gas.");

                    }
                }
            }
        }

	}
        
    public override void Activate()
    {
        // This is simply used to trigger the activation of the payload, based on its type.
        if (!Activated)
        {
            MyParticles = (GameObject)Instantiate(SkullCloudParticles, this.transform.position, Quaternion.LookRotation(Vector3.up));
            
            Activated = true;
        }
    }


}
