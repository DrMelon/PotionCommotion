using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Matthew Cormack
// 29th - 23:38
// Allows the player to throw the object forward/in to the cauldron

public class PlayerObjectThrowScript : MonoBehaviour
{
	public Transform Target;
	public Transform TargetAt;
	public float Speed = 10;
	public float firingAngle = 45.0f;
	public float gravity = 9.8f;

	public Transform Projectile;
	public Transform myTransform;

	private bool itemDeleted;
	private int PlayerID;

	void Start()
	{
		myTransform = transform;
		itemDeleted = false;
	}

	void Update()
	{
		PlayerID = GetComponent<PlayerControllerScript>().ControllerID;
		if ( PlayerID == -1 ) return;

		if ( Input.GetKeyDown( "joystick " + PlayerID + " button 3" ) )
		{
			ThrowObject();
		}
	}

	// From: http://forum.unity3d.com/threads/throw-an-object-along-a-parabola.158855/
	public void ThrowObject()
	{
		// Set the projectile as the held object
		if ( gameObject.GetComponent<Inventory>().selectedSlot.transform.childCount == 0 ) return;
		Projectile = gameObject.GetComponent<Inventory>().selectedSlot.transform.GetChild( 0 );
		gameObject.GetComponent<Inventory>().selectedItem = null;
		Projectile.SetParent( null );

		// Handle type of throw
		myTransform = transform;
		GameObject cauldron = null;
		foreach ( Collider collider in Physics.OverlapSphere( transform.position, 5 ) )
		{
			if ( collider.gameObject.name.Contains( "Cauldron" ) )
			{
				cauldron = collider.gameObject;
			}
		}
		if ( cauldron )
		{
			TargetAt = cauldron.transform;
			ThrowObjectAt();
		}
		else
		{
			ThrowObjectForward();
		}

		//Delete the object from inventory
		if ( Projectile.tag == "Potion" )
		{
			DeleteObjectFromInventory( gameObject.GetComponent<Inventory>().potionInventory );
		}
		else if ( Projectile.tag == "Ingredient" )
		{
			DeleteObjectFromInventory( gameObject.GetComponent<Inventory>().ingredientInventory );
		}
	}

	public void ThrowObjectAt()
	{
		StartCoroutine( SimulateProjectile() );
	}

	public void ThrowObjectForward()
	{
		// Move projectile to the position of throwing object + add some offset if needed.
		//Projectile.position = transform.position + new Vector3( 0, 1.0f, 0 );

		// If the projectile is a potion vial, let it know who threw it.
		if ( Projectile.GetComponent<PotionVialBaseScript>() != null )
		{
			Projectile.GetComponent<PotionVialBaseScript>().BelongTo = this.gameObject;
		}

		// Calculate distance to target
		float target_Distance = Vector3.Distance( Projectile.position, Target.position );

		// Calculate the velocity needed to throw the object to the target at specified angle.
		float projectile_Velocity = target_Distance / ( Mathf.Sin( 2 * firingAngle * Mathf.Deg2Rad ) / gravity );

		// Extract the X  Y componenent of the velocity
		float Vx = Mathf.Sqrt( projectile_Velocity ) * Mathf.Cos( firingAngle * Mathf.Deg2Rad );
		float Vy = Mathf.Sqrt( projectile_Velocity ) * Mathf.Sin( firingAngle * Mathf.Deg2Rad );

		// Calculate flight time.
		float flightDuration = target_Distance / Vx;

		// Rotate projectile to face the target.
		//Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);
		Vector3 forward = transform.forward * Vx * ( Speed + ( GetComponent<PlayerMovementScript>().Velocity.magnitude * 100 ) );
		Vector3 up = transform.up * Vy * Speed;
		Projectile.GetComponent<Rigidbody>().isKinematic = false;
		Projectile.GetComponent<Rigidbody>().AddForce( forward + up );
	}

	IEnumerator SimulateProjectile()
	{
		Projectile.GetComponent<Rigidbody>().isKinematic = true;

		// Move projectile to the position of throwing object + add some offset if needed.
		Projectile.position = myTransform.position + new Vector3( 0, 1.0f, 0 );

		// Calculate distance to target
		float target_Distance = Vector3.Distance( Projectile.position, TargetAt.position );

		// Calculate the velocity needed to throw the object to the target at specified angle.
		float projectile_Velocity = target_Distance / ( Mathf.Sin( 2 * firingAngle * Mathf.Deg2Rad ) / gravity );

		// Extract the X  Y componenent of the velocity
		float Vx = Mathf.Sqrt( projectile_Velocity ) * Mathf.Cos( firingAngle * Mathf.Deg2Rad );
		float Vy = Mathf.Sqrt( projectile_Velocity ) * Mathf.Sin( firingAngle * Mathf.Deg2Rad );

		// Calculate flight time.
		float flightDuration = target_Distance / Vx;

		// Rotate projectile to face the target.
		Projectile.rotation = Quaternion.LookRotation( TargetAt.position - Projectile.position );

		float elapse_time = 0;

		while ( elapse_time < flightDuration )
		{
			if ( !Projectile.transform ) break;

			Projectile.Translate( 0, ( Vy - ( gravity * elapse_time ) ) * Time.deltaTime, Vx * Time.deltaTime );

			elapse_time += Time.deltaTime;

			yield return null;
		}

		// Tell cauldron to receive
		PlayerCauldronScript cauldronscrto = TargetAt.GetComponentInParent<PlayerCauldronScript>();
		if ( cauldronscrto )
		{
			cauldronscrto.AddIngredient( Projectile.gameObject );
			// Add score
			GameObject.Find( "MultiSceneVariables" ).GetComponent<MultiSceneVariablesScript>().AddScore( GetComponent<PlayerControllerScript>().PlayerID, 2 );
		}
		// More logic for player potion return state
		PlayerCauldronScript cauldronscrfrom = myTransform.GetComponentInParent<PlayerCauldronScript>();
		if ( cauldronscrfrom )
		{
			// Add potion to player & equip
			List<string> inventory = gameObject.GetComponent<Inventory>().potionInventory;
			for ( int i = 0; i < 4; i++ )
			{
				if ( inventory[i] == "EMPTY" )
				{
					inventory[i] = Projectile.gameObject.name;
					gameObject.GetComponent<Inventory>().Select( Projectile.gameObject );
					break;
				}
			}
		}
	}

	void DeleteObjectFromInventory( List<string> inventory )
	{
		//Remove item from inventory
		for ( int i = 0; i < 4; i++ )
		{
			if ( inventory[i] == Projectile.gameObject.name.Substring( 0, Projectile.gameObject.name.Length - 7 ) )
			{
				if ( itemDeleted == false )
				{
					inventory[i] = "EMPTY";
					itemDeleted = true;
				}
			}
		}
		itemDeleted = false;
	}
}
