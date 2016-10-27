using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

	public Transform player;
	public float enemyIdleSpeed;
	public float enemyRunSpeed;

	public bool isNeutral;

	public bool IsWalkingIdle;
	public bool Aggro;
	public bool Return;

	public float walkingTimer;

	bool isWalking;
	bool isKnockedBack;

	Animator anim;
	NavMeshAgent nav;
	Vector3 originalPos;
	DeathController deathController;

	float x;
	float z;


	Vector3[] idlePaths;
	int pathIndex;



	void Awake () {
		anim = GetComponent<Animator>();
		nav = GetComponent<NavMeshAgent>();
		deathController = GameObject.Find("Player").GetComponent<DeathController>();

		nav.enabled = true;

		player = GameObject.Find("Player").transform;
		originalPos = transform.position;

		//IDLE MOVING PATHS
		/*x = gameObject.transform.position.x;
		z = gameObject.transform.position.z;

		idlePaths = new [] {new Vector3(x + enemyIdleSpeed * 0.5f, 0, z + enemyIdleSpeed * 0.5f),
							new Vector3(x - enemyIdleSpeed * 0.5f, 0, z + enemyIdleSpeed * 0.5f)};
		pathIndex = 0;
		*/

		//INITIAL BOOLS
		Aggro = false;
		IsWalkingIdle = true;
		Return = false;
	}
	

	void Update () {

		if(deathController.playerDead && !anim.GetBool("PlayerDead"))
		{
			Aggro = false;
			Return = true;
			anim.SetBool("PlayerDead", true);
			anim.ResetTrigger("Attack");
		}
			

		Moving();

		if (isKnockedBack)
		{
			transform.position = Vector3.Lerp(transform.position, anim.deltaPosition, .1f);
		}


	}

	void Moving()
	{
		if(Return)
		{
			anim.SetTrigger("Return");
			nav.destination = originalPos;
			nav.speed = 3;

			if(nav.remainingDistance == 0)
			{
				anim.ResetTrigger("Return");
				nav.Stop();
				IsWalkingIdle = true;
				anim.SetBool("IsWalkingIdle", true);
				Return = false;
			}

		}

		else if(Aggro) 
		{
			nav.destination = player.position;
			IsWalkingIdle = false;

			transform.position = Vector3.RotateTowards(transform.position, player.transform.position, 0, 0);
		}


		else if (IsWalkingIdle)
		{
			nav.Resume();

			if (walkingTimer >= 2.0f)
			{
				/*
				nav.destination = idlePaths[pathIndex];
				isWalking = true;
				anim.SetBool("IsWalkingIdle", true);
				*/
			}

			else if (walkingTimer <= 0f){

				/*
				isWalking = false;
				anim.SetBool("IsWalkingIdle", false);
				pathIndex = 1 - pathIndex;
				*/
			}

		}


		Animating();
	}



	void Animating()
	{
		if (Aggro)
		{
			anim.SetTrigger("Aggro");
			anim.SetBool("IsWalkingIdle", false);
			nav.speed = enemyRunSpeed;
		}
		else if (Return)
		{
			anim.ResetTrigger("Aggro");

		}
	}


	void ReEnableNav()
	{
		nav.Resume();
		isKnockedBack = false;
	}


	void KnockedBack()
	{
		isKnockedBack = true;
		nav.Stop();
		anim.SetBool("IsAttacking", false);
	}
		
}
