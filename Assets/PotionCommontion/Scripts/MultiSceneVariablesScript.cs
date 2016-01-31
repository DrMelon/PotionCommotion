using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Matthew Cormack
// 30th - 15:50
// Script which holds variables between scenes

public class MultiSceneVariablesScript : MonoBehaviour
{
	public int Players = 1;
	public ArrayList PlayerToController = new ArrayList();
	public int[] Score = new int[]{ 0, 0, 0, 0 };

	void Awake()
	{
		DontDestroyOnLoad( transform.gameObject );
	}

	public void AddScore( int player, int score )
	{
		player--;
		Score[player] += score;
		// Update HUD
		GameObject.Find( "Player " + ( player + 1 ) ).transform.GetChild( 0 ).GetChild( 0 ).GetComponent<Text>().text = Score[player].ToString();
	}
}
