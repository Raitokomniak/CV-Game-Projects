using UnityEngine;
using System.Collections;

public class ProgressionHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CheckLevelUpRewards(int level, CharacterStats stats){
		if (level == 2) {
			Debug.Log ("unlocked skill " + GameControl.gameControl.skillLib.fireBlast.name);
			stats.skillSet [0] = GameControl.gameControl.skillLib.fireBlast;
		}
		else if (level == 3) {
			Debug.Log ("unlocked skill " + GameControl.gameControl.skillLib.iceStrike.name);
			stats.skillSet [2] = GameControl.gameControl.skillLib.iceStrike;
		}
	}
}
