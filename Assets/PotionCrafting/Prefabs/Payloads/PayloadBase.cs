using UnityEngine;
using System.Collections;

// The payload object is what drives the potion's explosion.
// A potion can be given many different payload types.
// The effect applied by the payload is given by the potion's type/multiplier.

// This base class isn't really intended for use ingame. Use one of the many derived types instead.


public class PayloadBase : MonoBehaviour
{

    public PotionVialBaseScript myParentPotion; // This lets us track the potion that we came from, to apply its effects.
    public bool Activated = false;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public virtual void Activate()
    {
        // This is simply used to trigger the activation of the payload, based on its type.
        if(!Activated)
        {
            Activated = true;
        }
        else
        {
            return;
        }
    }
}
