using UnityEngine;
using System.Collections;

public class BackgroundCreator : MonoBehaviour {
	//Place a gameobject with a sprite renderer attached and the grass tile in that. 
	//Create it into a prefab and place it into your "Resources" folder.
	public GameObject tilePrefab;
	
	//Tilemap width and height
	public int mapWidth = 5;
	public int mapHeight = 5;
	
	//Size you want your tile in unity units
	public float tileSize = 1.28f;
	
	//2D array to hold all tiles, which makes it easier to reference adjacent tiles etc.
	public GameObject[,] map;

	public void Build()
	{
		//Initialize our 2D Transform array with the width and height
		map = new GameObject[mapWidth, mapHeight];
		GameObject platform = new GameObject("Platform");
		Instantiate( platform ,Vector3.zero, Quaternion.identity );


		//Iterate over each future tile positions for x and y
		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				//Instantiate tile prefab at the desired position as a Transform object
				GameObject tile = Instantiate (tilePrefab, new Vector3 ( x * tileSize, y * tileSize, 0), Quaternion.identity) as GameObject;
				//Set the tiles parent to the GameObject this script is attached to
				tile.transform.parent = platform.transform;
				//Set the 2D map array element to the current tile that we just created.
				map[x, y] = tile;
			}
		}
	}
	
	//Returns a tile from the map array at x and y
	public GameObject GetTileAt (int x, int y)
	{
		if (x < 0 || y < 0 || x > mapWidth || y > mapHeight)
		{
			Debug.LogWarning ("X or Y coordinate is out of bounds!");
			return null;
		}
		return map[x, y];
	}
	
	void Update () 
	{
		
	}
}
