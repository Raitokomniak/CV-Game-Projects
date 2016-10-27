using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

	GameObject player;
	GameObject grid;
	GameObject gameController;

	Movement movement;
	CharacterAttack attack;
	GameObject closestCell;

	CharacterStats stats;
	ArrayList cellsToCheck;

	public bool turnOver;

	// Use this for initialization
	void Awake () {
		movement = GetComponent<Movement> ();
		attack = GetComponent<CharacterAttack> ();
		stats = GetComponent<CharacterStats> ();

		grid = GameObject.Find ("Grid");
		player = GameObject.FindWithTag ("Player");

		stats.canStillMove = true;
		stats.canStillAttack = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AwakeEnemy(){
		turnOver = false;
		Debug.Log ("Processing " + this.gameObject.name + " turn");

		CheckAttackableArea ();

		if(stats.canStillMove){
			CheckAggroRange ();
		}

		if (stats.canStillAttack) {
			CheckAttackableArea ();
			stats.canStillMove = false;
		}

		if (!stats.canStillAttack && !stats.canStillMove) {
			turnOver = true;
			GameControl.gameControl.turn.UpdateTurn ("end", this.gameObject);
		}
	}

	IEnumerator ProcessTurn(){
		CheckAggroRange ();
		yield return new WaitForSeconds (2);
	}

	void CheckAttackableArea(){
		GameControl.gameControl.phase.SetEnemyPhase ("Attack Phase", this.gameObject);

		cellsToCheck = grid.GetComponent<AccessibleArea> ().accessibleCells;

		foreach (GameObject cell in cellsToCheck) {
			if (cell != null) {
				if (player.GetComponent<Movement> ().X_pos == cell.GetComponent<CellSelection> ().X_index
					&& player.GetComponent<Movement> ().Z_pos == cell.GetComponent<CellSelection> ().Z_index) {
					attack.Attack (cell);
					stats.canStillAttack = false;
					stats.canStillMove = false;
					return;
				}
			}
		}

		stats.canStillAttack = false;


	}

	void CheckAggroRange(){
		GameControl.gameControl.phase.SetEnemyPhase ("Aggro Phase", this.gameObject);

		cellsToCheck = grid.GetComponent<AccessibleArea> ().accessibleCells;

		foreach (GameObject cell in cellsToCheck) {
			if (cell != null) {
				if (player.GetComponent<Movement> ().X_pos == cell.GetComponent<CellSelection> ().X_index
					&& player.GetComponent<Movement> ().Z_pos == cell.GetComponent<CellSelection> ().Z_index) {
					CheckMovableArea (cell.GetComponent<CellSelection> ().X_index, cell.GetComponent<CellSelection> ().Z_index);
					break;
				}
			}
		}

		stats.canStillMove = false;
	
	}
		
	void CheckMovableArea(int X_pos, int Z_Pos){

		GameControl.gameControl.phase.SetEnemyPhase ("Aggro Phase", this.gameObject);


		cellsToCheck = grid.GetComponent<AccessibleArea> ().accessibleCells;
		ArrayList cellsToCompare = grid.GetComponent<AccessibleArea> ().GetAccessibleEnemyCells ();

		ArrayList closestCells = new ArrayList();

		foreach (GameObject cell in cellsToCheck) {
			foreach(GameObject compareCell in cellsToCompare){
				if (cell != null && compareCell != null) {
					if (cell.name == compareCell.name) {
						closestCells.Add (cell);
					}
				}
			}
		}

		if (closestCells.Count != 0) {
			GameObject closestCell = closestCells [0] as GameObject;
			movement.Move (closestCell.GetComponent<CellSelection> ().X_index, closestCell.GetComponent<CellSelection> ().Z_index);
			stats.canStillMove = false;
			stats.canStillAttack = true;
		} else {
			stats.canStillMove = false;
			stats.canStillAttack = false;
		}

	}



}
