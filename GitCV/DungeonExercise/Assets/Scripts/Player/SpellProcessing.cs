using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpellProcessing : MonoBehaviour {

	Animator anim;
	ParticleSystem part;
	PlayerEnergy energy;
	AudioController audio;

	//FIND RELATED OBJECTS
	GameObject player;
	GameObject spellTargetPoint;
	GameObject spellTargetParticles;
	public GameObject spellParticles;


	Slider ChannelingSlider;
	float totalSliderTime;


	//FOR CASTING
	Vector3 spellCastPoint;
	Vector3 particlePosition;
	LayerMask enemyMask;
	Collider[] colliders;
	PlayerHealth health;

	public int[] spellDamage;
	public int[] energyCost;
	public int[] energyGain;
	bool channeling;
	bool selfCastAnimating;

	public int spellID;
	public int numberOfSpells = 10;
	public float[] coolDown;
	public float[] coolDownTimer;
	public bool[] coolDownOn;
	public int[] coolDownStore;

	//UI
	Text[] coolDownTimerText;

	float globalParticleDestroyTimer;


	void Awake () {
		audio = GetComponent<AudioController>();

		player = GameObject.Find("Player");
		enemyMask = LayerMask.GetMask("Enemies");

		energy = player.GetComponent<PlayerEnergy>();
		spellTargetParticles = GameObject.Find("Particles");

		health = player.GetComponent<PlayerHealth>();

		ChannelingSlider = GameObject.Find("ChannelingSlider").GetComponent<Slider>();
		ChannelingSlider.gameObject.SetActive(false);


		spellDamage = new int[numberOfSpells];
		spellDamage[1] = 10;
		spellDamage[2] = 10;
		spellDamage[3] = 30;
		spellDamage[4] = 0;

		energyCost = new int[numberOfSpells];
		energyCost[1] = 0;
		energyCost[2] = 0;
		energyCost[3] = 30;
		energyCost[4] = 0;

		energyGain= new int[numberOfSpells];
		energyGain[1] = 10;
		energyGain[2] = 10;
		energyGain[3] = 0; 
		energyGain[4] = 0;

		coolDownTimerText = new Text[numberOfSpells];

		coolDownTimerText[1] = GameObject.Find("spell_Icon01").GetComponentInChildren<Text>();
		coolDownTimerText[2] = GameObject.Find("spell_Icon02").GetComponentInChildren<Text>();
		coolDownTimerText[3] = GameObject.Find("spell_Icon03").GetComponentInChildren<Text>();
		coolDownTimerText[4] = GameObject.Find("spell_Icon04").GetComponentInChildren<Text>();

		coolDown = new float[numberOfSpells];
		coolDownTimer = new float[numberOfSpells];
		coolDownOn = new bool[numberOfSpells];

		coolDownStore = new int[numberOfSpells];

		coolDown[1] = 3f;
		coolDown[2] = 3f;
		coolDown[3] = 1f;
		coolDown[4] = 1.5f;

		globalParticleDestroyTimer = 5f;

		for (int i = 0; i < numberOfSpells; i++)
		{
			coolDownTimer[i] = 0;
		}
			

		totalSliderTime = 0f;


	}
	
	// Update is called once per frame
	void Update () {

		ChangeCoolDownText();

		//DESTROY SPELL PARTICLES WHEN COOLDOWN IS OFF
		ParticleDestroyer();



		if(channeling)
		{
			totalSliderTime += Time.deltaTime;
			ChannelingSlider.value = totalSliderTime;

			if(totalSliderTime >= coolDown[4])
			{
				SelfSpellCast(4);
				totalSliderTime = 0;
				channeling = false;
				ChannelingSlider.gameObject.SetActive(false);
				ChannelingSlider.GetComponent<Slider>().maxValue = 1;
			}
		}

		if(selfCastAnimating && spellParticles.activeSelf)
		{

			for(float i = 0; i <= 4f; i+=Time.deltaTime * 0.1f)
			{
				spellParticles.transform.position = player.transform.position;
			}

		}
	}





	public void SpellCast(int spellID)
	{
		if(spellID==4)
		{
			Channel(spellID);
		}
		else{
		spellTargetPoint = GameObject.Find("SpellTargetPoint");

		Animate(spellID);
			audio.PlaySoundAudio2("Audio/FX/Spell_00");
		coolDownOn[spellID] = true;
		spellCastPoint = spellTargetPoint.transform.position;
		coolDownTimer[spellID] = coolDown[spellID];

		

		//CHECK COLLISION WITH ENEMY
		int i = 0;
		colliders = Physics.OverlapSphere(spellTargetPoint.transform.position, 1f, enemyMask);
		while (i < colliders.Length - 1)
		{
			Debug.Log("Osui");

			colliders[i].GetComponent<EnemyHealth>().takeDamage(spellDamage[spellID]);
			//colliders[i].GetComponent<Animator>().SetTrigger("Knockback");
			energy.gainEnergy(energyGain[spellID]);
			colliders[i].GetComponent<EnemyMovement>().Aggro = true;
			i++;
		}
	
		}
	}

	public void Channel(int spellID)
	{
		channeling = true;
		ChannelingSlider.gameObject.SetActive(true);
		ChannelingSlider.GetComponent<Slider>().maxValue = coolDown[spellID];
	}

	public void SelfSpellCast(int spellID)
	{
		if(spellID==4)
		{
			health.heal(5);
			coolDownOn[spellID] = true;
			coolDownTimer[spellID] = coolDown[spellID];
			Animate(4);
		}
	}

	void Animate(int spellID)
	{
		spellTargetPoint = GameObject.Find("SpellTargetPoint");

		switch(spellID)
		{
		case 1:
			selfCastAnimating = false;
			spellParticles = Instantiate(Resources.Load("HolyBlast"), spellTargetPoint.transform.position, spellTargetPoint.transform.rotation) as GameObject;
			spellParticles.transform.position = spellTargetPoint.transform.position;
			break;
		case 2:
			selfCastAnimating = false;
			spellParticles = Instantiate(Resources.Load("Explosion"), spellTargetPoint.transform.position, spellTargetPoint.transform.rotation) as GameObject;
			spellParticles.transform.position = spellTargetPoint.transform.position;
			break;
		case 3:
			selfCastAnimating = false;
			spellParticles = Instantiate(Resources.Load("Lightning Spark"), spellTargetPoint.transform.position, spellTargetPoint.transform.rotation) as GameObject;
			spellParticles.transform.position = spellTargetPoint.transform.position;
			break;
		case 4:
			selfCastAnimating = false;
			spellParticles = Instantiate(Resources.Load("Flame Enchant"), player.transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
			spellParticles.transform.position = player.transform.position;
			selfCastAnimating = true;
			break;
		}

		spellParticles.SetActive(true);
		part = spellParticles.GetComponent<ParticleSystem>();

		part.Emit(10);



	}


	void ChangeCoolDownText()
	{
		//CHANGE UI COOLDOWN TIMER TEXT


		if (coolDownOn[1] && coolDownTimer[1] > 0.0f)
		{
			coolDownTimerText[1].text = coolDownTimer[1].ToString("F1");
		}

		if(coolDownOn[2] && coolDownTimer[2] > 0.0f)
		{
			coolDownTimerText[2].text = coolDownTimer[2].ToString("F1");
		}
		if(coolDownOn[3] && coolDownTimer[3] > 0.0f)
		{
			coolDownTimerText[3].text = coolDownTimer[3].ToString("F1");
		}
		if(coolDownOn[4] && coolDownTimer[4] > 0.0f)
		{
			coolDownTimerText[4].text = coolDownTimer[4].ToString("F1");
		}

	}



	void ParticleDestroyer()
	{
		int i = 0 ;

		if(spellParticles)
		{
			spellParticles.tag = "SpellParticles";
			GameObject[] toDestroy = GameObject.FindGameObjectsWithTag("SpellParticles");
			globalParticleDestroyTimer -= Time.deltaTime;

			if (globalParticleDestroyTimer <= 0)
			{
				Destroy(toDestroy[i]);

				i++;
				if (i == 10){ i = 0; }

				globalParticleDestroyTimer = 2f;
			}
		}
		else{
			globalParticleDestroyTimer = 3f;
		}



		if (coolDownTimer[spellID] <= 0 && coolDownOn[spellID])
		{
			coolDownOn[spellID] = false;
		}
		else
		{
			coolDownTimer[1] -= Time.deltaTime;
			coolDownTimer[2] -= Time.deltaTime;
			coolDownTimer[3] -= Time.deltaTime;
			coolDownTimer[4] -= Time.deltaTime;
		}

	}
}
