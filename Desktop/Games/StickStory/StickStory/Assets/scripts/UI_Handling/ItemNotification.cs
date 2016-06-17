using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemNotification: MonoBehaviour 
{

	public float verticalSpeed = 1f;
	private Text text;


	// Use this for initialization
	void Start () 
	{
		text = GetComponent< Text > ( );
		StartCoroutine( "FadeTextIn" );
	}

	//Slowly translate text vertically
	void Update()
	{
		transform.Translate( new Vector3( 0f, .01f, 0f ) * verticalSpeed * Time.deltaTime );
	}
	
	IEnumerator FadeTextOut() 
	{

		//Fadeout faster than in
		for (float f = 1f; f > -1; f -= 0.1f) 
		{
			Color c = text.color;
			c.a = f;
			text.color = c;

			yield return new WaitForSeconds( .25f );
		}

		Destroy( gameObject );
	}

	IEnumerator FadeTextIn() 
	{
		for (float f = 1f; f < 1f; f += 0.05f) 
		{
			Color c = text.color;
			c.a = f;
			text.color = c;

			//Perform longer wait if we are at full visibility
			if( f > .99f )
				yield return new WaitForSeconds( 3f );
			else
				yield return new WaitForSeconds( .1f );
		}

		//Begin the fadout
		StartCoroutine( "FadeTextOut" );
	}
}
