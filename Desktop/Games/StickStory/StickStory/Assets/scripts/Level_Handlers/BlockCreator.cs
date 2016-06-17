using UnityEngine;
using System.Collections;

public class BlockCreator : MonoBehaviour 
{
	//Place a gameobject with a sprite renderer attached and the grass tile in that. 
	//Create it into a prefab and place it into your "Resources" folder.
	public GameObject tilePrefabTopMid;
	public GameObject tilePrefabTopLeft;
	public GameObject tilePrefabTopRight;

	public GameObject tilePrefabMidLeft;
	public GameObject tilePrefabMidMid;
	public GameObject tilePrefabMidRight;

	public GameObject tilePrefabBottomLeft;
	public GameObject tilePrefabBottomMid;
	public GameObject tilePrefabBottomRight;

	public GameObject tilePrefabConnectRight;
	public GameObject tilePrefabConnectLeft;

	public GameObject tilePrefabConnectRightFromHigh;
	public GameObject tilePrefabConnectLeftFromHigh;


	//Tells us what we want for collider
	public bool OnlyCollideTop = false;

	// 0 -> none
	// 1 -> left
	// 2 -> right
	public int lowerConnection = 0;

	// 0 -> none
	// 1 -> left
	// 2 -> right
	public int higherConnection = 0;

	//At what row will this block connect? => only used for higherConnection
	public int connectingRow = -1;
	
	//Tilemap width and height
	public int blockWidth = 5;
	public int blockHeight = 5;
	
	//Size you want your tile in unity units
	public float tileSize = 1.28f;

	public int blockLayer = 3;
	
	//2D array to hold all tiles, which makes it easier to reference adjacent tiles etc.
	public GameObject[,] map;

	private float offset = .64f;
	
	void Start () 
	{

	}
	
	public void Build()
	{
		//Initialize our 2D Transform array with the width and height
		map = new GameObject[blockWidth, blockHeight];
		GameObject platform = new GameObject("Terrain");
		Instantiate( platform ,Vector3.zero, Quaternion.identity );
		
		
		//Iterate over each future tile positions for x and y
		for (int y = 0; y < blockHeight; y++)
		{
			for (int x = 0; x < blockWidth; x++)
			{
				//Decides which prefab 
				GameObject thisPrefab = DecideTile( x, y );

				//Instantiate tile prefab at the desired position as a Transform object
				GameObject tile = Instantiate (thisPrefab, new Vector3 ( x * tileSize, y * tileSize, 0), Quaternion.identity) as GameObject;

				//Set drawing layer
				SpriteRenderer spriteRenderer = tile.GetComponent< SpriteRenderer > ( );
				spriteRenderer.sortingOrder = blockLayer;

				//Set the tiles parent to the GameObject this script is attached to
				tile.transform.parent = platform.transform;
				//Set the 2D map array element to the current tile that we just created.
				map[x, y] = tile;
			}
		}

		
		CreateBoxCollider( platform );


	}

	private void CreateBoxCollider( GameObject parent )
	{
		parent.AddComponent<BoxCollider2D>(  );
		BoxCollider2D boxCollider2D = parent.GetComponent< BoxCollider2D >( );

		//== Calculate sizes
		float sizeX = 1f;
		float sizeY = 1f;

		if( blockWidth > 1 )
			sizeX = (float)( blockWidth ) * 1.28f;

		if( blockHeight > 1 && !OnlyCollideTop )
			sizeY = (float)( blockHeight ) * 1.28f;

		//== Calculate offsets
		float offsetX = ( (float)( blockWidth ) - 1f ) * offset;
		float offsetY = ( (float)( blockHeight ) - 1f ) * offset;

		if( OnlyCollideTop ) offsetY *= 2f;


		//== Set size and offset
		boxCollider2D.size = new Vector2( sizeX, sizeY );
		boxCollider2D.offset = new Vector2( offsetX, offsetY );
	}

	private GameObject DecideTile(int x, int y)
	{

		//== Handle Top Row
		if( y == blockHeight - 1 )
		{
			//== Top Left
			if( x == 0 )
			{
				//Check if this block will connect to a taller one, and on which side
				if( lowerConnection == 1 ) return tilePrefabConnectLeft;
				else return tilePrefabTopLeft;
			}

			//== Top Right
			else if( x == ( blockWidth - 1  ) )
			{
				//Check if this block will connect to a taller one, and on which side
				if( lowerConnection == 2 ) return tilePrefabConnectRight;
				else return tilePrefabTopRight;
			}

			//== Mid Top
			else
			{
				return tilePrefabTopMid;
			}
		}

		//== Handle Mid tiles
		else if( y > 0 && y <  ( blockHeight - 1 ) )
		{
			//== Mid Left
			if( x == 0 )
			{
				if( higherConnection == 1 && y == connectingRow )
					return tilePrefabConnectLeftFromHigh;

				//Check if this block will connect to a smaller one and at what row
				else if( higherConnection == 1 && y <= connectingRow  || 
				    lowerConnection == 1 ) return tilePrefabMidMid;

				else return tilePrefabMidLeft;
			}

			//== Mid Right
			else if( x == ( blockWidth - 1 ) )
			{
				if( higherConnection == 2 && y == connectingRow )
					return tilePrefabConnectRightFromHigh;

				//Check if this block will connect to a smaller one and at what row
				if( higherConnection == 2 && y <= connectingRow || 
				    lowerConnection == 2 ) return tilePrefabMidMid;

				return tilePrefabMidRight;
			}

			//== Mid Mid
			else
			{
				return tilePrefabMidMid;
			}
		}

		//== Handle bottom tiles
		else
		{
			
			//== Bottom Left
			if( x == 0 )
			{
				//Check if this block will connect to a smaller one and at what row
				if( higherConnection == 1 && y <= connectingRow || 
				    lowerConnection == 1 ) return tilePrefabBottomMid;

				return tilePrefabBottomLeft;
			}

			//== Bottom Right
			else if( x == blockWidth - 1)
			{
				//Check if this block will connect to a smaller one and at what row
				if( higherConnection == 2 && y <= connectingRow || 
				    lowerConnection == 2 ) return tilePrefabBottomMid;

				return tilePrefabBottomRight;
			}

			//== Bottom Mid
			else
			{
				return tilePrefabBottomMid;
			}
		}

	}

}
