using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public ArrayList enemyList;
	public GameObject enemy;

	public bool setupReady;

	public void Setup(){
		setupReady = false;
		enemyList = new ArrayList ();
		enemy = Resources.Load ("Battlers/Enemy") as GameObject;

		Spawn (2, 5);
		Spawn (4, 6);

		setupReady = true;
	}

	public void Spawn(int givenX, int givenZ){
		enemy = ( Instantiate (enemy, new Vector3 (givenX * 2, 0.7f, givenZ * 2), Quaternion.Euler(0,0,0))) as GameObject;

		enemyList.Add(enemy);
		enemy.name = "Basic Enemy " + enemyList.Count;
		GameControl.gameControl.ui.CreateHPBar (enemy);
	}
}
