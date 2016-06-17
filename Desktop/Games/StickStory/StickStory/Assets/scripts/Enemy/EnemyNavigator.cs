using UnityEngine;
using System.Collections;

public class EnemyNavigator : MonoBehaviour {
	
	public float speed = 10f;
	public Vector3 movementRangeMin;
	public Vector3 movementRangeMax;
	public Movement player;
	
	//Shared with agro control
	public  Movement.Direction currentDirection;
	public bool playerInRange;
	
	//Boundaries for enemy travels
	private Vector3 minBounds;
	private Vector3 maxBounds;
	public float spanTillDecision = 2f;


	public bool hitDelay = false;
	public float hitDelayTime = 1f;
	private float currentDelay;

	private Animator anim;
	private Rigidbody2D rigidBody;
	private bool isMoving;
	private bool isJumping;
	private float decideMovementTimer;

	private EnemyController enemyController;
	
	
	//Get motion based on input, speed and time delay
	private float motion = .1f;
	
	
	// Use this for initialization
	void Start () 
	{
		enemyController = GetComponent< EnemyController >( );
		enemyController.active = true;

		//Init boundaries
		minBounds = transform.position - movementRangeMin;
		maxBounds = transform.position + movementRangeMax;

		//player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement>();
		anim = GetComponent<Animator> ();
		rigidBody = GetComponent<Rigidbody2D>();
		currentDirection = Movement.Direction.Left;
		isMoving = false;
		isJumping = false;
		playerInRange = false;
		decideMovementTimer = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If waiting for hit delay, skip motion
		if( hitDelay )
		{
			CheckHitDelay( );
			return;
		}

		if( !enemyController.active ) return;
		updateMotion();
	}

	void CheckHitDelay( )
	{
		if( Time.time - currentDelay >= hitDelayTime )
		{
			hitDelay = false;
		}
	}

	//Called from controller to activate hit
	public void StartHitDelay( )
	{
		playerInRange = true;
		hitDelay = true;
		currentDelay = Time.time;
	}
	
	
	void updateMotion()
	{
		
		//Follow player
		if(playerInRange)
		{
			followPlayer();
			move ();
		}
		else
		{
			if(Time.time - decideMovementTimer >= spanTillDecision)
			{
				decideMovement();
				decideMovementTimer = Time.time;
			}
			
			move ();
		}
	}
	
	void move()
	{
		Vector3 translate = new Vector3(motion, 0, 0) * speed * Time.deltaTime;
		
		//Set bool and animation sequence
		isMoving = true;
		
		if(!isJumping)
			anim.SetBool("walking", true);
		
		//Movement is right
		if(motion > 0f)
		{
			//Pass in current direction and new direction
			assessDirection(currentDirection, Movement.Direction.Right);
			translate *= -1f;
		}
		
		//Movement has to be left
		else
		{
			assessDirection (currentDirection, Movement.Direction.Left);	
		}
		
		//Perform the translation
		transform.Translate(translate);
		
		//Clamp position to boundaries
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, 
		                                             minBounds.x, 
		                                             maxBounds.x), 
		                                 Mathf.Clamp(transform.position.y, 
		            minBounds.y, 
		            maxBounds.y), 
		                                 0f);
	}
	
	void followPlayer()
	{
		float playerX = player.transform.position.x;
		float enemyX = transform.position.x;
		
		//Distance is negative, move left
		if(playerX < enemyX)
		{
			//Does not see player
			if(currentDirection == Movement.Direction.Right)
				return;
			
			motion = -.01f;
		}
		else
		{
			//Does not see player
			if(currentDirection == Movement.Direction.Left)
				return;
			
			motion =  .01f;
		}
	}
	
	void decideMovement()
	{
		//Choose 1 or 2 to denote direction
		float action = Random.Range(0f,1f);
		
		if(action <=  .4f)	motion = .01f;
		else if(action <= .8f && action > .4f) motion = -.01f;
		else if(action > .8f) motion = 0f;
	}
	
	void assessDirection(Movement.Direction directionNow, 
	                     Movement.Direction newDirection)
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
	
}
