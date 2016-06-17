using UnityEngine;
using System.Collections;

public class WigHandler : MonoBehaviour 
{

	public ItemNotificationHandler itemNotifier;

	public GameObject parent;
	private GameController gameController;
	public Sprite goldSprite;
	public int value;
	private Sprite silverSprite;


	// Use this for initialization
	void Start () 
	{
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<  GameController >( );
		itemNotifier = GameObject.FindGameObjectWithTag("item_notification_handler").GetComponent<  ItemNotificationHandler >( );
	}

	void OnTriggerStay2D( Collider2D other )
	{

		if( Input.GetButtonDown( "Pickup" ) && other.tag == "Player" )
		{
			//Make message for notification
			string message = "Wigs x " + value.ToString();

			gameController.AlterWigs( value );
			itemNotifier.InstantiateNotification( message );
			Destroy( parent );
		}
	}
}
