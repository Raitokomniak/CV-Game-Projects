using UnityEngine;
using System.Collections;
using System.Linq;

public class AccessibleArea : MonoBehaviour {

	GridScript grid;
	public ArrayList accessibleCells;

	// Use this for initialization
	void Start () {
		accessibleCells = new ArrayList ();
	}

	public void Setup(){
		grid = GetComponent<GridScript> ();
		accessibleCells = new ArrayList();
	}

	// Starts Moving Phase and Attack Phase
	// Determines player's current location
	// Finds nearby cells and makes them accessible
	// Changes accessible cells' color theme

	public void DetermineArea(GameObject target){
		
		string phase = GameControl.gameControl.phase.GetPhase ();

		int currentPos_X = target.GetComponent<Movement> ().X_pos;
		int currentPos_Z = target.GetComponent<Movement> ().Z_pos;

		GameObject currentCell = GameObject.Find ("Cell" + currentPos_X.ToString() + "_" + currentPos_Z.ToString ());

		if (phase == "Moving Phase") {
			accessibleCells = new ArrayList ();
			int moveRange = target.GetComponent<CharacterStats> ().moveRange;

			DeterminePattern ("Moving", moveRange, currentPos_X, currentPos_Z);

			Filter("Enemies");
			Filter ("Players");
			Filter("Obstacles");

			foreach (GameObject cell in accessibleCells.ToArray()) {
				
				if (cell != null && GameControl.gameControl.playerSpawner.playerList.Contains (target)) {
					cell.GetComponent<CellSelection> ().defaultColor = Color.cyan;
					cell.GetComponent<CellSelection> ().hoveringColor = Color.green;
				}
			}

		} else if (phase == "Attack Phase") {
			accessibleCells = new ArrayList ();
			DeterminePattern ("Attack", 1, currentPos_X, currentPos_Z);

			Filter ("Obstacles");

			foreach (GameObject cell in accessibleCells) {
				if (cell != null && GameControl.gameControl.playerSpawner.playerList.Contains (target)) {
					cell.GetComponent<CellSelection> ().defaultColor = Color.magenta;
					cell.GetComponent<CellSelection> ().hoveringColor = Color.red;
				}
			}

		} else if (phase == "Aggro Phase") {
			accessibleCells = new ArrayList ();

			DeterminePattern ("Moving", 2, currentPos_X, currentPos_Z);

			Filter ("Obstacles");

			foreach (GameObject cell in accessibleCells) {
				if (cell != null && GameControl.gameControl.playerSpawner.playerList.Contains (target)) {
					cell.GetComponent<CellSelection> ().defaultColor = Color.red;
					//accessibleCells [i].GetComponent<CellSelection> ().hoveringColor = Color.red;
				}
			}
		} else if (phase == "Special Phase") {
			accessibleCells = new ArrayList ();
			if (GameControl.gameControl.phase.specialPhase == 0) {
				DeterminePattern ("Attack", 2, currentPos_X, currentPos_Z);
				Filter ("Players");
				foreach (GameObject cell in accessibleCells) {
					if (cell != null && GameControl.gameControl.playerSpawner.playerList.Contains (target)) {
						cell.GetComponent<CellSelection> ().defaultColor = Color.red;
					}
				}
			}
			else if (GameControl.gameControl.phase.specialPhase == 1) {
				DeterminePattern ("Moving", 1, currentPos_X, currentPos_Z);
				Filter("Enemies");
				foreach (GameObject cell in accessibleCells) {
					if (cell != null && GameControl.gameControl.playerSpawner.playerList.Contains (target)) {
						cell.GetComponent<CellSelection> ().defaultColor = Color.green;
					}
				}
			}
			else if (GameControl.gameControl.phase.specialPhase == 2) {
				DeterminePattern ("Attack", 1, currentPos_X, currentPos_Z);
				Filter ("Players");
				foreach (GameObject cell in accessibleCells) {
					if (cell != null && GameControl.gameControl.playerSpawner.playerList.Contains (target)) {
						cell.GetComponent<CellSelection> ().defaultColor = Color.red;
					}
				}
			}
			else if (GameControl.gameControl.phase.specialPhase == 3) {
				DeterminePattern ("Attack", 1, currentPos_X, currentPos_Z);
				Filter("Enemies");
				foreach (GameObject cell in accessibleCells) {
					if (cell != null && GameControl.gameControl.playerSpawner.playerList.Contains (target)) {
						cell.GetComponent<CellSelection> ().defaultColor = Color.green;
					}
				}
			}
			else if (GameControl.gameControl.phase.specialPhase == 4) {
				DeterminePattern ("Self", 0, currentPos_X, currentPos_Z);

				foreach (GameObject cell in accessibleCells) {
					if (cell != null && GameControl.gameControl.playerSpawner.playerList.Contains (target)) {
						cell.GetComponent<CellSelection> ().defaultColor = Color.green;
					}
				}
			}
		}

	}

