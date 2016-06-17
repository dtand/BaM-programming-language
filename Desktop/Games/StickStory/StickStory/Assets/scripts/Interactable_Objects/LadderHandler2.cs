using UnityEngine;
using System.Collections;

//Used to enter ladder from above
public class LadderHandler2 : MonoBehaviour 
{
	GameController gameController;
	
	// Use this for initialization
	void Start () 
	{
		gameController = GameObject.FindGameObjectWithTag( "GameController" ).GetComponent< GameController >( );
	}
	
	void OnTriggerStay2D( Collider2D other )
	{
		if( other.tag == "Player" )
		{
			GameObject player = gameController.mainCharacter;
			
			//Get player movement
			Movement playerMovement = gameController.characterMovement;
			JumpController playerJump = playerMovement.jumpController;
			float f = Input.GetAxis( "Vertical" );

			//Do nothing if player is already on the ladder
			if( playerMovement.onLadder ) return;
			
			//Place player onto the ladder
			if( f < 0f )
			{
				//Reset jump
				if ( playerJump.jumping ) playerJump.jumping = false;

				playerMovement.MoveToLadder( gameObject );
				player.transform.position = transform.position;
				playerMovement.actualSprite.layer = 18;
			}
		}
	}
}
