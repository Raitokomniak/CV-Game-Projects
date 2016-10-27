using UnityEngine;
using System.Collections;

public class AggroRelease : MonoBehaviour {

	public GameObject player;
	EnemyMovement movement;


	void Awake () {
		player = GameObject.Find("Player");
		movement = GetComponentInParent<EnemyMovement>();
	}
		
	void Update () {

	}

	void OnTriggerExit(Collider other)
	{

		if (other.gameObject == player)
		{
			if (movement.Aggro)
			{
				movement.Aggro = false;
				movement.Return = true;
			}
		}
	}

}
