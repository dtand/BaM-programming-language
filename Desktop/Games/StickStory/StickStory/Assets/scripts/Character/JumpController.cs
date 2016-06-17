using UnityEngine;
using System.Collections;

public class JumpController : MonoBehaviour {

	public float velocityScale = 2f;
	public Animator anim;

	public bool jumping;
	public bool grounded;
	public Rigidbody2D rigidBody2D;
	private Movement characterMovement;

	public GameObject actualSprite;
	public float sprintToJumpForce;
	
	void Start()
	{
		characterMovement = GetComponent< Movement >( );
		jumping = false;
		rigidBody2D = GetComponent<Rigidbody2D>();
	}
	
	void Update()
	{


		//Update collision layers
		if( jumping )
		{
			//Player is moving up, and can pass passable objects
			if( rigidBody2D.velocity.y > 0f )
			 	actualSprite.layer = 18;
			else
				actualSprite.layer = 16;		//return to def layer
		}

		//Reset jumping
		if( jumping && rigidBody2D.velocity.y < .001 && rigidBody2D.velocity.y > -.001)
		{
			grounded = true;
			jumping = false;
			anim.SetBool( "jumping", jumping );
			actualSprite.layer = 16;
		}
		
		//Apply jump
		if( Input.GetButtonDown("Jump") && !jumping )
		{	
			grounded = false;

			if( characterMovement.sprinting )
			{
				if( characterMovement.getDirection() == Movement.Direction.Right )
					rigidBody2D.AddForce( new Vector2( sprintToJumpForce, sprintToJumpForce * .5f ) );
				else
					rigidBody2D.AddForce( new Vector2( sprintToJumpForce * -1f, sprintToJumpForce * .5f ) );
			}

			//Character is leaving ladder -> reset gravity
			if( characterMovement.onLadder ) 
			{
				characterMovement.LeaveLadder( );
			}

			Vector2 velocity = Vector2.up * velocityScale;
			rigidBody2D.velocity = velocity;
			//rigidBody2D.AddForce( Vector2.up * velocityScale );
			jumping = true;
			anim.SetBool( "jumping", jumping );

			//Allow passability
			actualSprite.layer = 18;
		}
	}
}
