using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponOffsetContainer : MonoBehaviour 
{
	public SpriteRenderer wielder;
	public List< List<AnimationComponent> > lists;
	public List< AnimationComponent > sword;
	public List< AnimationComponent > swordOffsets;
	public List< AnimationComponent > dagger;
	public List< AnimationComponent > twoHanded;
	public List< AnimationComponent > staff;
	public List< AnimationComponent > bow;

	public int weapon;
	public int state;
	public float currentRotation;
	public Sprite currentFrame;

	private Quaternion defaultRot;

	/*== TABLE ==//
	Weapons:
	0 => sword
	1 => dagger
	2 => 2h weapon
	3 => staff
	4 => bow

	States:
	0 => idle_1
	1 => idle_2
	//== END TABLE ==*/

	void Start()
	{
		defaultRot = transform.localRotation;
	}

	public void ChangeFrame()
	{
		wielder.sprite = currentFrame;
	}

	//Saves local position into correct position
	public void Add( )
	{
		AnimationComponent stateToAdd = new AnimationComponent( transform.localPosition, currentRotation );

		switch( weapon )
		{
		case 0: sword[ state ] =  stateToAdd; break;
		case 1: dagger[ state ] = stateToAdd; break;
		case 2: twoHanded[ state ] = stateToAdd; break;
		case 3: staff[ state ] = stateToAdd; break;
		case 4: bow[ state ] = stateToAdd; break;
		default: break;
		}

		state++;
	}

	public void SetCurrentTransform( )
	{
		Vector3 position = Vector3.zero;
		float rot = 0f;

		switch( weapon )
		{
		case 0: position = sword[ state ].posDifference;  rot = sword[ state ].newRotation; break;
		case 1: position = dagger[ state ].posDifference; rot = dagger[ state ].newRotation; break;
		case 2: position = twoHanded[ state ].posDifference; rot = twoHanded[ state ].newRotation; break;
		case 3: position = staff[ state ].posDifference; rot = staff[ state ].newRotation; break;
		case 4: position = bow[ state ].posDifference; rot = bow[ state ].newRotation; break;
		default: break;
		}

		//Set the values
		transform.localPosition = position;
		transform.localRotation = defaultRot; 
		transform.Rotate ( new Vector3 ( 0f, 0f, rot ) );
	}

	public void CalculateOffsets()
	{
		switch( weapon )
		{
		case 0: _CalculateOffsets( swordOffsets, sword ); break;
		default: break;
		}
	}

	private void _CalculateOffsets( List<AnimationComponent> offsets, List<AnimationComponent> positions )
	{
		Vector3 defaultPosition = positions[ 0 ].posDifference;
		AnimationComponent defaultComponent = new AnimationComponent( new Vector3(0f,0f,0f), positions[ 0 ].newRotation );

		//Calucalate offsets based on default position
		for( int i = 1; i < positions.Count; i++)
		{
			float offsetX = positions[ i ].posDifference.x - defaultPosition.x;
			float offsetY = positions[ i ].posDifference.y - defaultPosition.y;

			AnimationComponent componentWithOffset = new AnimationComponent( new Vector3( offsetX, offsetY, 0f ), 
			                                                                positions[ i ].newRotation );

			offsets[ i ] = componentWithOffset;
		}

	}

	public void TestState( )
	{
		ChangeFrame( );
		switch( weapon )
		{
		case 0: _TestState( swordOffsets, sword[ 0 ].posDifference ); break;
		default: break;
		}
	}

	private void _TestState( List< AnimationComponent > offsets, Vector3 defaultPos)	
	{
		float rot = offsets[ state ].newRotation;

		//Set the values
		//transform.localPosition = defaultPos + offsets[ state ].posDifference;
		transform.localPosition = sword[ state ].posDifference;
		transform.localRotation = defaultRot; 
		transform.Rotate ( new Vector3 ( 0f, 0f, rot ) );

	}


}
