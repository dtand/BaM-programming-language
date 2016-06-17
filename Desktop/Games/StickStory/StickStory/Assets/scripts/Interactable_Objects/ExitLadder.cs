using UnityEngine;
using System.Collections;

public class ExitLadder : MonoBehaviour {

	public JumpController jumpController;
	public Movement playerMovement;
	public GameObject actualSprite;

	void OnTriggerEnter2D( Collider2D other )
	{
		if( playerMovement.onLadder )
		{
			//Colliding w/ lower terrain and moving down
			if( other.tag == "terrain" || other.tag == "passable_terrain" 
			   && playerMovement.ladderDirection == Movement.Direction.Down)
			{

				playerMovement.LeaveLadder( );
			}
		}

//		//If player is set to passable, but grounded
//		else if( other.tag == "passable_terrain" && actualSprite.layer == 18)
//		{
//
//		}
	}

}
