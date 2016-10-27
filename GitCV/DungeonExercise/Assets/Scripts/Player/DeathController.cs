using UnityEngine;
using System.Collections;

public class DeathController : MonoBehaviour {

	public bool playerDead;

	Animator anim;
	Rigidbody rb;
	PlayerSpawnController psc;


	void Awake () {
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		//psc = GameObject.Find("GameController").GetComponent<PlayerSpawnController>();
	}
	

	void Update () {
	}


	public void Die()
	{
		if(!playerDead)
		{
			playerDead = true;
			rb.velocity = Vector3.zero;
			rb.isKinematic = true;
			Animating();

			psc.respawning = true;
		}
	}

	void Animating()
	{
		anim.SetTrigger("Die");
	}
}
