using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

	public ArrayList playerList;
	public GameObject playerCharacter;

	public bool setupReady;

	// Use this for initialization
	void Awake () {
		setupReady = false;
	}

	public void Setup(){
		playerList = new ArrayList ();
		playerCharacter = Resources.Load ("Battlers/Player") as GameObject;

		ArrayList spawnPositions = GameObject.Find ("Grid").GetComponent<GridScript> ().spawnPositions;
		ArrayList scene1 = (ArrayList)spawnPositions [0];

		Debug.Log ("partylist " + GameControl.gameControl.partyList.Count);

		for (int i = 0; i < GameControl.gameControl.partyList.Count; i++) {
			CharacterData character = (CharacterData)GameControl.gameControl.partyList [i];
			SpawnPosition spawnPos = (SpawnPosition)scene1 [i];
			Spawn (spawnPos.XPOS, spawnPos.ZPOS, character.characterIndex); // X, Z, index
		}

		setupReady = true;
	}

	public void Spawn(int givenX, int givenZ, int characterIndex){
		playerCharacter = Instantiate (playerCharacter, new Vector3 (givenX * 2, 0.7f, givenZ * 2), Quaternion.Euler(0,0,0)) as GameObject;
		playerCharacter.GetComponent<CharacterStats> ().SetUpCharacter (characterIndex);

		playerList.Add (playerCharacter);
		playerCharacter.name = "Player " + playerList.Count;
		GameControl.gameControl.ui.CreateHP_MPBar (playerCharacter);
	}
		
}
