using UnityEngine;
using System.Collections;

public class TurnController : MonoBehaviour {

	GameObject character;
	CharacterStats stats;

	bool playersTurn;
	bool readyToEndEnemyTurn;
	bool victory;


	public void Setup(){
		victory = false;

		if (GameControl.gameControl.playerSpawner.setupReady && !GameControl.gameControl.scene.sceneLoaded) {
			character = GameObject.FindWithTag ("Player");
			PlayersTurn ();
		}
	}

	// Update is called once per frame
	void Update () {

		if (GameControl.gameControl.scene.scene == 1) {
			if (GameControl.gameControl.scene.sceneLoaded) {
				CheckEnemyTurn ();
			}

			if (GameControl.gameControl.enemySpawner.enemyList.Count == 0 && !victory) {
				GameControl.gameControl.ui.ProcessVictoryScreen ();
				victory = true;
				GameControl.gameControl.storyProgression = 1;

				foreach (GameObject player in GameControl.gameControl.playerSpawner.playerList) {
					player.GetComponent<CharacterStats> ().UpdateStats ();
				}

			}
		}
	}

	IEnumerator WaitForTurn(){
		yield return new WaitForSeconds (1f);
		GameControl.gameControl.ui.ToggleTurnPanel (true, playersTurn);
		yield return new WaitForSeconds (1f);
		GameControl.gameControl.ui.ToggleEndTurnButton (true);
		GameControl.gameControl.ui.ToggleTurnPanel (false, playersTurn);

		if (!playersTurn) {
			foreach(GameObject enemy in GameControl.gameControl.enemySpawner.enemyList){
				if (!enemy.GetComponent<EnemyAI> ().turnOver) {
					Debug.Log ("Enemy turn");
					enemy.GetComponent<EnemyAI> ().AwakeEnemy ();
					break;
				}
			}
		}
	}

	public void EndTurn(){
		if (playersTurn) {
			playersTurn = false;
			GameControl.gameControl.ui.ToggleEndTurnButton (false);
			EnemyTurn ();

		} else {
			playersTurn = true;
			PlayersTurn ();
		}
	}

	public void PlayersTurn(){
		playersTurn = true;
		foreach (GameObject player in GameControl.gameControl.playerSpawner.playerList) {
			stats = player.GetComponent<CharacterStats> ();
			stats.canStillMove = true;
			stats.canStillAttack = true;
		}
		StartCoroutine (WaitForTurn ());
	}

	public void EnemyTurn(){
		playersTurn = false;
		StartCoroutine (WaitForTurn ());
		GameControl.gameControl.phase.ResetPhase ();
		foreach(GameObject enemy in GameControl.gameControl.enemySpawner.enemyList) {
			
			stats = enemy.GetComponent<CharacterStats> ();
			enemy.GetComponent<EnemyAI> ().turnOver = false;
			stats.GetComponent<CharacterStats> ().canStillMove = true;
			stats.GetComponent<CharacterStats> ().canStillAttack = true;
		}
		character = GameControl.gameControl.enemySpawner.enemyList[0] as GameObject;
		stats = character.GetComponent<CharacterStats> ();

	}


	public void UpdateTurn(string givenPhase, GameObject target){
		stats = target.GetComponent<CharacterStats> ();

		if (givenPhase == "Moving Phase") {
			stats.canStillMove = false;
			GameControl.gameControl.phase.ResetPhase ();
		} else if (givenPhase == "Attack Phase") {
			stats.canStillAttack = false;
			GameControl.gameControl.phase.ResetPhase ();
		} else if (!playersTurn) {
			foreach (GameObject enemy in GameControl.gameControl.enemySpawner.enemyList) {
				if (!enemy.GetComponent<EnemyAI> ().turnOver) {
					character = enemy;
					StartCoroutine (WaitForTurn ());
					break;
				}

			}
		}
	}


	public bool IsPlayersTurn()
	{
		return playersTurn;
	}


	public void CheckEnemyTurn(){
		if (!playersTurn) {
			readyToEndEnemyTurn = true;
			foreach (GameObject enemy in GameControl.gameControl.enemySpawner.enemyList) {
				if (!enemy.GetComponent<EnemyAI> ().turnOver) {
					readyToEndEnemyTurn = false;
				}
			}
			if (readyToEndEnemyTurn) {
				EndTurn ();
			}
		}
	}
}
