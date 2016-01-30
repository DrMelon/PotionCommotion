using UnityEngine;
using System.Collections;

// Matthew Cormack
// 30th - 11:29
// Animation polish on the cauldron objects

public class PlayerCauldronAnimateScript : MonoBehaviour
{
    Vector3 DefaultPosition;
    float time;
    float DirectionSin;
    float DirectionCos;

    void Start()
    {
        DefaultPosition = transform.localPosition;
    }

    void Update()
    {
        Vector3 offset = new Vector3( 5, 0, 5 );
        float stime = ( time * 2 ) - 1;
        offset.x *= Mathf.Sin( time * 10 * DirectionSin ) / 5;
        offset.z *= Mathf.Cos( time * 10 * DirectionCos ) / 5;
        Quaternion rot = Quaternion.Euler( offset );
        transform.localRotation = Quaternion.Lerp( transform.localRotation, rot, time );
        time += Time.deltaTime / 10;
        if ( time >= 1 )
        {
            time = 0;
            DirectionSin = Random.Range( -1, 1 );
            DirectionCos = Random.Range( -1, 1 );
        }
    }
}
