using UnityEngine;
using System.Collections;

public class MiniMapDrag : MonoBehaviour {

	public GameObject mouseTracker;
	public CameraMover cameraMover;
	public Camera cam;
	public float factor;

	bool hasPosition;
	Vector3 startDrag;
	Vector3 posAtStartDrag;

	// Use this for initialization
	void Start () 
	{
		hasPosition = false;
	}

	void OnMouseEnter()
	{
		Debug.Log( "Mouse Enter" );
	}

	void OnMouseDrag( )
	{
		//Vector3 mousePosition = cam.ScreenToViewportPoint( Input.mousePosition ) * factor;
		//mouseTracker.transform.position = mousePosition;
		MoveCamera( );
	}

	void MoveCamera( )
	{
		Vector3 screenMousePos = cam.ScreenToViewportPoint( Input.mousePosition );
		float difY = screenMousePos.y - posAtStartDrag.y;
		float difX = screenMousePos.x - posAtStartDrag.x;
		Vector3 translation = new Vector3( difX, difY, 0f );

		cam.transform.position = ( posAtStartDrag + translation ) * factor;
	}

	void OnMouseDown()
	{
		cameraMover.freeze = true;
		mouseTracker.SetActive( true );
		cameraMover.target = mouseTracker.transform;
		startDrag = cam.ScreenToViewportPoint( Input.mousePosition );
		posAtStartDrag = cam.transform.position;
	}

	void OnMouseUp()
	{
		cameraMover.freeze = false;
		mouseTracker.SetActive( false );
		cameraMover.ResetTarget( );
	}
}
