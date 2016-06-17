using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarController : MonoBehaviour 
{
	
	//Keep track of starting dimmensions
	private Vector3 defaultScale;
	private RectTransform rectTransform;
	private float factor;
	
	// Use this for initialization
	void Start () 
	{
		rectTransform = GetComponent< RectTransform >( );
		defaultScale = rectTransform.localScale;
		//factor = 1f / defaultScale.x;
	}
	
	// Update is called once per frame
	public void UpdateBarWidth ( float frac ) 
	{

		float newX = frac * defaultScale.x;
		rectTransform.localScale = new Vector2( newX, rectTransform.localScale.y );
	}
}
