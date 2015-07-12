using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {
	public bool canGrapple;
	public string targetName;
	private float grappleDistance;

	private GameObject player;
	private Player playerScript;
	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find("Player");
		playerScript = player.GetComponent<Player>();

		grappleDistance = 5f; 
		canGrapple = false;
	}

	void grappleCheck()
	{
		if (Vector3.Distance(player.transform.position, this.transform.position) <= grappleDistance)
		{
			canGrapple = true;
			targetName = this.name;
		}
		else
		{
			canGrapple = false;
		}
	}


	
	// Update is called once per frame
	void Update () 
	{
		grappleCheck();
	}
}
