﻿using UnityEngine;
using System.Collections;

// Matthew Cormack
// 29th - 23:38
// Allows the player to throw the object forward/in to the cauldron

public class PlayerObjectThrowScript : MonoBehaviour
{
	public bool Debug_Throw;

	public Transform Target;
	public Transform TargetAt;
	public float Speed = 10;
	public float firingAngle = 45.0f;
	public float gravity = 9.8f;

	public Transform Projectile;
	public Transform myTransform;

	void Start()
	{
		myTransform = transform;
	}

	void Update()
	{
		if ( Debug_Throw )
		{
			ThrowObject();
			Debug_Throw = false;
		}
	}

	// From: http://forum.unity3d.com/threads/throw-an-object-along-a-parabola.158855/
	public void ThrowObject()
	{
		GameObject cauldron = null;
		foreach ( Collider collider in Physics.OverlapSphere( transform.position, 15 ) )
		{
			if ( collider.gameObject.name.Contains( "Cauldron" ) )
			{
				cauldron = collider.gameObject;
			}
		}
		if ( cauldron )
		{
			TargetAt = cauldron.transform;
			StartCoroutine( SimulateProjectile() );
		}
		else
		{
			ThrowObjectForward();
		}
	}

	void ThrowObjectForward()
	{
		// Move projectile to the position of throwing object + add some offset if needed.
		Projectile.position = transform.position + new Vector3( 0, 1.0f, 0 );

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
		Vector3 forward = transform.forward * Vx * Speed;
		Vector3 up = transform.up * Vy * Speed;
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
			Projectile.Translate( 0, ( Vy - ( gravity * elapse_time ) ) * Time.deltaTime, Vx * Time.deltaTime );

			elapse_time += Time.deltaTime;

			yield return null;
		}
	}
}
