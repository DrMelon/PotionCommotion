using UnityEngine;
using System.Collections;

// Matthew Cormack
// 30th - 23:48
// Item gravity alterations

public class GravityAlterScript : MonoBehaviour
{
    public float Multiplier = 1;

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce( Physics.gravity * GetComponent<Rigidbody>().mass * Multiplier );
    }
}
