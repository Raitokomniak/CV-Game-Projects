using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

	public int X_pos;
	public int Z_pos;

	// Use this for initialization
	void Awake () {
		X_pos = Mathf.RoundToInt(transform.position.x / 2);
		Z_pos = Mathf.RoundToInt(transform.position.z / 2);
	}
}
