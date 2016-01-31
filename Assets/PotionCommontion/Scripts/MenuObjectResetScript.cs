using UnityEngine;
using System.Collections;

// Matthew Cormack
// 31st - 06:01
// Object constant falling on main menu

public class MenuObjectResetScript : MonoBehaviour
{
    void OnTriggerEnter( Collider other )
    {
        other.transform.position = new Vector3( Random.Range( -15, 15 ), -other.transform.position.y + Random.Range( 0, 5 ), 0 );
        other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        other.GetComponent<Rigidbody>().angularVelocity = new Vector3( 0, Random.Range( 10, 20 ), 0 );
    }
}