	public ArrayList GetAccessibleEnemyCells(){

		int currentPos_X = 0;
		int currentPos_Z = 0;

		GameObject randomPlayer = GameControl.gameControl.playerSpawner.playerList[Random.Range (0, GameControl.gameControl.playerSpawner.playerList.Count - 1)] as GameObject;

		currentPos_X = randomPlayer.GetComponent<Movement> ().X_pos;
		currentPos_Z = randomPlayer.GetComponent<Movement> ().Z_pos;

		accessibleCells = new ArrayList();

		DeterminePattern ("Moving", 1, currentPos_X, currentPos_Z);

		Filter("Enemies");
		Filter ("Players");
		Filter("Obstacles");

		return accessibleCells;
	}


	/////////////////////////
	//CELL FILTERING
	//

	void Filter(string filter){
		ArrayList deletedCells = new ArrayList ();
		ArrayList listToCheck = new ArrayList ();
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag ("Obstacle");

		foreach (GameObject cell in accessibleCells) {
			if (cell != null) {
				if (filter == "Players") {
					foreach (GameObject player in GameControl.gameControl.playerSpawner.playerList) {
						if (player.GetComponent<Movement> ().X_pos == cell.GetComponent<CellSelection> ().X_index
						    && player.GetComponent<Movement> ().Z_pos == cell.GetComponent<CellSelection> ().Z_index) {
							deletedCells.Add (cell);
						}
					}
				} else if (filter == "Enemies") {
					foreach (GameObject enemy in GameControl.gameControl.enemySpawner.enemyList) {
						if (enemy != null && cell != null) {
							if (enemy.GetComponent<Movement> ().X_pos == cell.GetComponent<CellSelection> ().X_index
							    && enemy.GetComponent<Movement> ().Z_pos == cell.GetComponent<CellSelection> ().Z_index) {
								deletedCells.Add (cell);
							}
						}
					}
				} else if (filter == "Obstacles") {
					
					for (int i = 0; i < obstacles.Length; i++) {
						if (obstacles [i].GetComponent<Obstacle> ().X_pos == cell.GetComponent<CellSelection> ().X_index
						    && obstacles [i].GetComponent<Obstacle> ().Z_pos == cell.GetComponent<CellSelection> ().Z_index) {
							deletedCells.Add (cell);
						}
					}
				} else if (filter == "DeadCells") {
					int X_pos = cell.GetComponent<CellSelection> ().X_index;
					int Z_pos = cell.GetComponent<CellSelection> ().Z_index;

					if (GameObject.Find ("Cell" + X_pos.ToString () + "_" + Z_pos.ToString ()) == null) {
						deletedCells.Add(cell);
					}
				}
			}
		}
		foreach (GameObject cell in deletedCells) {
			accessibleCells.Remove (cell);
		}
	}
		

	/////////////////////////////////
	// Turns off AAD
	// Empties list of accessible cells
	// Resets cell color theme

	public void TurnOff(){

		ArrayList deletedCells = new ArrayList ();

		if (accessibleCells != null) {
			foreach (GameObject cell in accessibleCells) {
				if (cell != null) {
				
					cell.GetComponent<CellSelection> ().defaultColor = Color.white;
					cell.GetComponent<CellSelection> ().hoveringColor = Color.gray;
					deletedCells.Add (cell);
				}
			}
			foreach (GameObject cell in deletedCells) {
				accessibleCells.Remove (cell);
			}
		}
	}


	// Checks if given cell matches the list of accessible cells
	public bool CheckIfCellAccessible(GameObject cell){
		foreach (GameObject aCell in accessibleCells) {
			if (aCell == cell) {
				return true;
			}
		}
		return false;
	}


