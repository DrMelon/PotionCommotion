using UnityEngine;
using System.Collections;

// Gaseous payload. Produces lil' skulls!

public class PayloadGas : PayloadBase
{

    public Object SkullCloudParticles = null;
    GameObject MyParticles = null;
    float TimeAlive = 0.0f;
    public float GasRadius = 6.0f;

    // Status effect is implied with gas explosion. If none specified, will be poison.

	// Use this for initialization
	void Start ()
    {
	
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
                Destroy(this.gameObject);
            }

            TimeAlive += Time.deltaTime;

            // Anything within the gas cloud is affected by status effects (but only while inside it?)
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject ply in players)
            {
                if (Vector3.Distance(ply.transform.position, this.transform.position) <= GasRadius)
                {

                    Debug.Log("Player hit with status-altering gas.");

                }
            }
        }

	}
        
    public override void Activate()
    {
        // This is simply used to trigger the activation of the payload, based on its type.
        if (!Activated)
        {
            MyParticles = (GameObject)Instantiate(SkullCloudParticles, this.transform.position, this.transform.rotation);
            Activated = true;
        }
    }
}
