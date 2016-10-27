using UnityEngine;
using System.Collections;

public class navtest : MonoBehaviour {

	public Transform target;

	NavMeshAgent nav;
	// Use this for initialization
	void Start () {
		nav = GetComponent<NavMeshAgent>();

		nav.SetDestination(target.transform.position);

	}
}
