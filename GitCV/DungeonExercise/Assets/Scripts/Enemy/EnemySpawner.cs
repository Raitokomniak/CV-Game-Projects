using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject[] enemyInstances;
	public GameObject[] enemyUIInstances;

	public GameObject[] bossInstances;
	public GameObject[] bossUIInstances;

	int i; //instance array counter



	void Start () {
		i = 0;

		enemyInstances = new GameObject[3];
		enemyUIInstances = new GameObject[3];

		bossInstances = new GameObject[1];
		bossUIInstances = new GameObject[1];

		//SPAWN SKELETONS
		SpawnSkeletons();
		//SpawnBoss();
	}
	

	void Update () {
	
	}



	void SpawnSkeletons()
	{
		for (i = 0; i < enemyInstances.Length; i++)
		{
			string spawnpointname = "EnemySpawnPoint" + i; 					//Find spawn point
			GameObject enemySpawnPoint = GameObject.Find(spawnpointname);	//by name

			if (i == 2)
			{
				//Spawn instances of enemies and their UIs to spawnpoint location
				enemyInstances[i] = Instantiate(Resources.Load("Horse"), enemySpawnPoint.transform.position, enemySpawnPoint.transform.rotation) as GameObject;
				enemyInstances[i].GetComponent<EnemyMovement>().isNeutral = true;
			}
			else
			{
			//Spawn instances of enemies and their UIs to spawnpoint location
			enemyInstances[i] = Instantiate(Resources.Load("Enemy_Skeleton"), enemySpawnPoint.transform.position, enemySpawnPoint.transform.rotation) as GameObject;
			}


			enemyUIInstances[i] = Instantiate(Resources.Load("EnemyUI_instance")) as GameObject;


			//Change UI name to describe instanceID
			enemyUIInstances[i].name = enemyUIInstances[i].name + i;


			//Change UI location to zero
			enemyUIInstances[i].transform.SetParent(GameObject.Find("EnemyUI").transform);
			enemyUIInstances[i].transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
			enemyUIInstances[i].transform.localScale = new Vector3(1f, 1f, 1f);

			//Set UI's instance id
			enemyInstances[i].GetComponent<enemyUI>().instanceID = i;


		}
			
	}

	void SpawnBoss()
	{
		string spawnpointname = "BossSpawnPoint"; 					//Find spawn point
		GameObject bossSpawnPoint = GameObject.Find(spawnpointname);	//by name

		bossInstances[0] = Instantiate(Resources.Load("Boss_Skeleton"), bossSpawnPoint.transform.position, bossSpawnPoint.transform.rotation) as GameObject;
		//bossUIInstances[0] = Instantiate(Resources.Load("EnemyUI_instance")) as GameObject;

		//bossUIInstances[0].name = bossUIInstances[0].name + 0;

	}
}
