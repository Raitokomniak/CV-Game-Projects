using UnityEngine;
using System.Collections;

public class WorldTargeting : MonoBehaviour {

	Camera cam;
	GameObject GC;
	UIController ui;
	NPCController NPCC;

	GameObject target;
	bool targetExists;

	LayerMask NPCmask;

	// Use this for initialization
	void Awake () {
		cam = Camera.main;
		GC = GameObject.Find("GameController");
		ui = GC.GetComponent<UIController>();

		targetExists = false;

		NPCmask = 11;
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayhit;

		if(Physics.Raycast(ray, out rayhit))
		{
			Debug.DrawLine(cam.transform.position, rayhit.point, Color.blue);
			Transform objectHit = rayhit.transform;


			//CHECK TARGET ON LEFT MOUSE CLICK
			if(Input.GetMouseButtonDown(1))
			{
				if(targetExists && rayhit.collider.tag != "Environment") //if clicked environment, target doesn't change
				{
					ReleaseTarget();
				}

				target = rayhit.collider.gameObject; 					//set hit collider as new target

				if(target.tag == "NPC")
				{
					targetExists = true;
					Debug.Log("NPC targeted");
				}
				if(target.tag == "Enemy")
				{
					targetExists = true;
					Debug.Log("Enemy targeted");
					target.GetComponent<enemyUI>().SetTarget(true);
				}

			}

			else if(Input.GetMouseButtonDown(0) && rayhit.collider.tag == "NPC")
			{
				//TOGGLE NPC INTERACTING
				NPCC = objectHit.GetComponent<NPCController>();
				NPCC.toggleInteract(true);
			}


		}

		if(Input.GetButtonDown("Escape"))
		{
			Debug.Log("hit esc");
			if(targetExists)
			{
				ReleaseTarget();
				targetExists = false;
			}
		}

	}

	void ReleaseTarget()
	{
		if (target.tag == "Enemy")
		{
			target.GetComponent<enemyUI>().SetTarget(false);
		}
	}



}
