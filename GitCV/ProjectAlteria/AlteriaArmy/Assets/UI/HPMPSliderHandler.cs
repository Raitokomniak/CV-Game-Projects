using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPMPSliderHandler : MonoBehaviour {
	public GameObject target;
	Slider HPslider;
	Slider MPslider;

	CharacterStats stats;
	Transform HPText;
	Transform MPText;


	// Use this for initialization
	void Awake () {
		stats = GetComponent<CharacterStats> ();

		HPslider = gameObject.transform.GetChild (0).GetComponent<Slider>();
		MPslider = gameObject.transform.GetChild (1).GetComponent<Slider>();

		HPText = gameObject.transform.GetChild(0).GetChild (2);
		MPText = gameObject.transform.GetChild(1).GetChild (2);
	}

	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(50,0,0);

		if (target != null) {
			HPslider.transform.position = target.transform.position + new Vector3 (0, 1.3f, 0);
			HPText.gameObject.transform.position = HPslider.transform.position + new Vector3 (1f, -.5f, 0);
			HPslider.value = stats.HP;
			HPslider.maxValue = stats.maxHP;
			HPText.GetComponent<Text> ().text = stats.HP.ToString () + " HP";

			MPslider.transform.position = target.transform.position + new Vector3 (0, 1f, 0);
			MPText.gameObject.transform.position = MPslider.transform.position + new Vector3 (1f, -.5f, 0);
			MPslider.value = stats.MP;
			MPslider.maxValue = stats.maxMP;
			MPText.GetComponent<Text> ().text = stats.MP.ToString () + " MP";

		} else  {
			Destroy (this.gameObject);

		}

	}

	public void SetTarget(GameObject givenTarget)
	{
		target = givenTarget;
		stats = target.GetComponent<CharacterStats> ();

	}

	public void ToggleBar(bool toggled){
		gameObject.SetActive (toggled);
	}
}
