using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerExp : MonoBehaviour {

	SaveLoadController data;
	UIController ui;

	public int level;
	public int expCap;
	public int exp;
	public static float expCurve;

	Slider expBar;
	Text expText;
	Text levelText;

	// Use this for initialization
	void Start () {
		data = SaveLoadController.saveLoadControl;
		ui = GameObject.Find("GameController").GetComponent<UIController>();

		expBar = GameObject.Find("ExpBar").GetComponentInChildren<Slider>();
		expText = GameObject.Find("ExpText").GetComponent<Text>();
		levelText = GameObject.Find("LevelText").GetComponent<Text>();

		level = 1;
		expCap = 100;
		exp = 0;
		expCurve = 1.5f;

	}
	
	// Update is called once per frame
	void Update () {
		expBar.value = SaveLoadController.saveLoadControl.exp;
		expBar.maxValue = SaveLoadController.saveLoadControl.expCap;
		expText.text = SaveLoadController.saveLoadControl.exp.ToString() + " / " + SaveLoadController.saveLoadControl.expCap.ToString();
		levelText.text = SaveLoadController.saveLoadControl.level.ToString();

		expCap = data.expCap;
		exp = data.exp;
		level = data.level;

	}

	public void GainExp(int expGained)
	{
		data.exp += expGained;
		ui.SetInfoText("Gained " + expGained + "XP!");

		if(data.exp >= data.expCap)
		{
			data.level++;
			data.exp -= data.expCap;
			data.expCap = (int)(data.expCap * expCurve);
			ui.SetInfoText("Level up! Level " + data.level + "!");
		}
	}
}
