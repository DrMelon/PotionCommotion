using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Matthew Cormack
// 30th - 15:06
// Handle players joining the game in the initial menu scene

public class MenuPlayerJoinScript : MonoBehaviour
{
	public GameObject[] PlayerUI;

	private int[] PlayerControllerIDs = new int[4] { -1, -1, -1, -1 };
	private bool[] PlayerReady = new bool[4];

	void Update()
	{
		for ( int controller = 1; controller < 5; controller++ )
		{
			// Join/ready
			if ( Input.GetKeyDown( "joystick " + controller + " button 0" ) )
			{
				int player = CheckForController( controller );
				// Add controller
				if ( player == -1 )
				{
					for ( int playercon = 0; playercon < 4; playercon++ )
					{
						if ( PlayerControllerIDs[playercon] == -1 )
						{
							PlayerControllerIDs[playercon] = controller;
							break;
						}
					}
				}
				// Ready up
				else
				{
					PlayerReady[player] = true;
					CheckAllReady();
				}
				UpdateUI();
			}
			// Unjoin/unready
			if ( Input.GetKeyDown( "joystick " + controller + " button 1" ) )
			{
				int player = CheckForController( controller );
				if ( player != -1 )
				{
					// Not ready then unjoin
					if ( !PlayerReady[player] )
					{
						PlayerControllerIDs[player] = -1;
						// Reshuffle the other players to be in linear order from 0
						if ( player < 3 )
						{
							for ( int swap = player; swap < 3; swap++ )
							{
								PlayerControllerIDs[swap] = PlayerControllerIDs[swap+1];
								PlayerControllerIDs[swap+1] = -1;
							}
						}
					}
					// Otherwise just unready
					else
					{
						PlayerReady[player] = false;
					}
					UpdateUI();
				}
			}
		}
	}

	int CheckForController( int controller )
	{
		for ( int player = 0; player < 4; player++ )
		{
			if ( PlayerControllerIDs[player] == controller )
			{
				return player;
			}
		}
		return -1;
	}

	void CheckAllReady()
	{
		bool ready = true;
		// Check for players who have joined but are not ready
		for ( int player = 0; player < 4; player++ )
		{
			if ( ( PlayerControllerIDs[player] != -1 ) && ( !PlayerReady[player] ) )
			{
				ready = false;
				break;
			}
		}
		// If all the player's who have joined are also ready then continue to the game
		if ( ready )
		{
			// Store information for play state
			MultiSceneVariablesScript script = GameObject.Find( "MultiSceneVariables" ).GetComponent<MultiSceneVariablesScript>();
			{
				// Number of players
				int players = 0;
				{
					for ( int player = 3; player >= 0; player-- )
					{
						if ( PlayerControllerIDs[player] != -1 )
						{
							players = player + 1;
							break;
						}
					}
				}
				script.Players = players;

				// Mapping
				script.PlayerToController.Clear();
				for ( int player = 0; player < 4; player++ )
				{
					if ( PlayerControllerIDs[player] != -1 )
					{
						script.PlayerToController.Add( PlayerControllerIDs[player] );
					}
				}
			}
			Application.LoadLevel( "MatthewScene" );
		}
	}

	void UpdateUI()
	{
		for ( int player = 0; player < 4; player++ )
		{
			Transform info = PlayerUI[player].transform.GetChild( 2 );
			Text instruction = info.GetChild( 0 ).GetComponent<Text>();
			GameObject notready = info.GetChild( 1 ).gameObject;
			GameObject ready = info.GetChild( 2 ).gameObject;

			// Hasn't joined
			if ( PlayerControllerIDs[player] == -1 )
			{
				instruction.text = "Empty Slot";
				instruction.color = Color.black;

				notready.SetActive( true );
				ready.SetActive( false );
			}
			// Isn't ready
			else if ( !PlayerReady[player] )
			{
				instruction.text = "Ready?";
				instruction.color = Color.red;

				notready.SetActive( true );
				ready.SetActive( false );
			}
			// Isn't ready
			else
			{
				instruction.text = "Ready!";
				instruction.color = Color.green;

				notready.SetActive( false );
				ready.SetActive( true );
			}
		}
	}
}
