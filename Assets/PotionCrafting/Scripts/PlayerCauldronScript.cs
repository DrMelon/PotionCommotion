using UnityEngine;
using System.Collections;

// Matthew Cormack
// 29th - 19:28
// Script describing each players' potion mixing cauldron, with functionality for mixing and creating potions 

public class PlayerCauldronScript : MonoBehaviour
{
	public GameObject Debug_AddThing;
	public GameObject Debug_Potion;

	[System.Serializable]
	public struct PotionRecipeStruct
	{
		public string Name;
		public bool OrderImportant;
		public PotionIngredientScript[] Ingredients;
	}

	public PotionRecipeStruct[] PotionRecipes;
	public GameObject OwnerPlayer;
	public GameObject LiquidSurface;
	public Material Material_Colour;

    // Store the ingredients currently in the cauldron (GameObject)
    ArrayList Ingredients = new ArrayList();
	float StartLiquidY;

	void Start()
	{
		StartLiquidY = LiquidSurface.transform.localPosition.y;

		ParticleSystem liquidparticle = LiquidSurface.transform.GetChild( 0 ).GetComponent<ParticleSystem>();
		liquidparticle.GetComponent<Renderer>().material = LiquidSurface.GetComponent<Renderer>().material;
	}

	void Update()
	{
		if ( Debug_AddThing )
		{
			AddIngredient( Debug_AddThing );
			Debug_AddThing = null;
		}
		if ( Debug_Potion )
		{
			ExtractVialBrew( Debug_Potion );
			Debug_Potion = null;
		}
	}

	// Add the ingredient to the cauldron and inherit its stats
	// NOTE: Also handles adding potion vials to the brew in order to extract the current mix
	public void AddIngredient( GameObject ingredient )
	{
		PotionIngredientScript ingredientscript = ingredient.GetComponent<PotionIngredientScript>();
        PotionVialBaseScript potionvialscript = ingredient.GetComponent<PotionVialBaseScript>();
        if ( ( !ingredientscript ) && ( !potionvialscript ) ) return;

		// If the ingredient is a potion vial then different logic
		if ( potionvialscript )
		{
            ExtractVialBrew( ingredient );
            return;
		}
        else
		// Otherwise add to the brew
		{
			// Add ingredient to stored array
			Ingredients.Add( ingredientscript );

			// Increase height
			LiquidSurface.transform.localPosition += new Vector3( 0, 0.1f, 0 );

			// Blend colour
			//LiquidSurface.GetComponent<Renderer>().material.color /= 2;
			//LiquidSurface.GetComponent<Renderer>().material.color += ingredientscript.Colour;
			LiquidSurface.GetComponent<Renderer>().material.color = ingredientscript.Colour;

			// Play sound
			LiquidSurface.GetComponent<AudioSource>().Play();

			// Remove physical ingredient object
			Destroy( ingredient );
		}
	}

	// Fill the vial provided with the cauldron's brew and throw it back to the player
	void ExtractVialBrew( GameObject vial )
	{
		// Add to potion vial
		PotionVialBaseScript script = vial.GetComponent<PotionVialBaseScript>();
		foreach ( PotionIngredientScript ingredient in Ingredients )
		{
			foreach ( PotionIngredientScript.StatAlterStruct statalter in ingredient.StatAlters )
			{
				script.StatAlters.Add( statalter );
				print( statalter.Name + " x" + statalter.Multiplier );
			}
		}
		vial.GetComponent<PotionVialBaseScript>().LiquidMesh.SetActive( true );
		Renderer liquidrender = vial.GetComponent<PotionVialBaseScript>().LiquidMesh.GetComponent<Renderer>();
		liquidrender.material = Material_Colour;
		liquidrender.material.color = LiquidSurface.GetComponent<Renderer>().material.color;
		liquidrender.material.color = new Color( liquidrender.material.color.r, liquidrender.material.color.g, liquidrender.material.color.b, 1 );
		liquidrender.material.SetFloat( "_Mode", 1.0f );
		vial.GetComponent<PotionVialBaseScript>().InheritMaterial();

        // Send potion vial back
        PlayerObjectThrowScript throws = OwnerPlayer.GetComponent<PlayerObjectThrowScript>();
        throws.TargetAt = OwnerPlayer.transform;
        throws.myTransform = transform;
        throws.ThrowObjectAt();

        // Clear brew, change colour/height back
        LiquidSurface.transform.localPosition = new Vector3( 0, StartLiquidY, 0 );
		LiquidSurface.GetComponent<Renderer>().material.color = Color.white;
	}
}
