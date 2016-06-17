using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LockpickController : MonoBehaviour 
{
	public Text angleText;
	public Text depthText;
	public Text durabilityText;

	public GameObject parent;
	public ChestHandler chestHandler;
	public float rateOfInsertion;
	public float insertionDepthOnFail;

	//Speeds to check against
	public float speedOfInsertion;
	public float speedOfRetraction;

	private float insertTimeTracker;

	//Helpful flags
	private bool inserting;
	private bool angleGood;
	private bool badInsertion;
	private bool waitForKeyUp;

	private float durability;

	// Use this for initialization
	void Start () 
	{
		durability = 1f;
		inserting = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckForCorrectDepth( );

		//Sets insertion time, inserting flag, and checks for a good angle
		if( Input.GetButtonDown( "Jump" ) && !waitForKeyUp )
		{

			inserting = true;
			insertTimeTracker = Time.time;
			chestHandler.ActivateAnimation();

			float angle = parent.transform.eulerAngles.z;

			angle = angle % 360;

			if( angle < 0 )
			{
				angle += 360;
			}

			Debug.Log( "Angle: " + angle );
			//If key is in the sweet spot of the keyhole, proceed to 
			//reduce the width of the lockpick
			angleGood = chestHandler.PickInLocation( angle );
			badInsertion = false;
		}


		//Allow user to attempt to proceed to next level
		if( inserting && Input.GetButton( "Jump" ) )
		{

			//Go ahead and run the insertion to at least the fail marker
			if ( ( Time.time - insertTimeTracker ) > speedOfInsertion )
			{
				parent.transform.localScale = new Vector3( parent.transform.localScale.x - rateOfInsertion, 1f, 1f );
				insertTimeTracker = Time.time;
				float currentDepth = 1f - parent.transform.localScale.x;
				chestHandler.ChangeSpeed( currentDepth );
				depthText.text = currentDepth.ToString( );

				//Check for the fail marker => set fail marker if angle is bad, or if depth is exceeded
				if( parent.transform.localScale.x <= 1f - insertionDepthOnFail && !angleGood ||
				   chestHandler.ExceedsDepth( currentDepth )  )
				{
					badInsertion = true;
					inserting = false;
					waitForKeyUp = true;
				}
			}
		}

		//Performs retraction, and resets flags
		else if( badInsertion )
		{
			chestHandler.DeactivateAnimation();

			//Scale only if time allows, runs a retraction animation
			if ( ( Time.time - insertTimeTracker ) > speedOfRetraction )
			{
				parent.transform.localScale = new Vector3( parent.transform.localScale.x + rateOfInsertion, 1f, 1f );
				insertTimeTracker = Time.time;
				float currentDepth = 1f - parent.transform.localScale.x;
				depthText.text = currentDepth.ToString( );
			}

			float x = parent.transform.localScale.x;

			//Action complete => reset flags and update durability of lockpick
			if(x == 1f )
			{
				durability -= chestHandler.attemptCost;
				int percent = ( int ) Mathf.Ceil( durability * 100f );
				durabilityText.text = percent.ToString() + "%";
				inserting = false;
				badInsertion = false;
			}
		}
	}

	void CheckForCorrectDepth()
	{
		//Check if insertion was done correct, and set corresponding 
		//flags
		if( Input.GetButtonUp( "Jump" ) )
		{
			chestHandler.DeactivateAnimation();
			//We had a bad insertion, and no need to check
			if( waitForKeyUp )
			{
				waitForKeyUp = false;
				return;
			}

			if( chestHandler.MeetsDepth( parent.transform ) && angleGood )
			{
				chestHandler.IncreaseLevel( );
				inserting = false;
				badInsertion = false;
			}
			else
				badInsertion = true;
		}
	}

	void OnMouseDrag()
	{

		if( inserting ) return;

		Vector3 mouse_pos = Input.mousePosition;
		Vector3 rotatorPos = Camera.main.WorldToScreenPoint(parent.transform.position);
		mouse_pos.x = mouse_pos.x - rotatorPos.x;
		mouse_pos.y = mouse_pos.y - rotatorPos.y;
		float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
		parent.transform.rotation = Quaternion.Euler( new Vector3(0f, 0f, angle) );
	
		//Convert angle and set to text
		angle = angle % 360;
		
		if( angle < 0 )
		{
			angle += 360;
		}
		angleText.text = angle.ToString( );
	}

}
