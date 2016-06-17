using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	[HideInInspector]public bool isMoving = false;
	[HideInInspector]public bool freeze;

	public bool sprinting;
	private bool cantSprint;
	private int framesSprinting;
	private int framesNotSprinting;

	public bool onLadder;

	public float speed;
	public float sprintSpeed;
	public int costToSprint = 2;
	public BarController staminaBar;
	private float chosenSpeed;
	
	
	public enum Direction { Right, Left, Up, Down };
	private Direction currentDirection;
	public Direction ladderDirection;
	public Animator anim;

	public JumpController jumpController;

	public GameObject actualSprite;

	private GameObject topLadderEntry;
	private GameController gameController;
	
	
	// Use this for initialization
	void Start () 
	{
		gameController = GameObject.FindGameObjectWithTag( "GameController" ).GetComponent< GameController >( );
		framesSprinting = 0;
		framesNotSprinting = 0;
		topLadderEntry = null;
		jumpController = GetComponent< JumpController >( );
		chosenSpeed = speed;
		currentDirection = Direction.Right;
		onLadder = false;
		sprinting = false;
	}
	
	// Update is called once per frame
	void Update () 
	{	
		//Prevents player from moving
		if( freeze ) return;

		//Don't do any other processing while on ladder
		if( onLadder )
		{
			CheckClimbing( );
			return;
		}

		if ( CheckCrouch( ) ) return;


		sprinting = CheckSprint( );

		if(!sprinting )
		{
			if ( !gameController.IsFull( "stamina" ) && framesNotSprinting == 12 )
			{
				gameController.AlterStamina( 2 );
				framesNotSprinting = 0;
				gameController.Equalize( "stamina" );
			}
			
			framesNotSprinting++;
		}


		//Get motion based on input, speed and time delay
		float motion = Input.GetAxis ("Horizontal");
		
		//Character is moving
		if(motion != 0f)
		{
			Vector3 translate = new Vector3(motion, 0, 0) * chosenSpeed * Time.deltaTime;

			//Movement is right
			if(motion > 0f)
			{
				//Pass in current direction and new direction
				assessDirection(currentDirection, Direction.Right);
			}
			
			//Movement has to be left
			else
			{
				assessDirection (currentDirection, Direction.Left);
				translate *= -1f;	
			}
			
			//Perform the translation
			transform.Translate(translate);

			//Set bool and animation sequence
			isMoving = true;

			if( sprinting )
			{
				framesNotSprinting = 0;

				if ( framesSprinting == 6 )
				{
					gameController.AlterStamina( -2 );
					framesSprinting = 0;
				}

				framesSprinting++;
				anim.SetBool("running", true);
				anim.SetBool("walking", false);
				chosenSpeed = sprintSpeed;
			}
			else
			{

				if( !jumpController.jumping ) 
				{
					anim.SetBool("running", false);
					anim.SetBool("walking", true);
				}

				chosenSpeed = speed;
			}
			
		}
		else
		{
			isMoving = false;
			anim.SetBool("walking", false);
			anim.SetBool ("running", false);
		}
	}

	private void CheckClimbing()
	{
		float vertical = Input.GetAxis( "Vertical" );

		//Climb
		if( vertical > 0f || vertical < 0f )
		{
			//Set the layer based on the sprites movement
//			if ( vertical > 0f ) 
//			{ 
//				ladderDirection = Direction.Up;
//				actualSprite.layer = 18; 	//passable
//			}	
//			else 
//			{
//				actualSprite.layer = 18;				  //not passable
//				ladderDirection = Direction.Down;
//			}

			transform.Translate( new Vector3( 0f, .01f * vertical * speed, 0f ) );
			//anim.SetBool( "on_ladder", false );
			anim.SetBool( "climbing", true);
		}
		else
		{
			anim.SetBool( "climbing", false );
		}
	}
	
	private bool CheckCrouch()
	{
		if( Input.GetButton( "Crouch" ) )
		{
			anim.SetBool( "crouching", true );
			return true;
		}
		else if( Input.GetButtonUp( "Crouch" ) )
		{
			anim.SetBool( "crouching", false );
			return false;
		}
		
		return false;
	}

	private bool CheckSprint()
	{
		if( jumpController.jumping )
			return false;

		if( cantSprint )
		{
			if( Input.GetButtonUp( "Sprint" ) )
			{
				cantSprint = false;
			}

			return false;
		}

		if( gameController.characterStats.stamina < 2 )
		{
			if( Input.GetButton( "Sprint" ) )
			{
				cantSprint = true;
			}

			return false;
		}

		if( Input.GetButton("Sprint") )
		{
  			return true;
		}

		else if( Input.GetButtonUp( "Sprint" ) )
		{
			gameController.AlterStamina( -2 );
			framesSprinting = 0;
			return false;
		}

		return false;
	}
	
	void assessDirection(Direction directionNow, Direction newDirection)
	{
		//If we have changed directions, we need to rotate the character
		if(directionNow != newDirection)
		{

			//Create horizontal flip vector
			Vector3 flip = new Vector3(180f, 0f, 180f);
			
			//Flip character and all children
			transform.Rotate(flip);
			
			//Change direction to correct direction
			currentDirection = newDirection;
		}
	}
	
	public Direction getDirection()
	{
		return currentDirection;
	}

	public int GetDirectionAsInt()
	{
		if ( currentDirection == Direction.Right ) return 1;
		else return -1;
	}

	public void SetHitAnim()
	{
		freeze = true;
		anim.SetTrigger( "hit_trigger" );
		anim.SetBool("hit", true);
	}

	public void UnFreeze( )
	{
		freeze = false;
	}

	public void SetIdleFalse()
	{
		anim.SetBool("idle", false);
	}

	public void MoveToLadder( GameObject entry )
	{
		topLadderEntry = entry;
		Rigidbody2D rigidBody = GetComponent< Rigidbody2D >( );
		rigidBody.gravityScale = 0;
		onLadder = true;
		actualSprite.layer = 18;
		anim.SetBool("onLadder", true);
	}

	public void LeaveLadder()
	{
		topLadderEntry.SetActive( true );
		Rigidbody2D rigidBody = GetComponent< Rigidbody2D >( );
		rigidBody.gravityScale = 2;
		onLadder = false;
		anim.SetBool( "onLadder", false );
		anim.SetBool ( "climbinb", false );
	}

	public void EnterIntoLockpicking( )
	{
		anim.SetBool("crouching", true);
		freeze = true;
	}
}