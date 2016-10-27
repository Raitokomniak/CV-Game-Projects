using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPSliderHandler : MonoBehaviour {

	public GameObject target;
	Slider HPslider;

	CharacterStats stats;
	Transform HPText;


	// Use this for initialization
	void Awake() {
		stats = GetComponent<CharacterStats> ();

		HPslider = GetComponent<Slider>();
		HPText = gameObject.transform.GetChild (0);
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
