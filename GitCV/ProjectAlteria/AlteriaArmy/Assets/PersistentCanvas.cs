using UnityEngine;
using System.Collections;

public class PersistentCanvas : MonoBehaviour {

	public static PersistentCanvas persistentBattleCanvas;
	public static PersistentCanvas persistentMenuCanvas;
	public static PersistentCanvas persistentLoadingCanvas;

	// Use this for initialization
	void Start () {
		if (persistentBattleCanvas == null) {
			DontDestroyOnLoad (gameObject);
			if(gameObject.name == "Battle_UI_Canvas") persistentBattleCanvas = this;
			else if(gameObject.name == "Main_UI_Canvas") persistentMenuCanvas = this;
			else if(gameObject.name == "LoadingCanvas") persistentLoadingCanvas = this;

		} else if (persistentBattleCanvas != this || persistentMenuCanvas != this  || persistentLoadingCanvas != this) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
