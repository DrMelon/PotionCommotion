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
    public bool CheckProximity = false;
    public float ProximityRadius = 5.0f;
    public GameObject Thrower = null;
    public bool StatusEffect = false;
    public ArrayList StatusEffects = null; //PotionIngredientScript.StatAlterStruct

    // Use this for initialization
    void Start ()
    {
       
	}
	
	// Update is called once per frame
	public virtual void Update ()
    {
	    if(CheckProximity)
        {
            // Look for players that aren't our thrower, in a radius.
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject ply in players)
            {
                if(ply != Thrower)
                {
                    if (Vector3.Distance(ply.transform.position, this.transform.position) <= ProximityRadius)
                    {
                        // KABOOM!
                        Activate();
                    }
                }
            }
        }
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

    public static void ApplyStatusEffect(PotionIngredientScript.StatAlterStruct effect, PlayerControllerScript ply)
    {
        // This lets us apply the various status effects.
        if(effect.Name.Contains("MoveSpeed"))
        {
            ply.m_RunSpeed *= effect.Multiplier;
            ply.m_WalkSpeed *= effect.Multiplier;
        }
        if(effect.Name.Contains("JumpHeight"))
        {
            ply.m_JumpSpeed *= effect.Multiplier;
        }
    }

    public static void UnApplyStatusEffect(PotionIngredientScript.StatAlterStruct effect, PlayerControllerScript ply)
    {
        // Undo the status effect.
        if(effect.Name.Contains("MoveSpeed"))
        {
            ply.m_RunSpeed = 5;
            ply.m_WalkSpeed = 15;
        }
        if (effect.Name.Contains("JumpHeight"))
        {
            ply.m_JumpSpeed = 5;
        }
    }

    public static IEnumerator StatusEffectTimer(float time, PotionIngredientScript.StatAlterStruct effect, PlayerControllerScript ply)
    {
        yield return new WaitForSeconds(time);
        PayloadBase.UnApplyStatusEffect(effect, ply);

    }
}
