using UnityEngine;
using System.Collections;

public class EnvironmentSpawner : MonoBehaviour {
	GameObject rock;

	public bool setupReady;

	public void Setup(){
		rock = Resources.Load ("Rock") as GameObject;
		Spawn (4, 4);
		setupReady = true;
	}

	public void Spawn(int givenX, int givenZ){
		rock = Instantiate (rock, new Vector3 (givenX * 2, 0.7f, givenZ * 2), Quaternion.Euler(0,0,0)) as GameObject;
	}

}
