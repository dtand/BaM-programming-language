using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AnimationComponent
{
	public Vector3 posDifference;
	public float newRotation;
	
	public AnimationComponent()
	{
		posDifference = Vector3.zero;
		newRotation = 0f;
	}
	
	public AnimationComponent(Vector3 pos, float rot)
	{
		posDifference = pos;
		newRotation = rot;
	}
	
}

[System.Serializable]
public class DoubleInt
{
	public int int1;
	public int int2;

	public DoubleInt(int x, int y)
	{
		int1 = x;
		int2 = y;
	}

}

public class ApparelAnimator : MonoBehaviour {

	public WeaponOffsetContainer weaponPositions;
	public int weaponType = 0;
	public GameObject hair;
	public GameObject weapon;

	private Animator animator;
	private AnimatorStateInfo prevInfo;
	private AnimatorStateInfo currentInfo;

	//Movement states

	
	private List< AnimationComponent > lookupComponentWeapon;
	private  List< AnimationComponent > lookupComponentHair;
	private Vector3 weaponDefaultPosition;
	private Vector3 hairDefaultPosition;
	private Quaternion weaponDefaultQuaternion;
	private Quaternion hairDefaultQuaternion;



	// Use this for initialization
	void Start () 
	{

		animator = GetComponent< Animator >( );
		lookupComponentWeapon = new List< AnimationComponent >( );
		lookupComponentHair = new List< AnimationComponent >( );

		prevInfo = animator.GetCurrentAnimatorStateInfo( 0 );
		currentInfo = prevInfo;

		//STAND / IDLE ==> 0
		lookupComponentWeapon.Add( new AnimationComponent( new Vector3( 0f, .0f, 0f), 0f ) );
		lookupComponentHair.Add( new AnimationComponent( new Vector3( 0f, .0f, 0f), 0f ) );

		//step_1 ==> 1
		lookupComponentWeapon.Add( new AnimationComponent( new Vector3( -0.18f, -0.07f, 0f), -12f ) );
		lookupComponentHair.Add( new AnimationComponent( new Vector3( 0f, .0f, 0f), 0f ) );

		//step_2 ==> 2
		lookupComponentWeapon.Add( new AnimationComponent( new Vector3( 0.01f, -0.04f, 0f), -9f ) );
		lookupComponentHair.Add( new AnimationComponent( new Vector3( 0f, .0f, 0f), 0f ) );

		//step_3 ==> 3
		lookupComponentWeapon.Add( new AnimationComponent( new Vector3( 0.14f, 0.05f, 0f), 5f ) );
		lookupComponentHair.Add( new AnimationComponent( new Vector3( 0f, .0f, 0f), 0f ) );

		//step_4 ==> 4
		lookupComponentWeapon.Add( new AnimationComponent( new Vector3( -.11f, 0.37f, 0f), 50f ) );
		lookupComponentHair.Add ( new AnimationComponent( new Vector3( 0f, -.04f, 0f), 0f ) );

		lookupComponentWeapon.Add( new AnimationComponent( new Vector3( 0f, .0f, 0f), 0f ) );
		lookupComponentHair.Add( new AnimationComponent( new Vector3( 0f, .0f, 0f), 0f ) );
		
		//1h_1 ==> 5
		lookupComponentWeapon.Add( new AnimationComponent( new Vector3( 0f, .55f, 0f), 54f ) );
		lookupComponentHair.Add( new AnimationComponent( new Vector3( 0f, .0f, 0f), 0f ) );
		
		//1h_2 ==> 6
		lookupComponentWeapon.Add( new AnimationComponent( new Vector3( -.32f, 1.05f, 0f), 86f ) );
		lookupComponentHair.Add( new AnimationComponent( new Vector3( 0f, .0f, 0f), 0f ) );
		
		//1h_3 ==> 7
		lookupComponentWeapon.Add( new AnimationComponent( new Vector3( -1.15f, -0.05f, 0f), 120.6f ) );
		lookupComponentHair.Add( new AnimationComponent( new Vector3( 0f, .0f, 0f), 0f ) );
		
		//1h_4 ==> 8
		lookupComponentWeapon.Add( new AnimationComponent( new Vector3( .4f, .58f, 0f), 2f ) );
		lookupComponentHair.Add ( new AnimationComponent( new Vector3( 0f, 0f, 0f), 0f ) );

		//1h_5 ==>9
		lookupComponentWeapon.Add( new AnimationComponent( new Vector3( .55f, 0.7f, 0f),2.4f ) );
		lookupComponentHair.Add ( new AnimationComponent( new Vector3( 0f, 0f, 0f), 0f ) );




		weaponDefaultPosition = weapon.transform.localPosition;
		hairDefaultPosition = hair.transform.localPosition;
		weaponDefaultQuaternion = weapon.transform.localRotation;
		hairDefaultQuaternion = hair.transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	//Looks up stored data in hash and applies it
	public void ChangeApparelState( int index )
	{
		weaponPositions.weapon = weaponType;		
		weaponPositions.state = index;
		weaponPositions.SetCurrentTransform ( );

		return;
//		AnimationComponent test = lookupComponentWeapon[ index ];
//
//		float newX = weaponDefaultPosition.x + test.posDifference.x;
//		float newY = weaponDefaultPosition.y + test.posDifference.y;
//
////		weapon.transform.localPosition = weaponDefaultPosition + test.posDifference;
//		weapon.transform.localPosition = new Vector3( newX, newY, 1f );
//		weapon.transform.localRotation = weaponDefaultQuaternion;
//		weapon.transform.Rotate( new Vector3( 0f, 0f,  test.newRotation ) );
//
//		test = lookupComponentHair[ index ];
//
//		hair.transform.localPosition = hairDefaultPosition + test.posDifference;
//		//hair.transform.Rotate( new Vector3( 0f, 0f, test.newRotation ) );
	}

//	public void ChangeApparelState( int index, int type )
//	{
//		weaponPositions.weapon = type;		
//		weaponPositions.state = index;
//		weaponPositions.SetCurrentTransform ( );
//
//	}

	public void UpdateWeaponHandle( Vector3 position )
	{
		weaponDefaultPosition = position;
	}

	public void UpdateHairHandle( Vector3 position )
	{
		hairDefaultPosition = position;
	}


}
