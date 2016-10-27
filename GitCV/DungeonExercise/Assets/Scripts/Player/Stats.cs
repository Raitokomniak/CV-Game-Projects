using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {

	public int strength;
	public int stamina;

	public int initialStr;
	public int initialSta;

	// Use this for initialization
	void Start () {
		initialStr = 1;
		initialSta = 2;
	}
	
	// Update is called once per frame
	void Update () {
		//strength  = SaveLoadController.saveLoadControl.strength;
		//stamina  = SaveLoadController.saveLoadControl.stamina;
	}
}
