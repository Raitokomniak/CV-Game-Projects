using UnityEngine;
using System.Collections;

public class AggroDetect : MonoBehaviour {

	public GameObject player;

	EnemyMovement movement;
	DeathController deathController;
	UIController uiController;


	// Use this for initialization
	void Awake () {
		player = GameObject.Find("Player");
		movement = GetComponentInParent<EnemyMovement>();
		deathController = player.GetComponent<DeathController>();
		uiController = GameObject.Find("GameController").GetComponent<UIController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other)
	{
		LayerMask environmentMask = LayerMask.GetMask("EnvironmentObstacle");
		Vector3 offset = transform.position - player.transform.position;
		Ray ray = new Ray(transform.position, offset);
		RaycastHit hit;
		SphereCollider aggroRange = GetComponent<SphereCollider>();
	
		Debug.DrawLine(transform.position, other.gameObject.transform.position);
		Debug.DrawRay(transform.position, offset);

		if (other.gameObject == player && !deathController.playerDead && !movement.Return)
		{
			if (Physics.Raycast(ray, out hit, offset.magnitude, environmentMask))
				{
					return;
				}
				
			else{
				uiController.FlashAggroText();
				movement.Aggro = true;
			}
		
		}
	
	}


	
		
}
