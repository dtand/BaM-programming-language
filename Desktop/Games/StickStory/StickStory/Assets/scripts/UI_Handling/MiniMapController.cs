using UnityEngine;
using System.Collections;

public class MiniMapController : MonoBehaviour {

	public GameObject miniMap;
	public GameObject miniMapUI;
	private bool isMinimized;


	// Use this for initialization
	void Start () 
	{
		isMinimized = false;
	}

	public void OnClick()
	{

		if( isMinimized ) Maximize( );

 		else Minimize( );
 	}
	private void Minimize()
	{
		isMinimized = true;
		miniMap.SetActive( false );
		miniMapUI.SetActive( false );
	}

	private void Maximize()
	{
		isMinimized = false;
		miniMapUI.SetActive( true );
		miniMap.SetActive( true );
	}
}
