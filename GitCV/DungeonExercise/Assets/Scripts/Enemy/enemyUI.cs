using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class enemyUI : MonoBehaviour {

	EnemySpawner enemySpawner;
	GameObject enemyUI_Instance;
	GameObject damageText;

	public int instanceID;

	public Text enemyName;
	Text enemyHealthPercent;

	EnemyHealth enemyHealth;
	Slider enemyHealthBar;

	float enemyHealthPercentText;
	bool damageTextActive;
	float timer;

	GameObject targetingImage;
	public bool targeted;
	float animateTimer;
	float imageY = 5f;


	// Use this for initialization
	void Start () {
		//Get enemySpawner
		enemySpawner = GameObject.Find("GameController").GetComponent<EnemySpawner>();

		//Identify this instance
		enemyUI_Instance = enemySpawner.enemyUIInstances[instanceID];

		//Get components
		enemyName = enemyUI_Instance.GetComponentInChildren<Text>();
		enemyHealth = GetComponent<EnemyHealth>();
		enemyHealthBar = enemyUI_Instance.GetComponentInChildren<Slider>();

		//Enemy's name
		enemyName.text = gameObject.name;

		//Get enemy health value to UI bar
		enemyHealthBar.maxValue = enemyHealth.maxHealth;
		enemyHealthBar.value = enemyHealth.maxHealth;

		targetingImage = GameObject.Find("TargetingArrow");


	}
	

	void Update () {

		if(enemyUI_Instance)
		{
		// Keep ui position on enemy position
		enemyName.transform.position = transform.position + new Vector3(.2f, 1f, .1f);
		enemyHealthBar.gameObject.transform.position = transform.position + new Vector3(-.1f, .8f, .1f);


		//Health bar value update
		enemyHealthBar.value = enemyHealth.currentHealth;
		}


		if(damageTextActive)
		{
			timer += Time.deltaTime;
			damageText.transform.position += new Vector3(0f, .01f, 0f);
			if (timer >= 2)
			{
				Destroy(damageText);
				timer = 0;
				damageTextActive = false;
			}
		}
			
		//targetingImage.SetActive(targeted);

		if(targeted)
		{
			targetingImage.transform.SetParent(GameObject.Find("EnemyUI").transform);
			float UIx = enemyUI_Instance.transform.position.x;
			float UIz = enemyUI_Instance.transform.position.z;
			targetingImage.transform.position = new Vector3(UIx, transform.position.y + 3f, UIz);
		}
	}
		
	//When enemy dies, UI is destroyed
	public void DestroyUI()
	{
		Destroy(enemyUI_Instance);
	}

	public void DisplayDamageTaken(int damageTaken)
	{
		if(damageText)
		{
			Destroy(damageText);

		}	
		damageText = Instantiate(Resources.Load("EnemyDamageTakenText")) as GameObject;

		damageText.transform.SetParent(GameObject.Find("EnemyUI").transform);
		damageText.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
		damageText.transform.localScale = new Vector3(1f, 1f, 1f);
		damageText.transform.position =  transform.position + new Vector3(.4f, .4f, .1f);

		damageText.GetComponent<Text>().text = "-" + damageTaken.ToString();
		damageTextActive = true;
	}


	public void SetTarget(bool mode)
	{
		targeted = mode;
	}
}
