using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveLoadController : MonoBehaviour {

	InventorySystem inventorySystem;
	PlayerExp playerExp;
	Button loadButton;
	AudioController audio;
	UIController ui;

	public static SaveLoadController saveLoadControl;
	public int health;
	GameObject Dialog;

	public int level;
	public int expCap;
	public int exp;

	public int strength;
	public int stamina;

	public GameObject[] items;
	public GameObject[] inventory;	//Inventory for gameobjects
	public int inventory0;
	public int inventory1;
	public int inventory2;

	public int equip0;
	public int equip1;

	public bool inventoryLoaded;

	void Awake () {
		audio = GameObject.Find("GameController").GetComponent<AudioController>();
		inventorySystem = GameObject.Find("GameController").GetComponent<InventorySystem>();
		playerExp = GameObject.Find("Player").GetComponent<PlayerExp>();
		ui = GameObject.Find("GameController").GetComponent<UIController>();


		LoadItemResources();
		inventory = new GameObject[3];

		level = 1;
		expCap = 100;
		exp = 0;

		if(saveLoadControl == null)
		{
			DontDestroyOnLoad(gameObject);
			saveLoadControl = this;
		}
		else if(saveLoadControl != this)
		{
			Destroy(gameObject);
		}



	}

	void OnGUI()
	{
		if(Dialog ==null)
		{
		Dialog = GameObject.Find("DialogPanel");
		//Dialog.SetActive(false);
		}

		if(GUI.Button(new Rect(400, 0, 100, 50), "Load")){
			Load();
		}
		if(GUI.Button(new Rect(500, 0, 100, 50), "Save")){
			Save();
		}
		if(GUI.Button(new Rect(600, 0, 100, 50), "Loot0")){
			inventorySystem.Loot(0);
		}
		if(GUI.Button(new Rect(700, 0, 100, 50), "Loot1")){
			inventorySystem.Loot(1);
		}
		if(GUI.Button(new Rect(800, 0, 100, 50), "Gain exp")){
			playerExp.GainExp(15);
		}
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

		PlayerData data = new PlayerData();

		data.level = level;
		data.expCap = expCap;
		data.exp = exp;

		data.health = health;
		data.strength = strength;
		data.stamina = stamina;
		data.inventory0 = inventory0;
		data.inventory1 = inventory1;
		data.inventory2 = inventory2;

		data.equip0 = equip0;
		data.equip1 = equip1;


		bf.Serialize(file, data);
		file.Close();
	}


	public void Load()
	{

		if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();

			level = data.level;
			expCap = data.expCap;
			exp = data.exp;
			health = data.health;
			strength = data.strength;
			stamina = data.stamina;

			inventory0 = data.inventory0;
			inventory1 = data.inventory1;
			inventory2 = data.inventory2;


			equip0 = data.equip0;
			equip1 = data.equip1;
		}

		SceneManager.LoadScene("Level01");

	}




public void LoadItemResources() {
		
	items = new GameObject[99];

	for(int i = 0; i < items.Length; i++) {
		items[i] = Resources.Load("Items/Item_" + i) as GameObject;
	}
}

}

[System.Serializable]
class PlayerData
{
	public int level;
	public int expCap;
	public int exp;
	public int health;
	public int strength;
	public int stamina;


	public int inventory0;
	public int inventory1;
	public int inventory2;

	public int equip0;
	public int equip1;
}