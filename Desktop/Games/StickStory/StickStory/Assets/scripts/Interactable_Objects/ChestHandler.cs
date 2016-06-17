using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestHandler : MonoBehaviour 
{
	public GameObject wholeUI;
	public float[] levels;
	public float[] levelDepths;
	public float angleBounds = .5f;
	public float animationSpeed;
	public Animation shakeChest;
	public float attemptCost = .05f;

	//Level length is based off scaling of lockpick,
	//so if lockpick is being inserted into the second level, 
	//and its x-scale value is .8, then its levelDepth is .2 ( the difference )
	//between the scale of the lockpick and 1
	public float levelDepthBounds = .1f;

	//Which level are we at in the process
	public int currentLevel = 0;
	public int maxLevel = 2;

	private Animator anim;

	// Use this for initialization
	void Start () 
	{
		anim = GetComponent< Animator >( );
	}


	public void Unlock( )
	{

	}

	public bool PickInLocation( float angle )
	{
			//Check if angle is within the sweetspot
		if( angle > ( levels[ currentLevel ] - angleBounds ) && angle < ( levels[ currentLevel ] + angleBounds ) )
		{
			return true;
		}


		return false;
	}

	public bool MeetsDepth( Transform lockPickTransform )
	{
		//Get width, to calculate depth
		float scaleX = lockPickTransform.localScale.x;
		float depth = 1f - scaleX;

		//If it falls within the bounds it meets the depth
		if( depth < ( levelDepths[ currentLevel + 1 ] + levelDepthBounds ) &&  
		   depth > ( levelDepths[ currentLevel + 1 ] - levelDepthBounds ) )
			return true;

		return false;
	}

	public void IncreaseLevel()
	{
		currentLevel++;
	}

	public void ActivateAnimation()
	{
		anim.SetFloat("shakeSpeed", animationSpeed );
		anim.SetBool( "inserting", true );
	}

	public void DeactivateAnimation()
	{
		anim.SetBool( "inserting", false );
	}

	//We want to change the speed with respect to how close we are to the sweet spot
	public void ChangeSpeed( float currentDepthAmount )
	{
		float destinationDepth = levelDepths[ currentLevel + 1 ];
		float ratio = currentDepthAmount / destinationDepth;
		float speed = 1f + ratio * 3f;
		anim.SetFloat( "shakeSpeed", speed );
	}

	public bool ExceedsDepth( float currentDepthAmount )
	{
		if( currentDepthAmount > levelDepths[ currentLevel + 1 ] )
			return true;

		return false;
	}

	public void DeactivateUI()
	{
		wholeUI.SetActive( false );
	}


}
