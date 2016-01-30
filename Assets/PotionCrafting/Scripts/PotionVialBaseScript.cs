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

	public ArrayList StatAlters = new ArrayList(); // (StatAlterStruct)
	// Reference to the internal liquid mesh of this vial object, for colouring
	public GameObject LiquidMesh;
    public PayloadBase PotionPayload;

	void Start()
	{
		// Instantiate a new version of the material just for this potion vial
		Renderer renderer = LiquidMesh.GetComponent<Renderer>();
		renderer.material = Instantiate( renderer.material );
	}

	public void InheritMaterial()
	{
		// Initialise the particle system for wobble effect
		ParticleSystem liquidparticle = LiquidMesh.transform.GetChild( 0 ).GetComponent<ParticleSystem>();
		liquidparticle.GetComponent<Renderer>().material = LiquidMesh.GetComponent<Renderer>().material;
		liquidparticle.Play();
	}

    public void OnCollisionEnter()
    {
        // Depending on potion type, do an explosion on collision?
        if (PotionPayload != null)
        {
            PotionPayload.Activate();
        }
    }
}
