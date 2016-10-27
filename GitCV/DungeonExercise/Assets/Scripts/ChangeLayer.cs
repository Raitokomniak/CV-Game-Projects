using UnityEngine;
using System.Collections;

public class ChangeLayer : MonoBehaviour {

	GameObject player;
	LayerMask floorMask = 10;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerStay(Collider c)
	{
		if (c = player.GetComponent<Collider>())
			{
			gameObject.layer = floorMask;
			}
	}

	void OnTriggerExit(Collider c)
	{
		if (c = player.GetComponent<Collider>())
		{
			gameObject.layer = 0;
		}
	}
}
