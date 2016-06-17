using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour {

	private Animator anim;
	public GameObject hitBox1;
	public GameObject hitBox2;

	// Use this for initialization
	void Start () 
	{
		anim = GetComponent< Animator >( );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( Input.GetButton( "Fire1" ) )
		{
			anim.SetBool( "attacking", true );
		}
		else if( Input.GetButtonUp( "Fire1" ) )
		{
			anim.SetBool( "attacking", false );
			SetHitBox( 0 );
		}
	}

	public void SetHitBox( int state )
	{
		bool flag = false;

		if( state != 0 ){
			flag = true;
		}


		hitBox1.SetActive( flag );
		hitBox2.SetActive( flag );
	}
}
