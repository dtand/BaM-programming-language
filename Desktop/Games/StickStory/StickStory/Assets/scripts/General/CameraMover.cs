using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
	public float minX, maxX;
	public float minY, maxY;
}

public class CameraMover : MonoBehaviour {
	
	public float dampTime = 0.15f;
	
	private Vector3 velocity = Vector3.zero;
	private Camera camera;
	private Transform defaultTarget;
	public Transform target;
	
	public Boundary bounds;
	public bool freeze;
	
	void Awake()
	{
		freeze = false;
		camera = GetComponent<Camera>();
		defaultTarget = target;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( freeze ) return;
		FollowTarget();
	}
	
	//Follows current target [player or telescope]
	void FollowTarget()
	{
		if (target)
		{
			Vector3 point =  camera.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
			transform.position = new Vector3(Mathf.Clamp(transform.position.x, bounds.minX, bounds.maxX), 
			                                 Mathf.Clamp(transform.position.y, bounds.minY, bounds.maxY), -10f);
		}
	}

	public void ResetTarget()
	{
		target = defaultTarget;
	}
}
