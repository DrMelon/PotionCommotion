using UnityEngine;
using System.Collections.Generic;

// Explosion effect behaviour handling.

public class ExplosionScript : MonoBehaviour
{

    public float Lifetime = 5.0f;
    public float StartScale = 1.0f;
    public float EndScale = 5.0f;
    public int NumBeams = 5;
    float currentLife = 0.0f;
    
    List<GameObject> myBeams = null;
    public Object BeamCopy = null;
    

	// Use this for initialization
	void Start ()
    {
        this.transform.localScale = new Vector3(StartScale, StartScale, StartScale);

        // Spawn beams
        myBeams = new List<GameObject>();
        for(int i = 0; i < NumBeams; i++)
        {
            Vector3 randomRotation = new Vector3(Random.Range(-45, 45), Random.Range(-45, 45), Random.Range(-45, 45));
            GameObject newBeam = (GameObject)Instantiate(BeamCopy, this.transform.position, Quaternion.LookRotation(randomRotation));
            newBeam.transform.parent = this.transform;
            myBeams.Add(newBeam);
            
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Increase lifetime by deltatime.
        currentLife += Time.deltaTime * 5.0f;

        // Lerp!
        float lerpAmt = currentLife / Lifetime;
        this.transform.localScale = new Vector3(EndScale * lerpAmt, EndScale * lerpAmt, EndScale * lerpAmt);

        // Wiggle beams
        foreach(GameObject beam in myBeams)
        {
            Vector3 randomRotation = new Vector3(Random.Range(-45, 45), Random.Range(-45, 45), Random.Range(-45, 45));
            beam.transform.rotation = Quaternion.Slerp(beam.transform.rotation, Quaternion.LookRotation(randomRotation), Time.deltaTime * 5.0f);
        }

        // If we've reached max life...
        if(currentLife > Lifetime)
        {
            Destroy(this.gameObject);
        }
	}
}
