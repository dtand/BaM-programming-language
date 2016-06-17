using UnityEngine;
using System.Collections;

public class AgroController : MonoBehaviour 
{
	public EnemyNavigator enemyNavigator;	
	
	void OnTriggerStay2D(Collider2D other)
	{
		//If player in collider and enemy is looking at player
		if(other.gameObject.tag == "Player")
		{
			enemyNavigator.playerInRange = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			enemyNavigator.playerInRange = false;
		}
	}
}
