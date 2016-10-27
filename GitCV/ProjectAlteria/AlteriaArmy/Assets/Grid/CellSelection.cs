using UnityEngine;
using System.Collections;

public class CellSelection : MonoBehaviour {

	Camera mainCamera;

	GridScript grid;
	SpriteRenderer renderer;
	AccessibleArea AAD;

	public Color defaultColor;
	public Color hoveringColor;

	public int X_index;
	public int Z_index;

	public int cellIndex;

	void Awake(){
		grid = GameObject.Find ("Grid").GetComponent<GridScript> ();
		AAD = grid.GetComponent<AccessibleArea> ();

		transform.SetParent (grid.transform, false);
		renderer = GetComponent<SpriteRenderer>();
		mainCamera = Camera.main;

		X_index = grid.cellX;
		Z_index = grid.cellZ;
		name = "Cell" + grid.cellX.ToString () + "_" + grid.cellZ.ToString ();

		defaultColor = Color.white;
		hoveringColor = Color.gray;

	}
	// Update is called once per frame
	void Update () {

		//Define ray
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit ();

		//Cast ray
		if (Physics.Raycast (ray, out hit)) {
			if (!GameControl.gameControl.ui.menuOpen) {
				//See if the ray hit this gameobject. If it did, render hovering color
				if (hit.collider.gameObject == this.gameObject) {
					this.renderer.color = hoveringColor;

					//If the cell is clicked, check phase and if the cell is accessable
					if (Input.GetKeyDown (KeyCode.Mouse0)) {
						this.renderer.color = hoveringColor;

						if (GameControl.gameControl.phase.GetPhase () == "Moving Phase") {

							if (AAD.CheckIfCellAccessible (this.gameObject)) {
								GameControl.gameControl.phase.selectedPlayerCharacter.GetComponent<Movement> ().Move (X_index, Z_index);
								GameControl.gameControl.turn.UpdateTurn ("Moving Phase", GameControl.gameControl.phase.selectedPlayerCharacter);
							}
						} else if (GameControl.gameControl.phase.GetPhase () == "Attack Phase") {

							if (AAD.CheckIfCellAccessible (this.gameObject)) {
								Debug.Log ("Attack!");
								GameControl.gameControl.phase.selectedPlayerCharacter.GetComponent<CharacterAttack> ().Attack (this.gameObject);
								GameControl.gameControl.turn.UpdateTurn ("Attack Phase", GameControl.gameControl.phase.selectedPlayerCharacter);
							}
						} else if (GameControl.gameControl.phase.GetPhase () == "Special Phase") {

							if (AAD.CheckIfCellAccessible (this.gameObject)) {
								Debug.Log ("Special Attack!");
								GameControl.gameControl.phase.selectedPlayerCharacter.GetComponent<CharacterSkill> ().DealSingleCellEffect (this.gameObject);
								GameControl.gameControl.turn.UpdateTurn ("Attack Phase", GameControl.gameControl.phase.selectedPlayerCharacter);
							}
						}


					} 
				}
			//If it didn't hit, render default color
			else {
					this.renderer.color = defaultColor;
				}
			}
		}
	}

}
