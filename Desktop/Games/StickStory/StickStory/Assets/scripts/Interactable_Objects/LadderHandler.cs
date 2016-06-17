using UnityEngine;
using System.Collections;

public class LadderHandler : MonoBehaviour 
{
	GameController gameController;
	public GameObject topEntry;

	// Use this for initialization
	void Start () 
	{
		topEntry.SetActive( false );
		gameController = GameObject.FindGameObjectWithTag( "GameController" ).GetComponent< GameController >( );
	}

	void OnTriggerStay2D( Collider2D other )
	{
		if( other.tag == "Player" )
		{
			GameObject player = gameController.mainCharacter;
			topEntry.SetActive( false );

			//Get player movement
			Movement playerMovement = gameController.characterMovement;
			JumpController playerJump = playerMovement.jumpController;
			float f = Input.GetAxis( "Vertical" );

			//Place player onto the ladder
			if( f > 0f )
			{
				//Reset jump
				if ( playerJump.jumping ) playerJump.jumping = false;

				//Set layer to passable
				other.gameObject.layer = 18;
				playerMovement.MoveToLadder( topEntry );
			}
		}
	}

	void OnTriggerExit2D( Collider2D other )
	{
		if( other.tag == "Player" )
		{
			GameObject player = gameController.mainCharacter;

			//Get player movement
			Movement playerMovement = gameController.characterMovement;
			float f = Input.GetAxis( "Vertical" );
			
			//Place player onto the ladder
			if( f > 0f )
			{
				//				player.transform.position = new Vector3( transform.position.x, 
				//				                                        player.transform.position.x, 
				//				                                        1f );

				//Return character to original layer
				other.gameObject.layer = 16;
				playerMovement.LeaveLadder( );
			}
		}
	}
}
