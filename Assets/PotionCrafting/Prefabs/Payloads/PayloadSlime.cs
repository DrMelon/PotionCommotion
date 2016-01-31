using UnityEngine;
using System.Collections;

// Slime potion explosion!
// Basically, it just plays a particle effect and spawns a ton of Slime Floor Objects. 
// The floor objects have logic for passing on their status effects to players standing on them.

public class PayloadSlime : PayloadBase
{

    public Object SlimeFloorObject;
    public Object SlimeCloudParticles;
    GameObject MyParticles;
    public int NumSplats = 10;
    float TimeAlive = 0.0f;


    // Use this for initialization
    void Start()
    {
        SlimeFloorObject = Resources.Load("Effects/SlimeSplot");
        SlimeCloudParticles = Resources.Load("Effects/SlimeSplosion");
    }

    // Update is called once per frame
    public override void Update()
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

            // Slime objects take care of themselves.
        }

    }

    public override void Activate()
    {
        // This is simply used to trigger the activation of the payload, based on its type.
        if (!Activated)
        {
            MyParticles = (GameObject)Instantiate(SlimeCloudParticles, this.transform.position, Quaternion.LookRotation(Vector3.up));

            for(int i = 0; i < NumSplats; i++)
            {
                // Create splats!
                Vector3 randOffset = new Vector3(Random.Range(-3.0f, 3.0f), 0, Random.Range(-3.0f, 3.0f));
                GameObject thisSplat = (GameObject)Instantiate(SlimeFloorObject, (this.transform.position + randOffset), Quaternion.identity);
                thisSplat.transform.localScale = new Vector3(10, 10, 10);
            }

            Activated = true;
        }
    }
}
