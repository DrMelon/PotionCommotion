using UnityEngine;
using System.Collections;

// Bomberman-style explosion visuals script.
// Apply to each 'leg' of the explosion.

public class XPlosionScript : MonoBehaviour
{

    public float Lifetime = 5.0f;
    public float StartScale = 1.0f;
    public float EndScale = 5.0f;

    float currentLife = 0.0f;

    // Use this for initialization
    void Start ()
    {
        this.transform.localScale = new Vector3(1, StartScale, 1);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Increase lifetime by deltatime.
        currentLife += Time.deltaTime * 5.0f;

        // Lerp!
        float lerpAmt = currentLife / Lifetime;
        this.transform.localScale = new Vector3(1, EndScale * lerpAmt, 1);


        // If we've reached max life...
        if (currentLife > Lifetime)
        {
            Destroy(this.gameObject);
        }
    }
}
