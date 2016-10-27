using UnityEngine;
using System.Collections;

public class NPCController : MonoBehaviour {

	GameObject player;
	UIController ui;
	GameObject GC;

	bool interacting;
	Quaternion initialRotation;

	// Use this for initialization
	void Start () {
		GC = GameObject.Find("GameController");
		ui = GC.GetComponent<UIController>();

		player = GameObject.Find("Player");
		initialRotation = transform.rotation;

		ui.setName("Rylai", "<Quest Giver>");
	}
	
	// Update is called once per frame
	void Update () {
		if(interacting)
		{
			InteractWithPlayer();
			ui.toggleDialogPanel(true);
		}

		else if(!interacting)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, 0.1f);
			ui.toggleDialogPanel(false);
		}

		ui.updateNPCNamePosition();
	}

	public void InteractWithPlayer()
	{
		//MAKE NPC ROTATE TOWARDS PLAYER WHEN INTERACTING
		Vector3 targetRotation = player.transform.position;
		Vector3 offSet = transform.position - player.transform.position;
		Ray attentionRay = new Ray(transform.position, -offSet);

		Debug.DrawRay(transform.position, -offSet, Color.red);
		//transform.rotation = Quaternion.FromToRotation(transform.position, -offSet);
		//transform.rotation = Quaternion.Euler(360, player.transform.rotation.y, 360);

		//IF OUT OF RANGE, DISABLE INTERACTING
		if(offSet.magnitude > 10)
		{
			toggleInteract(false);
		}
	}

	public void toggleInteract(bool mode)
	{
		interacting = mode;
	}

	public void forceOffInteract()
	{
		interacting = false;
	}
}
