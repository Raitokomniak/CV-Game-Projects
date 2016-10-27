using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	
	public GridScript grid;

	public int X_pos;
	public int Z_pos;

	public bool moving;
	public bool movingHorizontally;
	Vector3 newPosition;


	// Update is called once per frame
	void Update () {

		//TEMPORARILY HERE
		if (GameControl.gameControl.scene.scene == 1) {
			
			grid = GameObject.Find ("Grid").GetComponent<GridScript> ();

			X_pos = Mathf.RoundToInt (transform.position.x / 2);
			Z_pos = Mathf.RoundToInt (transform.position.z / 2);
		}


		if (moving) {
			transform.position = Vector3.Lerp (this.transform.position, newPosition, 10f * Time.deltaTime);
			if (movingAnimation (newPosition)) {
				//StartCoroutine (movingAnimation ());
				moving = false;
				GameControl.gameControl.phase.SoftResetPhase ();
			}
		}
	}

	public void Move(int givenX, int givenZ){
		newPosition = new Vector3 (givenX * 	2, transform.position.y, givenZ * 2);
		moving = true;

		if (givenX == X_pos) {
			movingHorizontally = false;
		} else {
			movingHorizontally = true;
		}

		X_pos = givenX;
		Z_pos = givenZ;
	}


	bool movingAnimation(Vector3 targetPosition)
	{
		float startPos;
		float endPos;

		if (movingHorizontally) {
			startPos = transform.position.x;
			endPos = targetPosition.x;
		} else {
			startPos = transform.position.z;
			endPos = targetPosition.z;
		}


		if (Mathf.Approximately (startPos, endPos)) {
			return true;
		} else {
			moving = true;
			return false;
		}
	}

	IEnumerator movingAnimation()
	{
		Debug.Log ("wait period");
		yield return new WaitForSeconds (2);
	}
}
