using UnityEngine;
using System.Collections;

public class WigCollector : MonoBehaviour 
{
	GameController gameController;
	public int value = 1;

	void Start()
	{
		gameController = GameObject.FindGameObjectWithTag( "GameController" ).GetComponent< GameController >( );
	}


	void OnTriggerStay2D( Collider2D other )
	{
		
		if( Input.GetButtonDown( "Pickup" ) && other.tag == "Player" )
		{
			gameController.AlterWigs( value );
			Destroy( gameObject );
		}
	}
}
