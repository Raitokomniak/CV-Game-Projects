using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	//This is the base class for all game items
	//Gameobjects created in resources have this component


	public int id;
	public string itemName;
	public Sprite image;
	public string type;
	public int strength;
	public int stamina;
	public bool equippable;

	int arrayIndex;

	void Start () {
		id = 0;
		itemName = "0";
		image = null;
		type = "0";
		strength = 0;
		stamina = 0;
		equippable = false;

		arrayIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
