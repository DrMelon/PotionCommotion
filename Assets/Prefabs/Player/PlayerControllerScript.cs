using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

// Matthew Cormack
// 31st - 08:04
// Player movement, based on FPSController standard asset

[RequireComponent( typeof( CharacterController ) )]
[RequireComponent( typeof( AudioSource ) )]
public class PlayerControllerScript : MonoBehaviour
{
    public int PlayerID;
    public int ControllerID = -1;

    [SerializeField]
    private bool m_IsWalking;
    [SerializeField]
    private float m_WalkSpeed;
    [SerializeField]
    private float m_RunSpeed;
    [SerializeField]
    [Range( 0f, 1f )]
    private float m_RunstepLenghten;
    [SerializeField]
    private float m_JumpSpeed;
    [SerializeField]
    private float m_StickToGroundForce;
    [SerializeField]
    private float m_GravityMultiplier;
    [SerializeField]
    private float m_StepInterval;
    [SerializeField]
    private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField]
    private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField]
    private AudioClip m_LandSound;           // the sound played when character touches back on ground.
   
    private bool m_Jump;
    private bool m_Dive;
    private float m_YRotation;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded;
    private float m_StepCycle;
    private float m_NextStep;
    private bool m_Jumping;
    private AudioSource m_AudioSource;

    // Use this for initialization
    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
        m_Jumping = false;
        m_AudioSource = GetComponent<AudioSource>();

        // Get the controller to player map from the MultiSceneVariablesScripts
        ControllerID = -1;
        int player = PlayerID;
        MultiSceneVariablesScript script = GameObject.Find( "MultiSceneVariables" ).GetComponent<MultiSceneVariablesScript>();
        {
            int playeriter = 0;
            foreach ( int playercon in script.PlayerToController.ToArray() )
            {
                if ( ( player - 1 ) == playeriter )
                {
                    ControllerID = playercon;
                    break;
                }
                playeriter++;
            }
        }
        // Disable unused players
        if ( ControllerID == -1 )
        {
            gameObject.SetActive( false );
            enabled = false;
        }
    }


    // Update is called once per frame
    private void Update()
    {
        if ( ControllerID == -1 ) return;

        // Rotate to face direction of travel
        if ( Mathf.Abs( m_MoveDir.x ) > 0.001f || Mathf.Abs( m_MoveDir.z ) > 0.001f )
        {
            Vector3 facingDirection = new Vector3( m_MoveDir.x, 0, m_MoveDir.z );
            transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( facingDirection ), Time.deltaTime * 25 );
        }

        // the jump state needs to read here to make sure it is not missed
        if ( !m_Jump )
        {
            m_Jump = Input.GetKeyDown( "joystick " + ControllerID + " button 0" );
        }
        if ( !m_Dive )
        {
            m_Dive = Input.GetKeyDown( "joystick " + ControllerID + " button 2" );
        }

        if ( !m_PreviouslyGrounded && m_CharacterController.isGrounded )
        {
            PlayLandingSound();
            m_MoveDir.y = 0f;
            m_Jumping = false;
        }
        if ( !m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded )
        {
            m_MoveDir.y = 0f;
        }

        m_PreviouslyGrounded = m_CharacterController.isGrounded;
    }


    private void PlayLandingSound()
    {
        m_AudioSource.clip = m_LandSound;
        m_AudioSource.Play();
        m_NextStep = m_StepCycle + .5f;
    }


    private void FixedUpdate()
    {
        float speed;
        GetInput( out speed );
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = -Vector3.forward * m_Input.y + Vector3.right * m_Input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast( transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                            m_CharacterController.height / 2f );
        desiredMove = Vector3.ProjectOnPlane( desiredMove, hitInfo.normal ).normalized;

        m_MoveDir.x = desiredMove.x * speed;
        m_MoveDir.z = desiredMove.z * speed;


        if ( m_CharacterController.isGrounded )
        {
            m_MoveDir.y = -m_StickToGroundForce;

            if ( m_Jump )
            {
                m_MoveDir.y = m_JumpSpeed;
                PlayJumpSound();
                m_Jump = false;
                m_Jumping = true;
            }
            else if ( m_Dive )
            {
                m_MoveDir.y = m_JumpSpeed / 4;
                m_MoveDir.x *= m_RunSpeed;
                m_MoveDir.z *= m_RunSpeed;
                //PlayJumpSound();
                m_Jump = false;
                m_Dive = false;
                m_Jumping = true;
            }
        }
        else
        {
            m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }
        m_CollisionFlags = m_CharacterController.Move( m_MoveDir * Time.fixedDeltaTime );

        ProgressStepCycle( speed );

        if ( transform.position.y < 0 )
        {
            transform.position = new Vector3( transform.position.x, 1.5f, transform.position.z );
        }
    }


    private void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
    }


    private void ProgressStepCycle( float speed )
    {
        if ( m_CharacterController.velocity.sqrMagnitude > 0 && ( m_Input.x != 0 || m_Input.y != 0 ) )
        {
            m_StepCycle += ( m_CharacterController.velocity.magnitude + ( speed * ( m_IsWalking ? 1f : m_RunstepLenghten ) ) ) *
                            Time.fixedDeltaTime;
        }

        if ( !( m_StepCycle > m_NextStep ) )
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }

    private void PlayFootStepAudio()
    {
        if ( !m_CharacterController.isGrounded )
        {
            return;
        }
        if ( m_FootstepSounds.Length == 0 ) return;

        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range( 1, m_FootstepSounds.Length );
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot( m_AudioSource.clip );
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }

    private void GetInput( out float speed )
    {
        // Read input
        float horizontal = Input.GetAxis( "P" + ControllerID + "_Move_X" );
        float vertical = Input.GetAxis( "P" + ControllerID + "_Move_Y" );

        bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
        // On standalone builds, walk/run speed is modified by a key press.
        // keep track of whether or not the character is walking or running
        m_IsWalking = !Input.GetKey( KeyCode.LeftShift );
#endif
        // set the desired speed to be walking or running
        speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
        m_Input = new Vector2( horizontal, vertical );

        // normalize input if it exceeds 1 in combined length:
        if ( m_Input.sqrMagnitude > 1 )
        {
            m_Input.Normalize();
        }
    }

    private void OnControllerColliderHit( ControllerColliderHit hit )
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if ( m_CollisionFlags == CollisionFlags.Below )
        {
            return;
        }

        if ( body == null || body.isKinematic )
        {
            return;
        }
        body.AddForceAtPosition( m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse );
    }
}
