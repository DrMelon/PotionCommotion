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

	void Start()
	{
		// Instantiate a new version of the material just for this potion vial
		Renderer renderer = LiquidMesh.GetComponent<Renderer>();
		renderer.material = Instantiate( renderer.material );
	}
}
