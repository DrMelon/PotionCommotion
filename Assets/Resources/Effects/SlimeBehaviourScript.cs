using UnityEngine;
using System.Collections.Generic;

public class SlimeBehaviourScript : MonoBehaviour
{

    // Status effect to apply

    // Life
    public float Lifetime = 5.0f;
    float currentLife = 0.0f;

    List<Collider> affectedPlayers;

    // Use this for initialization
    void Start ()
    {
        affectedPlayers = new List<Collider>();
        affectedPlayers.Clear();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Increase lifetime by deltatime.
        currentLife += Time.deltaTime;

        // If we've reached max life...
        if (currentLife > Lifetime)
        {
            // Go through list of affected players and un-apply effects.
            foreach(Collider ply in affectedPlayers)
            {
                // un-apply effect
            }
            affectedPlayers.Clear();
            Destroy(this.transform.parent.gameObject);
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // someone stepped on the slime.
        if(other.gameObject.tag.Contains("Player"))
        {
            // apply effect

            // add to list of current players
            if(!affectedPlayers.Contains(other))
            {
                affectedPlayers.Add(other);
            }
        }

    }

    void OnTriggerExit(Collider other)
    {
        // someone stopped steppin on the slime.
        if (other.gameObject.tag.Contains("Player"))
        {
            // un-apply effect

            // remove from affected list
            if(affectedPlayers.Contains(other))
            {
                affectedPlayers.Remove(other);
            }
        }
    }
}
