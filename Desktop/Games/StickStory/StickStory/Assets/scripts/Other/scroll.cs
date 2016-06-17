using UnityEngine;
using System.Collections;

public class scroll : MonoBehaviour 
{
	public float speed = 2.5f;
	private MeshRenderer meshRenderer;


	// Use this for initialization
	void Start () 
	{
		meshRenderer = GetComponent< MeshRenderer >( );
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector2 offset = new Vector2( Time.time * speed, 0f );
		meshRenderer.material.SetTextureOffset( "_MainTex", offset );
	}
}
