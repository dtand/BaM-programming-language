using UnityEngine;
using System.Collections;

public class KarmaHandler : MonoBehaviour 
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
		itemNotifier = GameObject.FindGameObjectWithTag("item_notification_handler").GetComponent<  ItemNotificationHandler >( );
		gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<  GameController >( );
	}
	
	void OnTriggerStay2D( Collider2D other )
	{
		
		if( Input.GetButtonDown( "Pickup" ) && other.tag == "Player" )
		{
			//Make message for notification
			string message = "Karma x " + value.ToString();
			itemNotifier.InstantiateNotification( message );
			gameController.AlterKarma( value );
			Destroy( parent );
		}
	}
}
