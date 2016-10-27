using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerSpell : MonoBehaviour {

	public bool spellTargetingActive;



	//DEFINING RELATED OBJECTS
	GameObject player;
	GameObject spellTargetPoint;
	GameObject spellTargetParticles;
	public GameObject rangeRing;


	Animator anim;
	SpellProcessing spellProcessing;
	PlayerEnergy energy;
	GameObject gameController;
	UIController ui;

	//FOR TARGETING
	Vector3 destinationPos;
	float destinationDist;
	Vector3 mousePosition;
	Vector3 spellCastPoint;

	//SPELLS

	public int spellID;

	//SPELLCASTING RANGE
	float maxX = 2.1f;
	float minX = -2.1f;
	float maxZ = 2.1f;
	float minZ = -2.1f;
	Vector3 playerSpellCastRange;
	bool inCastingRange;

	//MASKS FOR TARGETING
	LayerMask floorMask;
	LayerMask environmentMask;


	//float timer = 0.1f;

	// Use this for initialization
	void Start () {
		


		spellTargetPoint = GameObject.Find("SpellTargetPoint");
		spellTargetPoint.GetComponent<Light>().enabled = false;

		gameController = GameObject.Find("GameController");
		ui = gameController.GetComponent<UIController>();

		spellProcessing = gameController.GetComponent<SpellProcessing>();

		spellTargetParticles = GameObject.Find("Particles");
		spellTargetParticles.SetActive(false);

		anim = GetComponent<Animator>();

		player = GameObject.Find("Player");
		floorMask = LayerMask.GetMask("Floors");
		environmentMask = LayerMask.GetMask("EnvironmentObstacle");

		energy = player.GetComponent<PlayerEnergy>();

	}
	
	// Update is called once per frame
	void Update () {

		GetKeyInput();


		if(spellTargetingActive)
		{

				//MOVE TARGETING GRAPHIC WITH MOUSE

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayhit;

			playerSpellCastRange = transform.position - spellTargetPoint.transform.position;
			findPlayerSpellCastRange();

			if (Physics.Raycast(ray, out rayhit, Mathf.Infinity, floorMask))
				{
				Debug.DrawRay(ray.origin, ray.direction);
				Debug.DrawLine(ray.origin, rayhit.point, Color.red);

				Vector3 targetPoint = new Vector3(rayhit.point.x, rayhit.point.y + 0.2f, rayhit.point.z);
				spellTargetPoint.transform.position = targetPoint;
				spellTargetPoint.GetComponent<SphereCollider>().center = transform.position;

				rangeRing.transform.rotation = rayhit.collider.transform.rotation;
		}


			// CAST SPELL ON CURRENT POSITION
			if (Input.GetMouseButtonDown(0))
			{
				if(spellProcessing.coolDownTimer[spellID] <= 0)
				{
					if(inCastingRange)
					{
						if(energy.currentEnergy >= spellProcessing.energyCost[spellID]) 
							{
								energy.useEnergy(spellProcessing.energyCost[spellID]);
								enableTargetingGraphic(false);
								anim.SetTrigger("CastSpell");
								spellProcessing.SpellCast(spellID);
								ui.warningText.gameObject.SetActive(false);
							}

						else //if not enough energy
						{ ui.SetWarningText("Not enough energy"); }
					}

					else //if not in range
					{ ui.SetWarningText("Not in range"); }
				}

				else //if cooldown still on
				{ ui.SetWarningText("Spell not ready yet"); }
			}
		}

	
	}



	void enableTargetingGraphic(bool mode)
	{
		spellTargetingActive = mode;
		spellTargetParticles.SetActive(mode);
		rangeRing.SetActive(mode);
		rangeRing.transform.position = transform.position;
	}


	void findPlayerSpellCastRange()
	{
		if( playerSpellCastRange.x <= maxX &&
			playerSpellCastRange.x >= minX &&
			playerSpellCastRange.z <= maxZ &&
			playerSpellCastRange.z >= minZ 
			)
		{
			Debug.DrawRay(transform.position, -playerSpellCastRange, Color.green);
			LayerMask environmentMask = LayerMask.GetMask("EnvironmentObstacle");

			Ray ray = new Ray(transform.position, -playerSpellCastRange);
			RaycastHit hit;

			inCastingRange = true;

			/*if (Physics.Raycast(ray, out hit, Mathf.Infinity, environmentMask))
			{
				Debug.Log("hit envi");
				Debug.DrawRay(transform.position, -playerSpellCastRange, Color.yellow);
				inCastingRange = false;
			}
			else
			{
				inCastingRange = true;
			}*/


		}
		else
		{
			Debug.DrawRay(transform.position, -playerSpellCastRange, Color.red);
			inCastingRange = false;
		}

	}



	void GetKeyInput()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if(spellID != 1){
				enableTargetingGraphic(true);
				spellID = 1;
			}
			else {
				enableTargetingGraphic(false);
				spellID = 0;
			}
		}

		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			if(spellID != 2)
			{
				enableTargetingGraphic(true);
				spellID = 2;
			}
			else
			{
				enableTargetingGraphic(false);
				spellID = 0;
			}
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			if(spellID != 3)
			{
				enableTargetingGraphic(true);
				spellID = 3;
			}
			else
			{
				enableTargetingGraphic(false);
				spellID = 0;
			}
		}

		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			spellID = 4;
			spellProcessing.SpellCast(4);
		}

	}

}
