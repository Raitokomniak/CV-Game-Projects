using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	Animator anim;
	NavMeshAgent nav;
	enemyUI UI;
	PlayerExp playerExp;

	public int maxHealth;
	public Slider enemyHealthBar;
	public Text enemyHealthText;
	public Text enemyNameText;
	public int currentHealth;
	public int expReward;

	public GameObject enemy;
	EnemyMovement enemyMovement;
	Vector3 textPos;

	// Use this for initialization
	void Awake () {
		anim = GetComponent<Animator>();
		nav = GetComponent<NavMeshAgent>();
		enemyMovement = GetComponent<EnemyMovement>();
		UI = GetComponent<enemyUI>();
		playerExp = GameObject.Find("Player").GetComponent<PlayerExp>();
			
		enemy = this.gameObject;
		currentHealth = maxHealth;
		expReward = 25;

		//enemyHealthText.text = currentHealth.ToString();

		/*enemyNameText.gameObject.SetActive(true);
		enemyNameText.text = "Skeleton";
		textPos = new Vector3(985f, -35f, 0f);
		enemyNameText.transform.position = textPos;
		*/

	}

	// Update is called once per frame
	void Update () {
		//enemyNameText.transform.position = textPos;
	}

	public void takeDamage(int damageTaken)
	{

		if(!enemyMovement.Return)
		{
			if (damageTaken >= currentHealth || currentHealth <= 0)
			{
				currentHealth = 0;
				Die();
				UI.DisplayDamageTaken(damageTaken);
				enemyMovement.Aggro = false;
			}
			else 
			{
				
				anim.SetTrigger("Hurt");
				anim.SetBool("IsAttacking", false);
				currentHealth -= damageTaken;
				enemyMovement.Aggro = true;
				UI.DisplayDamageTaken(damageTaken);



				//enemyHealthBar.value = currentHealth;
				//enemyHealthText.text = currentHealth.ToString();

			}
		}
	}

	void Die()
	{
		nav.Stop();
		anim.SetTrigger("Die");
		GetComponent<CapsuleCollider>().enabled = false;
		playerExp.GainExp(expReward);
		Destroy(this.gameObject, 4f);
		UI.DestroyUI();
	}
}
