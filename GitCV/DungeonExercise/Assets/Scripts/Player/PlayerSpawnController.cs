using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerSpawnController : MonoBehaviour {

	public GameObject player;
	public Transform spawnPoint;

	public float restartDelay;
	public bool respawning;

	PlayerMovement playerMovement;

	float restartTimer;

	// Use this for initialization
	void Start () {
		playerMovement = GetComponent<PlayerMovement>();
		//Spawn();
	}

	void Spawn()
	{
		Instantiate(player, spawnPoint.position, spawnPoint.rotation);
		player.transform.position = spawnPoint.position;
	}
	// Update is called once per frame
	void Update () {
		if (respawning)
		{
			restartTimer += Time.deltaTime;
			if (restartTimer >= restartDelay)
			{
				restartTimer = 0;
				respawning = false;
				SceneManager.LoadScene("Level01", LoadSceneMode.Single);
			}
		}
	}

}
