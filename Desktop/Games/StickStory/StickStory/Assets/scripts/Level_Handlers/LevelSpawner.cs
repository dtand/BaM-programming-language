using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LevelSpawner : NetworkBehaviour {

	public GameObject levelPrefab;

	public override void OnStartServer()
	{
		GameObject level = Instantiate( levelPrefab );
		NetworkServer.Spawn( level );
	}

}
