using UnityEngine;
using System.Collections;

// Matthew Cormack
// 30th - 13:54
// Setup the camera for each player

public class CameraSetupScript : MonoBehaviour
{
	public int Players = 1;
	public GameObject CameraPrefab;

	ArrayList Cameras = new ArrayList();

	void Start()
	{
		Players = GameObject.Find( "MultiSceneVariables" ).GetComponent<MultiSceneVariablesScript>().Players;

		Setup();
	}

	public void Setup()
	{
		GameObject camera;

		// Instantiate a camera for each player
		for ( int player = 0; player < Players; player++ )
		{
			camera = Instantiate( CameraPrefab );
			{
				camera.GetComponent<PlayerCameraFollowScript>().Player = GameObject.Find( "Player " + ( player + 1 ) );
				camera.transform.parent = GameObject.Find( "Cameras" ).transform;

				Camera caminstance = camera.transform.GetChild( 0 ).GetComponent<Camera>();
				int mask = 0;
				{
					for ( int maskplayer = 0; maskplayer < Players; maskplayer++ )
					{
						if ( player != maskplayer )
						{
							mask |= ( 1 << LayerMask.NameToLayer( "Player" + ( maskplayer + 1 ) ) );
						}
					}
				}
				caminstance.cullingMask = ~mask;

				camera.GetComponent<PlayerCameraFollowScript>().Player.GetComponent<Inventory>().LinkedCamera = camera;
			}
			Cameras.Add( camera );
		}

		// Cameras will need to be moved
		if ( Players != 1 )
		{
			// Vertical splitscreen
			if ( Players == 2 )
			{
				camera = (GameObject) Cameras.ToArray()[0];
				camera.GetComponentInChildren<Camera>().rect = new Rect( 0, 0.5f, 1, 0.5f );
				camera = (GameObject) Cameras.ToArray()[1];
				camera.GetComponentInChildren<Camera>().rect = new Rect( 0, 0, 1, 0.5f );
			}
			else if ( Players > 2 )
			{
				camera = (GameObject) Cameras.ToArray()[0];
				camera.GetComponentInChildren<Camera>().rect = new Rect( 0, 0.5f, 0.5f, 0.5f );
				camera = (GameObject) Cameras.ToArray()[1];
				camera.GetComponentInChildren<Camera>().rect = new Rect( 0.5f, 0.5f, 0.5f, 0.5f );
				camera = (GameObject) Cameras.ToArray()[2];
				camera.GetComponentInChildren<Camera>().rect = new Rect( 0, 0, 0.5f, 0.5f );
				if ( Players == 4 )
				{
					camera = (GameObject) Cameras.ToArray()[3];
					camera.GetComponentInChildren<Camera>().rect = new Rect( 0.5f, 0, 0.5f, 0.5f );
				}
			}
		}
	}
}
