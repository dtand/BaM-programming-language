using UnityEngine;
using System.Collections;

public class LockpickUISpawner : MonoBehaviour {

	public GameObject lockpickUI;
	private GameController gameController;

	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent< GameController >( );
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if( other.tag != "Player" )
			return;

		if( Input.GetButton("Pickup") )
		{
			
			Movement playerMovement = gameController.characterMovement;
			playerMovement.EnterIntoLockpicking( );
			lockpickUI.SetActive( true );
		}
	}
}
