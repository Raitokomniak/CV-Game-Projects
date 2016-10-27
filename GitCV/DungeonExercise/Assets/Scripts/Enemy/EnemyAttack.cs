using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

	public int attackDamage;
	public float attackCoolDown;
	public GameObject player;

	bool playerInRange;
	float timer;
	bool isAttacking = true;

	Animator anim;
	EnemyMovement move;
	NavMeshAgent nav;
	PlayerHealth playerhealth;
	DeathController deathController;

	// Use this for initialization
	void Awake () {
		player = GameObject.Find("Player");
		anim = GetComponent<Animator>();
		nav = GetComponent<NavMeshAgent>();
		playerhealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
		deathController = GameObject.Find("Player").GetComponent<DeathController>();
	}




	void Update () {
		timer += Time.deltaTime;

		if(timer >= attackCoolDown && playerInRange && !deathController.playerDead)
		{
			Attack();
		}
	}

	void Attack(){
		
		//timer = 0f;
		anim.SetTrigger("Attack");
		isAttacking = true;
		Animating();
	}

	void Animating()
	{
		anim.SetBool("IsAttacking", true);
	}

	void OnTriggerStay(Collider other)
	{
		Debug.DrawRay(transform.position, transform.position + player.transform.position);


		if(other.gameObject == player)
			{
				playerInRange = true;
	
			}
		anim.SetTrigger("Collided");
	}


	void OnTriggerExit(Collider other)
	{
		
		if(other.gameObject == player)
		{
			playerInRange = false;
			//anim.SetBool("IsAttacking", false);
		}

	}



	void DealDamage()
	{
		Debug.Log("deals damage");
		nav.Stop();
		isAttacking = false;
		timer = 0f;

		if(playerInRange)
		{
			playerhealth.takeDamage(attackDamage);
		}

	}

	void ReEnableMovement()
	{
		isAttacking = false;
		anim.SetBool("IsAttacking", false);
		nav.Resume();
	}


	void EndAttack()
	{
		anim.SetBool("IsAttacking", false);
	}
}
