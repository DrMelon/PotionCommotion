using UnityEngine;
using System.Collections;

// This script will allow a particle system to destroy itself.

public class SelfDestroy : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void InitSelfDestruct(float time)
    {
        StartCoroutine(SelfDestruct(time));
    }

    IEnumerator SelfDestruct(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
