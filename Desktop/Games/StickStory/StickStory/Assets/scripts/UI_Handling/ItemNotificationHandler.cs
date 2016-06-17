using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameObjectWithMessage
{
	public string message;
	public bool processing;

	public GameObjectWithMessage( string m, bool p )
	{
		processing = p;
		message = m;
	}
}

public class ItemNotificationHandler : MonoBehaviour {

	//Text to be sent
	public GameObject childText;

	//Position that must be passed before instantiating new text
	public float waitTime;
	private float startTime;

	//Holds messages waiting to be sent
	private Queue< GameObjectWithMessage > messageQueue;

	void Start()
	{
		messageQueue = new Queue< GameObjectWithMessage >( 16 );	
	}

	public void InstantiateNotification( string message )
	{
		//Initial Case, go ahead and instantiate, and push back
		//currently processed message data
		if( messageQueue.Count == 0 )
		{
			startTime = Time.time;
			_InstantiateNotification( message );
			messageQueue.Enqueue( new GameObjectWithMessage( message, true  ) );
			StartCoroutine( "ProcessMessageQueue" );
			return;
		}

		//Push new message onto queue
		messageQueue.Enqueue( new GameObjectWithMessage( message, false  ) );
	}

	//Internal function called if handler is ready to instantiate
	void _InstantiateNotification( string message )
	{
		Text childMessage = childText.GetComponent< Text >( );
		childMessage.text = message;

		GameObject g = Instantiate( childText ) as GameObject;
		g.transform.SetParent( gameObject.transform, false );
	}

	//Check if Queue needs to be processed
	void Update()
	{
		if( messageQueue.Count == 0 ) return;

		//Process the queue, sets timers,
		//and moves to next message if needed
		ProcessMessageQueue( );
	}

	//Called in update to check queue processing
	void ProcessMessageQueue() 
	{
		//Check if timer is good
		if( Time.time - startTime >  waitTime )
		{
			//Checks for another message and dequeues current
			if( SetNextInQueue( ) )
			{
				_InstantiateNotification( messageQueue.Peek().message );
			}
		}
	}

	//Dequeues the queue, and inits next message
	bool SetNextInQueue()
	{
		messageQueue.Dequeue();
		bool isEmpty = messageQueue.Count == 0;

		//Set next to being processed and set time started
		if( !isEmpty ) 
		{
			startTime = Time.time;
			messageQueue.Peek( ).processing = true;
		}

		return !isEmpty;
	}
}
