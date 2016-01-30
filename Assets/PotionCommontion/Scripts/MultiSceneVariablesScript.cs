using UnityEngine;
using System.Collections;

// Matthew Cormack
// 30th - 15:50
// Script which holds variables between scenes

public class MultiSceneVariablesScript : MonoBehaviour
{
	public int Players = 1;
	public ArrayList PlayerToController = new ArrayList();

	void Awake()
	{
		DontDestroyOnLoad( transform.gameObject );
	}
}
