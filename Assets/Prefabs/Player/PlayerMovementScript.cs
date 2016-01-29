using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour
{

    // Movement Variables
    public Vector3 Velocity;
    public Vector3 Acceleration;

    // Controller ID
    public int ControllerID = 1;

    // Movement Constants & Modifiers (for potion effects)
    public float AccelSpeed = 5.0f;
    public float AccelSpeedMultiplier = 1.0f;
    public float MoveSpeed = 0.3f;
    public float MoveSpeedMultiplier = 1.0f;
    public float DiveSpeed = 2.5f;
    public float DiveSpeedMultiplier = 1.0f;
    public float MoveFriction = 0.7f;
    public float MoveFrictionMultiplier = 1.0f;
    
    // Move States
    



	// Use this for initialization
	void Start ()
    {
        Velocity = new Vector3(0, 0, 0);
        Acceleration = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        HandleInput();
        Move();
	}

    // For Input management
    void HandleInput()
    {
        string PlayerID = ControllerID.ToString();

        
        // Get Movement
        if(Mathf.Abs(Input.GetAxis("P" + PlayerID + "_Move_X")) > 0.1f)
        {
            Acceleration.x = Input.GetAxis("P" + PlayerID + "_Move_X");
        }
        else
        {
            Acceleration.x = 0;
        }

        if (Mathf.Abs(Input.GetAxis("P" + PlayerID + "_Move_Y")) > 0.1f)
        {
            Acceleration.z = -Input.GetAxis("P" + PlayerID + "_Move_Y");
        }
        else
        {
            Acceleration.z = 0;
        }



    }

    // For Movement
    void Move()
    {
        // Forward-Euler Integration
        Velocity = Velocity + ((Acceleration * AccelSpeed * AccelSpeedMultiplier) * Time.deltaTime);

        // If there's not a lot of movement going on, attenuate velocity.
        if (Mathf.Abs(Acceleration.magnitude) < 0.01f)
        {
            Velocity = Velocity * (MoveFriction * MoveFrictionMultiplier);
        }


        // Check for max move speed. [TODO] If Not Diving.
        Velocity = Vector3.ClampMagnitude(Velocity, MoveSpeed * MoveSpeedMultiplier);

        // Check for wall or player VS player collisions here, adjusting velocity as necessary.
        // [TODO]

        // Update Position from Velocity

        transform.position = transform.position + Velocity;

        // Rotate them to face their velocity a lil bit.
        transform.rotation = Quaternion.LookRotation(Velocity);

    }
}
