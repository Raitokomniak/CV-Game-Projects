using UnityEngine;
using System.Collections;

public class CharacterAttack : MonoBehaviour {

	public GameObject grid;
	CharacterStats stats;
	ArrayList listToCheck;

	void Awake () {
		if (GameControl.gameControl.scene.scene == 1) grid = GameObject.Find ("Grid");

		stats = GetComponent<CharacterStats> ();
	}

	public void Attack(GameObject cell) {

		if (this.gameObject.tag == "Player") listToCheck = GameControl.gameControl.enemySpawner.enemyList;
		else if (this.gameObject.tag == "Enemy") listToCheck = GameControl.gameControl.playerSpawner.playerList;
	
		foreach (GameObject character in listToCheck) {
			if (character != null) {
				if (character.GetComponent<Movement> ().X_pos == cell.GetComponent<CellSelection> ().X_index
					&& character.GetComponent<Movement> ().Z_pos == cell.GetComponent<CellSelection> ().Z_index) {
					character.GetComponent<CharacterStats> ().TakeDamage (stats.ATK, this.gameObject);
					break;
				}
			}
		}
		GameControl.gameControl.turn.UpdateTurn ("Attack Phase", GameControl.gameControl.phase.selectedPlayerCharacter);
	}
}
