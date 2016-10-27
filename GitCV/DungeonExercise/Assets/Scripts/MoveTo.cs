using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {

	public Transform goal;
	NavMeshAgent nav;
	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent>();
		nav.destination = goal.position;
	}
	
	// Update is called once per frame
	void Update () {
		nav.destination = goal.position;
	}
}
