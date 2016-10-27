using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerEnergy : MonoBehaviour {

	public int maxEnergy;
	Slider energyBar;
	Text energyText;
	public int currentEnergy;

	// Use this for initialization
	void Awake () {
		energyBar = GameObject.Find("EnergyBar").GetComponentInChildren<Slider>();
		energyText = GameObject.Find("EnergyText").GetComponent<Text>();

		maxEnergy = 100;
		currentEnergy = 0;
		energyText.text = currentEnergy.ToString();
	}

	// Update is called once per frame
	void Update () {

	}

	public void useEnergy(int energyUsed)
	{
		currentEnergy = currentEnergy - energyUsed;
		energyBar.value = currentEnergy;
		energyText.text = currentEnergy.ToString();

			if(currentEnergy < 0)
			{
				currentEnergy = 0;
			}

	}

	public void gainEnergy(int energyGained)
	{
		if(currentEnergy < maxEnergy)
		{
		currentEnergy = currentEnergy + energyGained;
		energyBar.value = currentEnergy;
		energyText.text = currentEnergy.ToString();
		}
	}
}
