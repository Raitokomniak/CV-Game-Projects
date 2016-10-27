using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	public int maxHealth;
	Slider healthBar;
	Text healthText;

	DeathController death;
	Animator anim;

	// Use this for initialization
	void Awake () {
		death = GetComponent<DeathController>();
		anim = GetComponent<Animator>();
		healthBar = GameObject.Find("HealthBar").GetComponentInChildren<Slider>();
		healthText = GameObject.Find("HealthText").GetComponent<Text>();

		maxHealth = 100;

		//healthText.text = SaveLoadController.saveLoadControl.health.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		healthBar.value = SaveLoadController.saveLoadControl.health;
		healthText.text = SaveLoadController.saveLoadControl.health.ToString();
	}

	public void takeDamage(int damageTaken)
	{
		if(damageTaken >= SaveLoadController.saveLoadControl.health)
		{
			SaveLoadController.saveLoadControl.health = 0;
			death.Die();
		}
		else
		{
			SaveLoadController.saveLoadControl.health -= damageTaken;
			anim.SetTrigger("Hurt");
		}
	}

	public void heal(int healAmount)
	{
		if(SaveLoadController.saveLoadControl.health<maxHealth)
		{
			SaveLoadController.saveLoadControl.health+=healAmount;
			if(SaveLoadController.saveLoadControl.health>maxHealth)
			{
				SaveLoadController.saveLoadControl.health = maxHealth;
			}
		}


	}
}
