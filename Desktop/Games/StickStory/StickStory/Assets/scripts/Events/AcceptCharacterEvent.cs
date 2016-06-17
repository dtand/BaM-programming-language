using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AcceptCharacterEvent : MonoBehaviour {

	public GameObject customizer;
	public GameObject door;
	private CanvasGroup canvasGroup;
	private AudioSource gateOpen;

	public AudioSource bgMusic;

	// Use this for initialization
	void Start () {
		gateOpen = GetComponent< AudioSource >( );
		canvasGroup = customizer.GetComponent< CanvasGroup >( );
	}
	
	// Update is called once per frame
	void Update () {

	}

	IEnumerator FadeWindow() 
	{
		for (float f = 1f; f > -1; f -= 0.1f) 
		{
			canvasGroup.alpha = f;
			bgMusic.volume = f;

			if( f <= .0f )
			{
				gateOpen.Play( );
				Animator doorAnimator = door.GetComponent< Animator >( );
				doorAnimator.SetTrigger( "open" );
			}

			yield return new WaitForSeconds(.2f);
		}
	}

	public void OnAccept()
	{
		StartCoroutine( "FadeWindow" );
	}


}
