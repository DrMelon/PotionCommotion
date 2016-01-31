using UnityEngine;
using System.Collections;

// Matthew Cormack
// 29th - 20:19
// Script describing the basic information of a potion vial

public class PotionVialBaseScript : MonoBehaviour
{
	// Must be inside class to serialize into inspector (also in PotionIngredientScript)
	[System.Serializable]
	public struct StatAlterStruct
	{
		public string Name;
		public float Multiplier;
	}
    // Type of potion activation
    public enum PotionActivationType
    {
        ACT_CONTACT,
        ACT_TIMER,
        ACT_PROXIMITY,
        ACT_BOUNCE,
        ACT_REMOTE
    }

	public ArrayList StatAlters = new ArrayList(); // (StatAlterStruct)
	// Reference to the internal liquid mesh of this vial object, for colouring
	public GameObject LiquidMesh;
    public PayloadBase PotionPayload;

    // Each thrown potion knows who it belongs to.
    public GameObject BelongTo = null; 

    // Potions default to timer activation.
    public PotionActivationType ActivateMethod = PotionActivationType.ACT_TIMER;
    int RemainingBounces = 5; // 5 bounces for ACT_BOUNCE potions.

	void Start()
	{
		// Instantiate a new version of the material just for this potion vial
		Renderer renderer = LiquidMesh.GetComponent<Renderer>();
		renderer.material = Instantiate( renderer.material );
	}

	public void InheritMaterial()
	{
		// Initialise the particle system for wobble effect
		//ParticleSystem liquidparticle = LiquidMesh.transform.GetChild( 0 ).GetComponent<ParticleSystem>();
		//liquidparticle.GetComponent<Renderer>().material = LiquidMesh.GetComponent<Renderer>().material;
		//liquidparticle.Play();
	}

    public void OnCollisionEnter(Collision collisionInfo)
    {
        // Don't forget to set the payload's thrower too!
        if(PotionPayload != null)
        {
            PotionPayload.Thrower = BelongTo;
        }

        // ACT_CONTACT potions explode as soon as they make contact physically with something, provided that what they
        // contact is not their own thrower!
        if (PotionPayload != null && ActivateMethod == PotionActivationType.ACT_CONTACT)
        {
            if(collisionInfo.collider.gameObject != BelongTo)
            {
                PotionPayload.Activate();
            }
            
        }

        // ACT_TIMER potions explode after a set time!
        if(PotionPayload != null && ActivateMethod == PotionActivationType.ACT_TIMER)
        {
            StartCoroutine(DelayedDetonation(3.0f));
        }

        // ACT_BOUNCE potions explode after they've used up all their bounces! (Bounces against their thrower don't count.)
        if(PotionPayload != null && ActivateMethod == PotionActivationType.ACT_BOUNCE)
        {
            if (collisionInfo.collider.gameObject != BelongTo)
            {
                RemainingBounces--;
                // Make sure to bounce higher!
                
                if (RemainingBounces <= 0)
                {
                    PotionPayload.Activate();
                }
            }
        }

        // ACT_PROXIMTY potions don't explode until they're close to a player. The collision will "arm" them.
        if(PotionPayload != null && ActivateMethod == PotionActivationType.ACT_PROXIMITY)
        {
            PotionPayload.CheckProximity = true;

        }

        // ACT_REMOTE potions don't explode until they're told to by a remote detonator.
        // Here, the collision will spawn a detonator object. 
        // [TODO]
        
    }

    IEnumerator DelayedDetonation(float time)
    {
        yield return new WaitForSeconds(time);
        PotionPayload.Activate();
    }
}
