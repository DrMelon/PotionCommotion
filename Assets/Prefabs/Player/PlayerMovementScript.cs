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
    public float Gravity = 0.98f;
    public float GravityMultiplier = 1.0f;
    public float JumpVelocity = 0.5f;
    public float JumpVelocityMultiplier = 1.0f;
    public float DivingFriction = 0.9f;
    public float DivingFrictionMultiplier = 1.0f;

    // Move States
    public bool OnGround = false;
    public bool CollidingX = false;
    public bool CollidingZ = false;
    public bool IsDiving = false;

    // Collider to use.
    public BoxCollider theCollider = null;



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
        if(!IsDiving)
        {
            if (Mathf.Abs(Input.GetAxis("P" + PlayerID + "_Move_X")) > 0.1f)
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

            if (Input.GetButtonDown("P" + PlayerID + "_Button_Jump"))
            {
                if (OnGround)
                {
                    OnGround = false;
                    Velocity.y = JumpVelocity * JumpVelocityMultiplier;
                }
            }

            if(Input.GetButtonDown("P" + PlayerID + "_Button_Dive"))
            {
                if(OnGround)
                {
                    OnGround = false;
                    IsDiving = true;
                    Velocity.y = JumpVelocity * JumpVelocityMultiplier * 0.5f;
                    Velocity.x = Input.GetAxis("P" + PlayerID + "_Move_X") * DiveSpeed * DiveSpeedMultiplier;
                    Velocity.z = -Input.GetAxis("P" + PlayerID + "_Move_Y") * DiveSpeed * DiveSpeedMultiplier;
                }
            }
        }
        else
        {
            Acceleration.x = 0;
            Acceleration.z = 0;
        }




    }

    // For Movement
    void Move()
    {
        // Forward-Euler Integration
        Velocity = Velocity + ((Acceleration * AccelSpeed * AccelSpeedMultiplier) * Time.deltaTime);

        // Gravity
        if(!OnGround)
        {
            Velocity.y = Velocity.y - ((Gravity * GravityMultiplier) * Time.deltaTime);
        }

        // If there's not a lot of movement going on, attenuate velocity.
        if(!IsDiving)
        {
            if (Mathf.Abs(Acceleration.magnitude) < 0.01f)
            {
                Velocity.x = Velocity.x * (MoveFriction * MoveFrictionMultiplier);
                Velocity.z = Velocity.z * (MoveFriction * MoveFrictionMultiplier);
            }
        }
        else
        {
            if (Mathf.Abs(Acceleration.magnitude) < 0.01f)
            {
                Velocity.x = Velocity.x * (DivingFriction * DivingFrictionMultiplier);
                Velocity.z = Velocity.z * (DivingFriction * DivingFrictionMultiplier);
            }

        }

        if(IsDiving)
        {
            if (Mathf.Abs(Velocity.x) < 0.1f && Mathf.Asin(Velocity.z) < 0.1f)
            {
                IsDiving = false;
            }
        }
        



        // Check for max move speed. [TODO] If Not Diving.
        // Make sure not to apply this to air velocity.
        if (!IsDiving)
        {
            float yVel = Velocity.y;
            Velocity = Vector3.ClampMagnitude(Velocity, MoveSpeed * MoveSpeedMultiplier);
            Velocity.y = yVel;
        }


        // Collision tests should evaluate here
        CheckCollisions();



        // [TODO]

        // Update Position from Velocity
        transform.position = transform.position + Velocity;

        

        // Rotate them to face their velocity a lil bit.
        if(Mathf.Abs(Velocity.x) > 0.001f || Mathf.Abs(Velocity.z) > 0.001f)
        {
            Vector3 facingDirection = new Vector3(Velocity.x, 0, Velocity.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(facingDirection), Time.deltaTime * 25);
            
        }


    }

    void CheckCollisions()
    {
        if(theCollider == null)
        {
            return;
        }

        // Collision with world.
        RaycastHit rayInfo;
        if(GetComponent<Rigidbody>().SweepTest(-Vector3.up, out rayInfo, -Velocity.y))
        {
            // If we're falling through the air...
            if(!OnGround && Velocity.y < 0)
            {

                if(rayInfo.rigidbody.GetComponent<Collider>().gameObject.CompareTag("World"))
                {
                    OnGround = true;
                    Velocity.y = 0;
                    Vector3 newPos = transform.position;
                    newPos.y = rayInfo.point.y + this.transform.localScale.y / 2;
                    transform.position = newPos;
                }
                    
                
            }

        }

        

        if(Velocity.x > 0)
        {
            Ray theRay = new Ray(transform.position, Vector3.right);
            if (Physics.Raycast(theRay, out rayInfo, Velocity.magnitude))
            {
                // Wall collision
                if (rayInfo.rigidbody.GetComponent<Collider>().gameObject.CompareTag("World"))
                {
                    Vector3 newPos = transform.position;

                    newPos.x = rayInfo.point.x - Velocity.magnitude;
                    Debug.DrawLine(transform.position, rayInfo.point, Color.red, 1000);

                    transform.position = newPos;

                    Velocity.x = 0;
                }
            }
        }
        if(Velocity.x < 0)
        {
            Ray theRay = new Ray(transform.position, -Vector3.right);
            if (Physics.Raycast(theRay, out rayInfo, Velocity.magnitude))
            {
                // Wall collision
                if (rayInfo.rigidbody.GetComponent<Collider>().gameObject.CompareTag("World"))
                {
                    Vector3 newPos = transform.position;

                    newPos.x = rayInfo.point.x + Velocity.magnitude;
                    Debug.DrawLine(transform.position, rayInfo.point, Color.red, 1000);

                    transform.position = newPos;

                    Velocity.x = 0;
                }
            }
        }
        if (Velocity.z > 0)
        {
            Ray theRay = new Ray(transform.position, Vector3.forward);
            if (Physics.Raycast(theRay, out rayInfo, Velocity.magnitude))
            {
                // Wall collision
                if (rayInfo.rigidbody.GetComponent<Collider>().gameObject.CompareTag("World"))
                {
                    Vector3 newPos = transform.position;

                    newPos.z = rayInfo.point.z - Velocity.magnitude;
                    Debug.DrawLine(transform.position, rayInfo.point, Color.red, 1000);

                    transform.position = newPos;

                    Velocity.z = 0;
                }
            }
        }
        if (Velocity.z < 0)
        {
            Ray theRay = new Ray(transform.position, -Vector3.forward);
            if (Physics.Raycast(theRay, out rayInfo, Velocity.magnitude))
            {
                // Wall collision
                if (rayInfo.rigidbody.GetComponent<Collider>().gameObject.CompareTag("World"))
                {
                    Vector3 newPos = transform.position;

                    newPos.z = rayInfo.point.z + Velocity.magnitude;
                    Debug.DrawLine(transform.position, rayInfo.point, Color.red, 1000);

                    transform.position = newPos;

                    Velocity.z = 0;
                }
            }
        }





    }




}
