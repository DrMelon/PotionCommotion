using UnityEngine;
using System.Collections;

// Matthew Cormack
// 30th - 13:41
// Lerping of each players' camera

public class PlayerCameraFollowScript : MonoBehaviour
{
	public float Speed = 5;
	public GameObject Player;

	void Update()
	{
		transform.position -= ( transform.position - ( Player.transform.position + Player.transform.forward ) ) * Time.deltaTime * Speed;
	}
}