	void DeterminePattern(string type, int patternIndex, int currentPos_X, int currentPos_Z){

		string X0 = (currentPos_X).ToString ();
		string Z0 = (currentPos_Z).ToString ();

		GameObject left_1 = GameObject.Find ("Cell" + (currentPos_X - 1).ToString () + "_" + Z0);
		GameObject right_1 = GameObject.Find ("Cell" + (currentPos_X + 1).ToString () + "_" + Z0);
		GameObject down_1 = GameObject.Find ("Cell" + X0 + "_" + (currentPos_Z - 1).ToString ());
		GameObject up_1 = GameObject.Find ("Cell" + X0 + "_" + (currentPos_Z + 1).ToString ());



		if (type == "Attack") {
			switch (patternIndex) {
			// Pattern 1  / Default attack
			//		.	
			//	.		.
			//		.

			case 1:
				accessibleCells.Add (left_1);
				accessibleCells.Add (right_1);
				accessibleCells.Add (down_1);
				accessibleCells.Add (up_1);
				break;

			// Pattern 2  / Special test
			//	.	.	.
			//	.		.
			//	.	.	.
			case 2:
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X - 1).ToString () + "_" + (currentPos_Z + 1).ToString ()));
				accessibleCells.Add (up_1);
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X + 1).ToString () + "_" + (currentPos_Z + 1).ToString ()));

				accessibleCells.Add (left_1);
				accessibleCells.Add (right_1);

				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X - 1).ToString () + "_" + (currentPos_Z - 1).ToString ()));
				accessibleCells.Add (down_1);
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X + 1).ToString () + "_" + (currentPos_Z - 1).ToString ()));
				break;
			}

		} else if (type == "Moving") {
			switch (patternIndex) {
			// Pattern 1 / Default move
			//	.	.	.
			//	.		.
			//	.	.	.
			case 1: 
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X - 1).ToString () + "_" + (currentPos_Z + 1).ToString ()));
				accessibleCells.Add (up_1);
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X + 1).ToString () + "_" + (currentPos_Z + 1).ToString ()));

				accessibleCells.Add (left_1);
				accessibleCells.Add (right_1);

				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X - 1).ToString () + "_" + (currentPos_Z - 1).ToString ()));
				accessibleCells.Add (down_1);
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X + 1).ToString () + "_" + (currentPos_Z - 1).ToString ()));
				break;



			// Pattern 2  // Aggro test pattern
			//		.	.	.
			//	.	.	.	.	.
			//	.	.		.	.
			//	.	.	.	.	.
			//		.	.	.

			case 2:

				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X - 1).ToString () + "_" + (currentPos_Z + 2).ToString ()));
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X).ToString () + "_" + (currentPos_Z + 2).ToString ()));
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X + 1).ToString () + "_" + (currentPos_Z + 2).ToString ()));

				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X - 2).ToString () + "_" + (currentPos_Z + 1).ToString ()));
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X - 1).ToString () + "_" + (currentPos_Z + 1).ToString ()));
				accessibleCells.Add (up_1);
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X + 1).ToString () + "_" + (currentPos_Z + 1).ToString ()));
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X + 2).ToString () + "_" + (currentPos_Z + 1).ToString ()));

				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X - 2).ToString () + "_" + (currentPos_Z).ToString ()));
				accessibleCells.Add (left_1);
				accessibleCells.Add (right_1);
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X + 2).ToString () + "_" + (currentPos_Z).ToString ()));

				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X - 2).ToString () + "_" + (currentPos_Z - 1).ToString ()));
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X - 1).ToString () + "_" + (currentPos_Z - 1).ToString ()));
				accessibleCells.Add (down_1);
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X + 1).ToString () + "_" + (currentPos_Z - 1).ToString ()));
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X + 2).ToString () + "_" + (currentPos_Z - 1).ToString ()));

				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X - 1).ToString () + "_" + (currentPos_Z - 2).ToString ()));
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X).ToString () + "_" + (currentPos_Z - 2).ToString ()));
				accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X + 1).ToString () + "_" + (currentPos_Z - 2).ToString ()));

				break;
			}
		} else if (type == "Self") {
			accessibleCells.Add (GameObject.Find ("Cell" + (currentPos_X).ToString () + "_" + (currentPos_Z).ToString ()));
		}


	}

}
