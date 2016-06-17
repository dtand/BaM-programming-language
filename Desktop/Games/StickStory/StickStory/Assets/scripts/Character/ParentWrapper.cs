using UnityEngine;
using System.Collections;

public class ParentWrapper : MonoBehaviour 
{
	public GameObject refToParent;
	public Movement movement;
	public Animator anim;

	void Start()
	{
		anim = GetComponent< Animator >( );
		movement = refToParent.GetComponent< Movement >( );
	}

	public void UnFreezeParent( )
	{
		anim.SetBool( "hit", false );
		movement.UnFreeze( );
	}

	public void ResetAllAnim( )
	{
//		anim.SetBool( "walking", false );
//		anim.SetBool ( "attacking", false );
//		anim.SetBool ( "jumping", false );
	}


}
