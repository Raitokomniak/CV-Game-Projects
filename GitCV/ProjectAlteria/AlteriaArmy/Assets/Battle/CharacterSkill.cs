using UnityEngine;
using System.Collections;

public class CharacterSkill : MonoBehaviour {

	public GameObject grid;
	AccessibleArea AAD;
	ArrayList listToCheck;

	public Skill[] skillSet;

	AoEDamage aoeDamage;
	SingleDamage singleDamage;
	AoEPositive aoePositive;
	SinglePositive singlePositive;
	CharacterSpecial characterSpecial;

	// Use this for initialization
	void Awake () {
		if (GameControl.gameControl.scene.scene == 1) {
			grid = GameObject.Find ("Grid");
			AAD = grid.GetComponent<AccessibleArea>();
		}
		skillSet = new Skill[5];
	}


	public void SetSkills(int characterIndex){
		CharacterData character = (CharacterData)GameControl.gameControl.playerList [characterIndex];
		skillSet = character.skillSet;
	}

	public void SpecialAttack(int skillSetIndex){
		string effect = skillSet [skillSetIndex].effect;
		bool isAoE;

		if (skillSetIndex <= 1) isAoE = true;
		else isAoE = false;

		//Check if enough mana
		if (GetComponent<CharacterStats> ().MP >= skillSet [skillSetIndex].manaCost) {
			AAD.DetermineArea (this.gameObject);

			//If is AoE, deals damage immediately when skill icon is clicked
			if (isAoE || skillSet[skillSetIndex].isSelf) {
				foreach (GameObject cell in AAD.accessibleCells)
					DealAoEEffect (cell);

				GetComponent<CharacterStats> ().UseMana (skillSet [skillSetIndex].manaCost);
				GameControl.gameControl.turn.UpdateTurn ("Attack Phase", this.gameObject);
			} 
			//If not AoE, sets special phase to wait for cell selection
			else {
				GameControl.gameControl.phase.SetSpecialPhase (skillSetIndex);
			}
				
		} else {
			Debug.Log ("not enough mana");
		}
	}


	////////////////////////////////
	//Find affected characters depending on effect type
	////////////////////////////////
	void FindList(string effect){
		if (effect == "Damage") {
			if (this.gameObject.tag == "Player")		listToCheck = GameControl.gameControl.enemySpawner.enemyList;
			else if (this.gameObject.tag == "Enemy")	listToCheck = GameControl.gameControl.playerSpawner.playerList;
		} 

		else if (effect == "Buff") {
			if (this.gameObject.tag == "Player")		listToCheck = GameControl.gameControl.playerSpawner.playerList;
			else if (this.gameObject.tag == "Enemy") 	listToCheck = GameControl.gameControl.enemySpawner.enemyList;
		} 
	}

	public void DealSingleCellEffect(GameObject cell){
		int skillSetIndex = GameControl.gameControl.phase.specialPhase;
		string effect = skillSet [skillSetIndex].effect;

		FindList (effect);

		foreach (GameObject character in listToCheck) {
			if (character != null) {
				if (character.GetComponent<Movement> ().X_pos == cell.GetComponent<CellSelection> ().X_index
					&& character.GetComponent<Movement> ().Z_pos == cell.GetComponent<CellSelection> ().Z_index) {

					if (effect == "Damage") {
						SingleDamage singled = (SingleDamage)skillSet [skillSetIndex];
						character.GetComponent<CharacterStats> ().TakeDamage (singled.GetDamage (), this.gameObject);
						break;
					} else if (effect == "Buff") {
						SinglePositive singlep = (SinglePositive)skillSet [skillSetIndex];
						character.GetComponent<CharacterStats> ().Buff (singlep.affectedStat, singlep.amount);
						break;
					}
				}
			}
		}


		//USE MANA ACCORDINGLY
		GetComponent<CharacterStats> ().UseMana (skillSet [skillSetIndex].manaCost);
	}


	public void DealAoEEffect(GameObject cell){
		int skillSetIndex = GameControl.gameControl.phase.specialPhase;
		string effect = skillSet [skillSetIndex].effect;
		FindList (effect);

		////////////////////////////////
		//Add found characters to a list
		////////////////////////////////
		ArrayList charactersToAffect = new ArrayList ();

		foreach (GameObject character in listToCheck) {
			if (character != null) {
				if (character.GetComponent<Movement> ().X_pos == cell.GetComponent<CellSelection> ().X_index
				    && character.GetComponent<Movement> ().Z_pos == cell.GetComponent<CellSelection> ().Z_index) {
					charactersToAffect.Add (character);
				}
			}
		}

		////////////////////////////////
		//Execute effect on all found characters
		////////////////////////////////
		foreach (GameObject character in charactersToAffect) {
			if (effect == "Damage") {
				AoEDamage aoed = (AoEDamage)skillSet [0];
				character.GetComponent<CharacterStats> ().TakeDamage (aoed.GetDamage (), this.gameObject);
			} else if (effect == "Buff") {
				if (skillSet [skillSetIndex].isSelf) {
					SinglePositive singlep = (SinglePositive)skillSet [skillSetIndex];
					character.GetComponent<CharacterStats> ().Buff (singlep.affectedStat, singlep.amount);
				} else {
					AoEPositive aoep = (AoEPositive)skillSet [skillSetIndex];
					character.GetComponent<CharacterStats> ().Buff (aoep.affectedStat, aoep.amount);
				}
			}
		}
	}


}
