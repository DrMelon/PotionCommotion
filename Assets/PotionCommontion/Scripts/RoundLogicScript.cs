using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

// Matthew Cormack
// 31st - 11:15
// Logic for round beginning and ending

public class RoundLogicScript : MonoBehaviour
{
	public float MaxTime = 180;
	public bool RoundPlaying = true;
	public Text Text_Timer;
	public Text Text_Winner;
	public Text[] Text_Players;
	public Text[] Text_Ready;

	float CurrentTime = 0;
	private bool[] Ready = new bool[]{ false, false, false, false };

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if ( !RoundPlaying )
		{
			// Check for all ready
			bool ready = true;
			int player = 0;
			foreach ( int controller in GameObject.Find( "MultiSceneVariables" ).GetComponent<MultiSceneVariablesScript>().PlayerToController )
			{
				if ( Input.GetKeyDown( "joystick " + controller + " button 0" ) )
				{
					Ready[player] = true;
					Text_Ready[player].text = "✔";
					Text_Ready[player].color = Color.green;
				}
				if ( Input.GetKeyDown( "joystick " + controller + " button 1" ) )
				{
					Ready[player] = false;
					Text_Ready[player].text = "X";
					Text_Ready[player].color = Color.red;
				}
				if ( !Ready[player] )
				{
					ready = false;
				}
				player++;
			}
			if ( ready )
			{
				EndRound();
			}

			return;
		}

		CurrentTime += Time.deltaTime;

		float remainingtime = Mathf.Max( 0, MaxTime - CurrentTime );
		//if ( remainingtime >= 0 )
		{
			int main = (int) Mathf.Floor( remainingtime );
			int rem = (int) ( ( remainingtime - main ) * 100 );
			string mainstr = main.ToString();
			{
				if ( mainstr.Length == 1 )
				{
					mainstr = "0" + main;
				}
				else if ( mainstr.Length == 0 )
				{
					mainstr.Insert( 0, "00" );
				}
			}
			string remstr = rem.ToString();
			{
				if ( remstr.Length == 1 )
				{
					remstr = "0" + rem;
				}
				else if ( remstr.Length == 0 )
				{
					remstr.Insert( 0, "00" );
				}
			}
			Text_Timer.text = mainstr + ":" + remstr;
		}
		if ( remainingtime <= 0 )
		{
			// Decide on winner
			Text_Winner.transform.parent.gameObject.SetActive( true );
			MultiSceneVariablesScript scores = GameObject.Find( "MultiSceneVariables" ).GetComponent<MultiSceneVariablesScript>();
			int winner = 1;
			int max = -1;
			{
				for ( int player = 0; player < 4; player ++ )
				{
					if ( scores.Score[player] > max )
					{
						winner = player;
						max = scores.Score[player];
					}
				}
			}
			Text_Winner.text = "POTION COMMOTION\nPlayer " + ( winner + 1 ) + " Wins!";

			// Display each player's score
			for ( int player = 0; player < 4; player ++ )
			{
				Text_Players[player].text = scores.Score[player].ToString();
			}

			// Flag as over, wait for return to menu input
			RoundPlaying = false;
			//EndRound();
		}
	}

	public void EndRound()
	{
		// Allow new variables to be created by the main menu
		Destroy( GameObject.Find( "MultiSceneVariables" ) );

		// Return to the main menu
		Application.LoadLevel( 0 );
	}
}
