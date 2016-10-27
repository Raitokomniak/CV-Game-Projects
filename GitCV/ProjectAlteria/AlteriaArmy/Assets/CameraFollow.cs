using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject target;
	bool rotatable;


	Vector3 offset;         //Private variable to store the offset distance between the player and camera

	void Awake() 
	{
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.

	}

	public void SetTarget(GameObject givenTarget){
		target = givenTarget;

		Plane plane = new Plane(Vector3.up, Vector3.zero);
		Vector3 v3Center = new Vector3 (0.5f, 0.5f, 0);

		Ray ray = Camera.main.ViewportPointToRay(v3Center);
		Debug.DrawRay (ray.origin, ray.direction);
		float fDist;

		if (plane.Raycast(ray, out fDist))
		{
			Vector3 v3Hit   = ray.GetPoint (fDist);
			float v3DeltaX = target.transform.position.x - v3Hit.x;
			float v3DeltaY = (target.transform.position.z - v3Hit.z) / 2;
			Camera.main.transform.Translate(new Vector3(v3DeltaX, v3DeltaY, 0));
			offset = transform.position - target.transform.position;
		}

		//
		//transform.position = target.transform.position + offset;
	}

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		if (target != null && target.GetComponent<Movement>().moving) {
			
			transform.position = target.transform.position + offset;
		}

		if (Input.GetAxis ("Mouse ScrollWheel") != 0) {
			ScrollZoom (Input.GetAxis ("Mouse ScrollWheel"));
		}

		if (Input.GetKey (KeyCode.Mouse2)) {
			rotatable = true;
		} else {
			rotatable = false;
		}

		if (Input.GetAxis ("Mouse Y") != 0 && rotatable) {
			transform.rotation = transform.rotation * Quaternion.Euler(-Input.GetAxis ("Mouse Y"), 0, 0);
			//transform.RotateAround(this.transform.position, offset, 30);
			//target = GameObject.Find ("Player 1");
			//transform.RotateAround (transform.position - offset, Vector3.down, 30 * Time.deltaTime);
		}
	}

	void ScrollZoom(float zoomValue){
		Camera.main.fieldOfView -= zoomValue * 15;
	}
}
