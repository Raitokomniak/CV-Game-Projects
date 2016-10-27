using UnityEngine;
using System.Collections;

public class PlayerAttackMelee : MonoBehaviour {

	public float meleeAttackCoolDown = 1.0f;
	public int attackDamage = 10;

	Animator anim;
	DeathController deathController;
	PlayerSpell spellController;
	GameObject player;

	GameObject fireBolt;

	int enemyLayer = 9;
	bool isAttacking;
	float timer;

	float projectileTimer = 2f;
	Vector3 targetPosition;

	float px;
	float py;
	float pz;

	LayerMask floorMask;

	// Use this for initialization
	void Awake () 
	{
		floorMask = LayerMask.GetMask("Floors");

		player = GameObject.Find("Player");
		px = player.transform.position.x;
		py = player.transform.position.y;
		pz = player.transform.position.z;

		timer += Time.deltaTime;
		anim = GetComponent<Animator>();
		deathController = GetComponent<DeathController>();
		spellController = GetComponent<PlayerSpell>();
	}
	
	// Update is called once per frame

	void FixedUpdate () {
	
		/*if (Input.GetMouseButtonDown(0) && !deathController.playerDead && !spellController.spellTargetingActive)// && timer > meleeAttackCoolDown)
		{
			Attack();
		}

		if(projectileTimer > 0f)
		{

			Debug.DrawRay(new Vector3(px, py + 0.5f, pz), targetPosition, Color.red);
			projectileTimer -= Time.deltaTime;
		}
		else if (projectileTimer <= 0f)
		{
			Destroy(fireBolt);
		}*/


	}

	void Attack()
	{
		Destroy(fireBolt);
		projectileTimer = 2f;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayhit;

		if (Physics.Raycast(ray, out rayhit, Mathf.Infinity, floorMask))
		{
			targetPosition = new Vector3(rayhit.point.x, rayhit.point.y + 0.5f, rayhit.point.z);
			fireBolt = GameObject.Instantiate(Resources.Load("Firebolt")) as GameObject;

			Animate();
		}
	}

	void Animate()
	{
		anim.SetTrigger("Attack");
		anim.SetBool("IsAttacking", true);


		//fireBolt.GetComponent<ParticleSystem>().Emit(10);

	}

	void SetAttackFlag()
	{
		isAttacking = true;

	}

	void EndAttack()
	{
		isAttacking = false;
		anim.SetBool("IsAttacking", false);
	}

	void OnCollisionStay(Collision other)
	{
		
		if (isAttacking == true && other.gameObject.layer == enemyLayer)
		{
			timer = 0f;
			other.gameObject.GetComponent<EnemyHealth>().takeDamage(attackDamage);
			isAttacking = false;
		}

		anim.SetBool("IsAttacking", false);
	}



}
