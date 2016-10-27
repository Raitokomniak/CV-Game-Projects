using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {

	public AccessibleArea AAD;
	public GameObject grid;

	public bool sceneLoaded;
	public int scene;
	public bool dialogueSceneRunning;
	public int battleScene;

	void Awake () {
		sceneLoaded = false;
		scene = 0;

		grid = Resources.Load ("Grid") as GameObject;

		battleScene = 0;
	}

	void Start(){
		
	
	}

	void OnGUI(){
		
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void LoadScene(int index, int battleSceneIndex)
	{
		StartCoroutine (LoadingScene (index, battleSceneIndex));
	}

	public void ReturnToOverworld()
	{
		SceneManager.UnloadScene (SceneManager.GetActiveScene ().buildIndex);
		StartCoroutine (LoadingScene (2, 0));
	}

	public void SetUpBattleScene(int battleSceneIndex)
	{
		scene = 1;
		battleScene = battleSceneIndex;

		//DECONSTRUCT PREVIOUS SCENE
		GameControl.gameControl.ui.battleUICanvas.SetActive (true);
		GameControl.gameControl.ui.battleWorldCanvas.SetActive (true);
		GameControl.gameControl.ui.overWorldWorldCanvas.SetActive (false);
		GameControl.gameControl.ui.menuScreenPanel.SetActive (false);

		//INITIALIZE SCENE
		GameControl.gameControl.playerSpawner.Setup ();
		GameControl.gameControl.enemySpawner.Setup ();
		GameControl.gameControl.environmentSpawner.Setup ();
		GameControl.gameControl.phase.Setup ();
		GameControl.gameControl.ui.SetupBattleScene();
		AAD = grid.GetComponent<AccessibleArea> ();
		AAD.Setup ();
		GameControl.gameControl.turn.Setup ();
	
		sceneLoaded = true;
	}

	public void SetUpOverworld(){
		GameControl.gameControl.items.AddToInventory ((Item)GameControl.gameControl.items.allItems [0], false);
		GameControl.gameControl.items.AddToInventory ((Item)GameControl.gameControl.items.allItems [1], false);
		GameControl.gameControl.items.AddToInventory ((Item)GameControl.gameControl.items.allItems [2], false);

		sceneLoaded = false;

		//DECONSTRUCT PREVIOUS SCENE
		GameControl.gameControl.ui.battleUICanvas.SetActive (false);
		GameControl.gameControl.ui.battleWorldCanvas.SetActive (false);
		GameControl.gameControl.ui.overWorldWorldCanvas.SetActive(true);
		GameControl.gameControl.ui.toggleLoadingScreen (false);

		//INITIALIZE SCENE
		GameControl.gameControl.ui.SetupOverWorldWorldCanvas ();
		GameControl.gameControl.dialogue.CheckForDialogue ();

		scene = 2;
		sceneLoaded = true;
	}

	IEnumerator LoadingScene(int sceneIndex, int battleSceneIndex)
	{
		sceneLoaded = false;
		AsyncOperation async = new AsyncOperation ();

		switch(sceneIndex)
		{
		case 1:
			async = SceneManager.LoadSceneAsync ("BattleScene_01");
			while (!async.isDone) {
				yield return null;
			}

			break;
		case 2:
			async = SceneManager.LoadSceneAsync ("Overworld");
			while (!async.isDone) yield return null;
			SetUpOverworld ();
			//ui.toggleLoadingScreen (true);
			break;
		}
		scene = sceneIndex;
		if (scene == 1) {
			SetUpBattleScene (battleSceneIndex);

		}
	}
}
