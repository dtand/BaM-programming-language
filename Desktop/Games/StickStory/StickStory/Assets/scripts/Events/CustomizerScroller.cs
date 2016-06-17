using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CustomizerScroller : MonoBehaviour {

	public Vector3 offset;

	public List< string > names;
	public List< Sprite > sprites;
	public List< Vector3 > positions;
	public GameObject correspondingGameObject;
	public Text text;


	public SpriteRenderer thingToChange;
	private int current;

	public ApparelAnimator appAnim;

	// Use this for initialization
	void Start () 
	{
		current = 0;
	}
	
	public void ScrollLeft(int which)
	{
		if( current == 0 )
		{
			current = names.Count - 1;
		} else current--;

		text.text = names[ current ];
		thingToChange.sprite = sprites [ current ];
		correspondingGameObject.transform.localPosition = positions[ current ];

		if( which == 0)
			appAnim.UpdateHairHandle( positions[ current ] + offset );
		else 
			appAnim.UpdateWeaponHandle( positions[ current ] + offset );
	}

	public void ScrollRight(int which)
	{
		if( current == names.Count - 1 )
		{
			current = 0;
		} else current++;
		
		text.text = names[ current ];
		thingToChange.sprite = sprites [ current ];
		correspondingGameObject.transform.localPosition = positions[ current ];

		if( which == 0)
			appAnim.UpdateHairHandle( positions[ current ] + offset );
		else 
			appAnim.UpdateWeaponHandle( positions[ current ] + offset );
	}
}
