using UnityEngine;
using System.Collections;

public class PlayerSelection : MonoBehaviour {
	
	public bool playerSelected;
	public GameObject grid;

	void Awake(){
		playerSelected = false;
	}

	// Update is called once per frame
	void Update () {

		//TEMPORARILY HERE
		grid = GameObject.Find ("Grid");
	
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit ();

		//Cast ray, see if hits anything, if hits player
		if (Physics.Raycast (ray, out hit)) {
			
			if (!GameControl.gameControl.ui.menuOpen) {
				if (hit.collider.gameObject == this.gameObject) {
					
					//If clicked player, select player if not yet selected. If already selected, deselect
					if (Input.GetKeyDown (KeyCode.Mouse0) && GameControl.gameControl.turn.IsPlayersTurn ()) {
						if (!playerSelected) {
							Select (true);
							Camera.main.GetComponent<CameraFollow> ().SetTarget (this.gameObject);
						} else {
							Select (false);
							GameControl.gameControl.ui.ToggleCharacterActionPanel (false, this.gameObject);
						}
					}
				}

			//If clicked on something else than the player, deselect player
			else {
					if (Input.GetKeyDown (KeyCode.Mouse0) && playerSelected && hit.collider.tag != "Button") {
						Select (false);
						//gameController.GetComponent<UIController> ().ToggleCharacterActionPanel (false, this.gameObject);
					}
				}
			}
		}

	}


	void Select(bool selected){
		if (selected) {
			GameControl.gameControl.phase.SetSelectedPlayerCharacter (this.gameObject);
			GameControl.gameControl.ui.ToggleCharacterActionPanel (true, this.gameObject);
			gameObject.GetComponent<Renderer> ().material.color = Color.blue;
		} 
		else {
			gameObject.GetComponent<Renderer> ().material.color = Color.white;
		}

		playerSelected = selected;
	}
}
