using UnityEngine;
using System.Collections;

// Matthew Cormack
// 30th - 13:41
// Lerping of each players' camera

public class PlayerCameraFollowScript : MonoBehaviour
{
	public float Speed = 5;
	public float Height = 0;
	public float Angle = 0;
	public GameObject Player;

	void Update()
	{
		Vector3 camforward = transform.GetChild( 0 ).forward;
		Vector3 targetpos = ( Player.transform.position+ ( camforward * Height ) );
		if ( Height == 0 )
		{
			targetpos += Player.transform.forward;
		}
		float distance = Vector3.Distance( targetpos, transform.position );
		if ( distance >= 1 )
		{
			transform.position -= ( transform.position - targetpos ) * Time.deltaTime * Speed;
		}
		if (
			( ( Angle != 0 ) && ( distance < 1 ) ) ||
			( Angle == 0 )
		)
		{
			transform.GetChild( 0 ).localEulerAngles = new Vector3( transform.GetChild( 0 ).localEulerAngles.x - ( ( transform.GetChild( 0 ).localEulerAngles.x - ( 50 + Angle ) ) * Time.deltaTime * Speed ), 0, 0 );
		}
	}
}
