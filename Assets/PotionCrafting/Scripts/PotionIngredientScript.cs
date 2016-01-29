using UnityEngine;
using System.Collections;

// Matthew Cormack
// 29th - 19:41
// Script describing each possible potion ingredient

public struct StatAlterStruct
{
	public string Name;
	public float Multiplier;
}

public class PotionIngredientScript : MonoBehaviour
{
	// Must be inside class to serialize into inspector (also in PotionVialBaseScript)
	[System.Serializable]
	public struct StatAlterStruct
	{
		public string Name;
		public float Multiplier;
	}

	public string Name;
	public StatAlterStruct[] StatAlters;
	public Color Colour;
}
