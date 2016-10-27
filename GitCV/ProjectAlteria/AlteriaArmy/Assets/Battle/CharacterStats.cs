using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CharacterStats : MonoBehaviour {

	CharacterData character;

	public int playerCharacterIndex;
	public bool mainCharacter;

	//Non-mechanical statistics
	public string characterName;
	public string combatClass;

	//Base statistics
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

	public int moveRange;

	//Equipment
	public Weapon weapon;
	public Ring ring;
	public Item[] equipment;

	public Skill[] skillSet;

	//Turn values
	public bool canStillMove;
	public bool canStillAttack;
	public bool dead;

	// Use this for initialization
	void Awake () {
		if (GameControl.gameControl.scene.scene == 1) {
			canStillMove = true;
			canStillAttack = true;
		}
		moveRange = 1;
	}


	// Update is called once per frame
	void Update () {
		if (this.gameObject.tag == "Player") {
			//UpdateCharacter ();
		}
	}

	public void UpdateCharacter(){
		character = GameControl.gameControl.playerList [playerCharacterIndex] as CharacterData;
		characterName = character.characterName;
		level = character.level;
		exp = character.exp;
		expCap = character.expCap;
		maxHP = character.maxHP;
		HP = character.HP;
		maxMP = character.maxMP;
		MP = character.MP;
		ATK = character.ATK;
		INT = character.INT;
		STA = character.STA;
		weapon = character.weapon;
		ring = character.ring;
		equipment = character.equipment;
		skillSet = character.skillSet;

	}

	public void UpdateStats()	{
		character = GameControl.gameControl.playerList [playerCharacterIndex] as CharacterData;
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
	}

	public void SetUpCharacter(int characterIndex){

		if (this.gameObject.tag == "Player") {
			character = GameControl.gameControl.playerList [characterIndex] as CharacterData;
			characterName = character.characterName;
			level = character.level;
			exp = character.exp;
			expCap = character.expCap;
			maxHP = character.maxHP;
			HP = character.HP;
			maxMP = character.maxMP;
			MP = character.MP;
			ATK = character.ATK;
			INT = character.INT;
			STA = character.STA;
			weapon = character.weapon;
			ring = character.ring;
			equipment = character.equipment;

			moveRange = 1;
			mainCharacter = true;

			playerCharacterIndex = characterIndex;

			GetComponent<CharacterSkill> ().SetSkills (characterIndex);

			skillSet = GetComponent<CharacterSkill> ().skillSet;
		}
	}



	//////////
	//BATTLE
	//////////
	public void Buff(string stat, int amount)
	{
		switch(stat)
		{
		case "ATK":
			ATK += amount;
			break;
		case "HP":
			Heal (amount);
			break;
		}
		if (stat != "HP") {
			Debug.Log (stat + " increased by " + amount + " for 3 seconds");
			StartCoroutine (BuffDuration (stat, amount));
		}
	}

	public void Heal(int amount)
	{
		HP += amount;
		if (HP > maxHP) {
			HP = maxHP;
		}
	}
	IEnumerator BuffDuration(string stat, int amount){
		yield return new WaitForSeconds (3f);
		switch (stat) {
		case "ATK":
			ATK -= amount;
			break;
		}
		Debug.Log ("Buff expired");
	}

	public void GainExp(int amount){
		Debug.Log (amount + " exp gained");
		exp += amount;
		if (exp >= expCap) {
			LevelUp ();
		}
		//UpdateStats ();
	}

	public void UseMana(int amount)
	{
		MP -= amount;
		if (MP < 0) {
			MP = 0;
		}
		//UpdateStats ();
	}

	void LevelUp(){
		level += 1;
		Debug.Log(this.gameObject.name + " leveled up to " + level);

		GameControl.gameControl.progress.CheckLevelUpRewards (level, this);
	}

	public void TakeDamage(int amount, GameObject inflictor){
		HP -= amount;
		Debug.Log (this.gameObject.name + " takes " + amount + " damage");

		if (HP <= 0) {
			HP = 0;
			if (inflictor.tag == "Player") {
				inflictor.GetComponent<CharacterStats> ().GainExp (50);
			}
			Die ();
		}
	}

	public void Die(){
		Destroy (this.gameObject);
		if (this.gameObject.tag == "Enemy") {
			GameControl.gameControl.enemySpawner.enemyList.Remove (this.gameObject);
		} else if(this.gameObject.tag == "Player") {
			GameControl.gameControl.playerSpawner.playerList.Remove (this.gameObject);
			if(GameControl.gameControl.playerSpawner.playerList.Count == 0)
			{
				StartCoroutine (ReloadScene ());
			}
		}
	}

	IEnumerator ReloadScene(){
		yield return new WaitForSeconds (1.5f);
		SceneManager.LoadScene ("Grid");
	}


}
