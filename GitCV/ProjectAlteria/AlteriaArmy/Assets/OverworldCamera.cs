using UnityEngine;
using System.Collections;

public class OverworldCamera : MonoBehaviour {


	float cameraSpeed;

	// Use this for initialization
	void Start () {
		Vector3 center = new Vector3 (Screen.width / 2, Screen.height / 2);

		Ray ray = Camera.main.ViewportPointToRay(center);
		Debug.DrawRay (ray.origin, ray.direction);

		cameraSpeed = 1;
	}
	
	// Update is called once per frame
	void Update () {

		if (!GameControl.gameControl.dialogue.dialogueSceneRunning && !GameControl.gameControl.ui.menuOpen && !GameControl.gameControl.ui.characterMenuOpen && !GameControl.gameControl.ui.preBattleMenuOpen) {
			if (Input.mousePosition.x > Screen.width * 0.85f && Input.mousePosition.x < Screen.width) {
				transform.position += new Vector3 (cameraSpeed, 0, 0);
			}
			if (Input.mousePosition.y > Screen.height * 0.85f && Input.mousePosition.y < Screen.height) {
				transform.position += new Vector3 (0, 0, cameraSpeed);
			}
			if (Input.mousePosition.x < Screen.width * 0.15f && Input.mousePosition.x > 0) {
				transform.position -= new Vector3 (cameraSpeed, 0, 0);
			}
			if (Input.mousePosition.y < Screen.height * 0.15f && Input.mousePosition.y > 0) {
				transform.position -= new Vector3 (0, 0, cameraSpeed);
			}
		}
	}
}
