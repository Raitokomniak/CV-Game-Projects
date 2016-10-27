using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float iniSpeed = 2;
	public float iniJumpHeight;


	Animator anim;
	Rigidbody rb;
	DeathController deathController;
	CharacterController cc;

	float moveSpeed;
	bool collided;
	bool jumping;
	bool enableJump;

	Transform playerTransform;
	Vector3 destinationPos;

	float destinationDist;

	float timer;
	float jumpHeight;

	bool isMovingAudio;

	void Start () {
		playerTransform = transform;

		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		deathController = GetComponent<DeathController>();
		cc = GetComponent<CharacterController>();

		enableJump = true;
		isMovingAudio = false;
	}
	

	void Update () {
		
		destinationDist = Vector3.Distance(destinationPos, playerTransform.position);

		if(destinationDist < .5f){
			moveSpeed = moveSpeed - moveSpeed;}
		else if(destinationDist > .5f){
			moveSpeed = iniSpeed;}


		//FREEZE ROTATING IF MOVESPEED IS 0
		if (moveSpeed <= 0)
		{
			rb.constraints = RigidbodyConstraints.FreezeRotation;
		}
		else if (!deathController.playerDead)
		{
			rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		}
			
	


	//MOVE PLAYER WITH MOUSE
		if (Input.GetMouseButton(1) && !deathController.playerDead)
		{
			cc.enabled = false;
			anim.SetBool("IsMoving", true);
			Plane playerPlane = new Plane(Vector3.up, playerTransform.position);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float hitdist = 0f;
			ToggleMovementAudio(true);

			if (playerPlane.Raycast(ray,out hitdist)) 
			{
				Vector3 targetPoint = ray.GetPoint(hitdist);
				destinationPos = ray.GetPoint(hitdist);
				Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
				playerTransform.rotation = targetRotation;
			}
		}


		//ENABLE MOVEMENT IF PLAYER IS ALIVE
		if (destinationDist > .5f && !deathController.playerDead)
		{
			playerTransform.position = Vector3.MoveTowards(playerTransform.position, destinationPos, moveSpeed * Time.deltaTime);
		}

		//STOP ANIMATION MOVEMENT IF MOVESPEED 0
		if(moveSpeed <= 0 && collided == false)
		{
			anim.SetBool("IsMoving", false);
			GetComponent<AudioSource>().Stop();
			isMovingAudio = false;
		}

		else if (collided == true)
		{
			collided = false;
		}
	

		//JUMPING

		if (Input.GetButtonDown("Jump"))// && !jumping && jumpCoolDownTimer > .5f)
		{
			if(enableJump)
			{
				jumpHeight = transform.position.y + iniJumpHeight;
				jumping = true;
				enableJump = false;
				rb.useGravity = false;
			}
		}


		if (rb.useGravity == false)
		{
			timer += Time.deltaTime;
			rb.useGravity = false;
			anim.SetBool("IsMoving", false);
			Vector3 jump = new Vector3(transform.position.x, jumpHeight, transform.position.z);
			transform.position = Vector3.Lerp (transform.position, jump, timer);

			Vector3 offset = jump - transform.position;

			//CHECKS IF MID-AIR
			if (offset.y <= .2)
			{
				rb.useGravity = true;
				timer = 0f;
				jumpHeight = iniJumpHeight;
			}
		}
	}




	//STOP MOVEMENT TEMPORARILY ON COLLISION
	void OnCollisionEnter(Collision c)
	{
			collided = true;
			destinationPos = playerTransform.position;
			anim.SetBool("IsMoving", false);
			rb.velocity = Vector3.zero;

		int FloorLayer = 10;

		if (c.gameObject.layer == FloorLayer)
		{
			timer = 0f;
			jumping = false;
			enableJump = true;
		}

	}

	void ToggleMovementAudio(bool mode)
	{

		if(isMovingAudio == false)
		{
			isMovingAudio = true;
			GetComponent<AudioSource>().Play();
		}

	}
}
