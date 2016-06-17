using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour 
{


	public int health = 12;
	public int minWigs = 2;
	public int maxWigs = 5;
	public int minKarma = 3;
	public int maxKarma = 5;
	public int collisionDamage = 9;
	public bool active = true;
	public GameObject wig;
	public GameObject karma;
	public float knockBackVelocity = 5f;

	private SpriteRenderer spriteRenderer;
	private Rigidbody2D rg2d;
	private EnemyNavigator enemyNav;
	AudioSource[] clips;

	private GameController gameController;



	// Use this for initialization
	void Start () 
	{
		enemyNav = GetComponent< EnemyNavigator >( );
		gameController = GameObject.FindGameObjectWithTag( "GameController" ).GetComponent< GameController >( );
		clips = GetComponents< AudioSource >( );
		spriteRenderer = GetComponent< SpriteRenderer >( );
		rg2d = GetComponent< Rigidbody2D > ( );
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void ApplyDamage( int damage )
	{

		health -= damage;

		if( health <= 0 )
		{
			clips[ 0 ].Play( );
			active = false;
			rg2d.isKinematic = true;
			gameObject.layer = 14;
			StartCoroutine( "FadeSprite" );
			DropGoods();
		}
		else
		{
			clips[ 1 ].Play( );
		}
	}

	//Wrapper to start delay
	public void StartDelay( )
	{
		enemyNav.StartHitDelay( );
	}

	IEnumerator FadeSprite() 
	{
		for (float f = 1f; f > -1; f -= 0.1f) 
		{
			Color c = spriteRenderer.color;
			c.a = f;
			spriteRenderer.color = c;

			yield return new WaitForSeconds(.2f);
		}

		Destroy( gameObject );
	}

	void DropGoods()
	{
		const float wigBounce = 3f;

		//Spawn wig
		GameObject wigObj = Instantiate( wig, transform.position, Quaternion.identity ) as GameObject;
		WigHandler wigHandler = wigObj.GetComponentInChildren< WigHandler >( );
		wigHandler.value = Random.Range( minWigs, maxWigs );

		//Spawn karma
		GameObject karmaObj = Instantiate( karma, transform.position, Quaternion.identity ) as GameObject;
		KarmaHandler karmaHandler = karmaObj.GetComponentInChildren< KarmaHandler >( );
		karmaHandler.value = Random.Range( minKarma, maxKarma ); 

		//Set velocities
		Rigidbody2D wigBody = wigObj.GetComponent< Rigidbody2D >( );
		wigBody.velocity = new Vector2( 0f, wigBounce );
		Rigidbody2D karmaBody = karmaObj.GetComponent< Rigidbody2D >( );
		karmaBody.velocity = new Vector2( 0f, wigBounce );

		//StartCoroutine( "SpawnWigs" );
	}

	IEnumerator SpawnWigs() 
	{
		const float wigsSeperator = .2f;
		const float wigBounce = 2f;

		int wigsDropped = Random.Range( minWigs, maxWigs );

		float xStart = wigsSeperator * ( float )( wigsDropped / 2 ) * -1;
		Vector3 firstPosition = new Vector3( transform.position.x + xStart, transform.position.y, 0f );

		//Instantiate wig, shift location for spawn and give it a slight bounce
		for (int n = 0; n < wigsDropped; n++) 
		{
			float newX = n * wigsSeperator;
			Vector3 position = new Vector3( firstPosition.x, firstPosition.y, 1f );
			GameObject wigObj = Instantiate( wig, transform.position, Quaternion.identity ) as GameObject;
			Rigidbody2D wigBody = wigObj.GetComponent< Rigidbody2D >( );
			wigBody.velocity = new Vector2( 0f, wigBounce );

			yield return new WaitForSeconds(.05f);
		}
	}

	void OnCollisionEnter2D( Collision2D other)
	{
		if ( other.collider.tag == "Player")
		{
			gameController.AlterCharacterHealth( -1 * collisionDamage );
			KnockBackPlayer( gameController.mainCharacter );
		}
	}

	void KnockBackPlayer( GameObject obj )
	{
		int dir = gameController.characterMovement.GetDirectionAsInt( ) * -1;
		gameController.characterMovement.SetHitAnim( );

		Rigidbody2D rgb2d = obj.GetComponent< Rigidbody2D >( );
		rgb2d.velocity = new Vector2( knockBackVelocity, 0f ) * dir;
	}


}
