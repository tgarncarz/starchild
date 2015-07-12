using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {
	private float speed, topSpeed, decSpeed, jumpHeight, grappleSpeed, health, epsilon;
	private int jumpFlag;
	private Vector2 newV;
	private bool isFacingRight, canJump, isGrounded;
	private Rigidbody rgb;
	
	private GameObject bestTarget;
	private GameObject[] targets;
	private Slider healthBar;
	// Use this for initialization
	void Start () {
		Physics.gravity = new Vector3(0, -12.5f, 0);
		jumpFlag = 0;
		speed =  1.2f;
		decSpeed = 0.4f;
		topSpeed = 4f;
		isFacingRight = true;
		canJump = true;
		isGrounded = false;
		grappleSpeed = 5f;
		health = 100f;
		epsilon = 0.1f;

		rgb = this.GetComponent<Rigidbody>();
    	healthBar = GameObject.Find("UI").GetComponent<Transform>().Find("HealthBar").gameObject.GetComponent<Slider>();

	}

	private void checkMovement(){
		if (Input.GetKey("a")){
			isFacingRight = false;
			if (GetComponent<Rigidbody>().velocity.x > -topSpeed){
				newV = GetComponent<Rigidbody>().velocity;
				newV.x -= speed;
				GetComponent<Rigidbody>().velocity = newV;
			}
			else{
				newV = GetComponent<Rigidbody>().velocity;
				newV.x = -topSpeed;
				GetComponent<Rigidbody>().velocity = newV;
			}
		}

		else if (Input.GetKey("d")){
			isFacingRight = true;
			if (GetComponent<Rigidbody>().velocity.x < topSpeed){
				newV = GetComponent<Rigidbody>().velocity;
				newV.x += speed;
				GetComponent<Rigidbody>().velocity = newV;
			}
			else{
				newV = GetComponent<Rigidbody>().velocity;
				newV.x = topSpeed;
				GetComponent<Rigidbody>().velocity = newV;
			}
			
		}

		else if (!Input.GetKey("d") && !(Input.GetKey("a"))){
			newV = GetComponent<Rigidbody>().velocity;
			if (newV.x != 0f){
				
				if (newV.x < 0f){
					newV.x += decSpeed;
				}
				else{
					newV.x -= decSpeed;
				}
				GetComponent<Rigidbody>().velocity = newV;
			}
		}

		if (Input.GetKeyDown("space")){
			if (canJump == true){
				newV = GetComponent<Rigidbody>().velocity;
				newV.y += jumpHeight;
				GetComponent<Rigidbody>().velocity = newV;
			jumpFlag += 1;	
			}
		}

		if (isGrounded == true){
			canJump = true;
			jumpFlag = 0;
			jumpHeight = 5f;
		}
		else if (isGrounded == false && jumpFlag < 1){
			canJump = true;
			jumpHeight = 5f;
		}
		else{
			canJump = false;
		}

	}

	private void checkGrapple ()
	{
		targets = GameObject.FindGameObjectsWithTag("Target");
		bestTarget = GetClosestTargets(targets);
		if (bestTarget.GetComponent<Target>().canGrapple){
			if (Input.GetKey("e")){
				MoveTowardGrappleTarget(bestTarget);
			}
			else
			{
				rgb.useGravity = true;
			}
		}

	}

	//http://forum.unity3d.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
	private GameObject GetClosestTargets (GameObject[] targets)
	{
		GameObject bestTarget = null;
		float closestDistanceSqr = Mathf.Infinity;
		Vector3 currentPosition = transform.position;
		foreach(GameObject potentialTarget in targets)
		{

			Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = potentialTarget;
			}
		}
	 
		return bestTarget;
	}

	private void MoveTowardGrappleTarget(GameObject bestTarget)
	{
		Vector3 direction = bestTarget.transform.position - this.transform.position;
		Vector3 moveVector = direction.normalized * grappleSpeed * Time.deltaTime * 2;
		if (Vector3.Distance(bestTarget.transform.position, this.transform.position) > epsilon){
			this.transform.position += moveVector;
		}
	}

	void OnCollisionEnter (Collision coll){
		if (coll.gameObject.tag == "Ground") isGrounded = true;
	}

	void OnCollisionStay (Collision coll){
		if (coll.gameObject.tag == "Enemy"){
			health -= 5f;
			if (isFacingRight)
				rgb.AddForce(new Vector3(-2, 0, 0), ForceMode.Impulse);
			else
				rgb.AddForce(new Vector3(2, 0, 0), ForceMode.Impulse);
		}
	}

	void OnCollisionExit (Collision coll){
		if (coll.gameObject.tag == "Ground") isGrounded = false;
	}
	
	// Update is called once per frame
	void Update () {
		checkMovement();
		checkGrapple();
		Debug.Log(health);
		healthBar.value = health;
		if (health <= 0f){
			Application.LoadLevel("test");
		}

	}
}
