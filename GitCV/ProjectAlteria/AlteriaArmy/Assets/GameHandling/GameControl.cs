using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class CharacterData
{
	public int characterIndex;

	public string characterName;
	public string combatClass;

	public int level;
	public int exp;
	public int expCap;

	public int maxHP;
	public int HP;

	public int maxMP;
	public int MP;

	public int ATK;
	public int INT;
	public int STA;

	public Item[] equipment;
	public Weapon weapon;
	public Ring ring;

	public Skill[] skillSet;

}


public class GameControl : MonoBehaviour {

	public static GameControl gameControl;

	[SerializeField] public EnemySpawner enemySpawner;
	[SerializeField] public PlayerSpawner playerSpawner;
	[SerializeField] public EnvironmentSpawner environmentSpawner;
	[SerializeField] public PhaseController phase;
	[SerializeField] public TurnController turn;
	[SerializeField] public SceneHandler scene;
	[SerializeField] public UIController ui;
	[SerializeField] public DialogueHandler dialogue;
	[SerializeField] public ItemController items;
	[SerializeField] public SkillLibrary skillLib;
	[SerializeField] public ProgressionHandler progress;

	public ArrayList playerList;
	public ArrayList partyList;
	public int partySize;

	public int storyProgression;

	public Item[] inventory;

	public bool loaded;


	void Awake () {
		if (gameControl == null) {
			DontDestroyOnLoad (gameObject);
			gameControl = this;
		} else if (gameControl != this) {
			Destroy (gameObject);
		}


	}


	void Start(){
		enemySpawner = GetComponent<EnemySpawner> ();
		playerSpawner = GetComponent<PlayerSpawner> ();
		environmentSpawner = GetComponent<EnvironmentSpawner> ();
		phase = GetComponent<PhaseController> ();
		turn = GetComponent<TurnController> ();
		ui = GetComponent<UIController> ();
		items = GetComponent<ItemController> ();

		scene = GetComponent<SceneHandler> ();

		dialogue = GetComponent<DialogueHandler> ();

		skillLib = GetComponent<SkillLibrary> ();
		progress = GetComponent<ProgressionHandler> ();

		playerList = new ArrayList ();
		partyList = new ArrayList ();
		ui.SetupMenuCanvas ();

		skillLib.CreateSkills ();
		SetupCharacters ();

		partySize = 2;
		//partyList.Add (playerList [0]);

		storyProgression = 0;
	}


	void Update () {
		//inventory = items.inventory;
	}


	public void SetupCharacters(){
		CharacterData mc = new CharacterData ();

		mc.characterIndex = 0;

		mc.characterName = "Iris";
		mc.combatClass 	= "Sworddancer";

		mc.level 	= 1;
		mc.exp 		= 0;
		mc.expCap 	= 50;
		mc.maxHP 	= 100;
		mc.HP 		= 100;
		mc.maxMP 	= 50;
		mc.MP 		= 50;
		mc.ATK 		= 2;
		mc.INT 		= 1;
		mc.STA 		= 1;
		mc.weapon 	= null;
		mc.ring 		= null;

		mc.equipment = new Item[2];
		mc.skillSet = new Skill[5];

		mc.skillSet [0] = null;
		mc.skillSet [1] = null;
		mc.skillSet [2] = null;
		mc.skillSet [3] = null;
		mc.skillSet[4] = GameControl.gameControl.skillLib.phoenixFire;

		playerList.Add (mc);

		CharacterData sub = new CharacterData ();

		sub.characterIndex = 1;

		sub.characterName = "Sub";
		sub.combatClass 	= "Advisor";

		sub.level 		= 1;
		sub.exp 		= 0;
		sub.expCap 	= 50;
		sub.maxHP 		= 100;
		sub.HP 		= 100;
		sub.maxMP 		= 50;
		sub.MP 		= 50;
		sub.ATK 		= 2;
		sub.INT 		= 1;
		sub.STA 		= 1;
		sub.weapon 	= null;
		sub.ring 		= null;

		sub.equipment = new Item[2];
		sub.skillSet = new Skill[5];

		playerList.Add (sub);

		CharacterData third = new CharacterData ();

		third.characterIndex = 2;

		third.characterName = "Third guy";
		third.combatClass 	= "Advisor";

		third.level 		= 1;
		third.exp 		= 0;
		third.expCap 	= 50;
		third.maxHP 		= 100;
		third.HP 		= 100;
		third.maxMP 		= 50;
		third.MP 		= 50;
		third.ATK 		= 2;
		third.INT 		= 1;
		third.STA 		= 1;
		third.weapon 	= null;
		third.ring 		= null;

		third.equipment = new Item[2];
		third.skillSet = new Skill[5];

		playerList.Add (third);
	}

	public void TempUpdateSingle(int characterIndex, string stat, int value)
	{
		CharacterData character = playerList [characterIndex] as CharacterData;

		switch(stat) {
		case "Level":
			character.level = value;
			break;
		}

	}

	public void TempUpdateAll(int characterIndex, int level, int exp, int expCap, int maxHP, int HP, int maxMP, int MP, int ATK, int INT, int STA, 
		Weapon weapon, Ring ring, Item[] equipment, Skill[] skillSet) 
	{
		CharacterData character = new CharacterData ();

		character.level = level;
		character.exp = exp;
		character.expCap = expCap;
		character.maxHP = maxHP;
		character.HP = HP;
		character.maxMP = maxMP;
		character.MP = MP;
		character.ATK = ATK;
		character.INT = INT;
		character.STA = STA;
		character.weapon = weapon;
		character.ring = ring;
		character.equipment = equipment;
		character.skillSet = skillSet;

		playerList.Insert (characterIndex, character);

		//Save ();
	}

	public void CalculateStats(CharacterData character, Item item)
	{
		if (character.weapon != null && item.type == "Weapon") {
			character.ATK += character.weapon.GetDamage ();
		} else if (character.weapon == null){
			character.ATK = 2;
		}
	}

	public CharacterData GetStats(int characterIndex)
	{
		return (CharacterData)playerList [characterIndex];
	}



	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");

		PlayerData data = new PlayerData ();
		CharacterData characterData = new CharacterData ();

		data.playerList = playerList;
		data.storyProgression = storyProgression;
		data.inventory = inventory;

		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load()
	{
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize (file);

			playerList = data.playerList;
			storyProgression = data.storyProgression;
			inventory = data.inventory;

			file.Close ();
		}

		loaded = true;
	}
}


[Serializable]
class PlayerData
{
	public ArrayList playerList;
	public int storyProgression;

	public Item[] inventory;
}