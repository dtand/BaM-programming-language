using UnityEngine;
using System.Collections;

public class WeaponHandler : MonoBehaviour {

	public Movement characterMovement;
	public float knockBackVelocity = 5f;
	public int weaponDamage = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter2D( Collider2D other )
	{
		if( other.tag == "enemy")
		{
			KnockBack( other.gameObject );

			EnemyController e = other.gameObject.GetComponent< EnemyController > ( );
			e.StartDelay( );
			e.ApplyDamage( weaponDamage );
		}
	}

	void KnockBack( GameObject obj )
	{
		float dir = 1;

		//Alter direction based on characters direction
		if( characterMovement.getDirection( ) == Movement.Direction.Left ) dir = -1;

		Rigidbody2D rgb2d = obj.GetComponent< Rigidbody2D >( );
		rgb2d.velocity = new Vector2( knockBackVelocity * dir, 0f );
	}
}
