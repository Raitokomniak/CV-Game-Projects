using UnityEngine;
using System.Collections;


public class SpawnPosition {
	public int XPOS;
	public int ZPOS;
}

public class GridScript : MonoBehaviour {

	public Transform CellPrefab;
	public Vector3 Size;
	public int cellX;
	public int cellZ;

	public ArrayList spawnPositions;

	// Use this for initialization
	void Awake () {
		cellZ = 0;

		switch (GameControl.gameControl.scene.battleScene) {
		case 1:
			Size = new Vector3 (15, 1, 15);
			break;
		case 2:
			Size = new Vector3 (10, 1, 10);
			break;
		}
		CreateGrid ();
		CreateSpawnPositions ();

	}

	void CreateGrid(){
		for (int x = 0; x < Size.x; x++) {
			cellX = x;
			for (int z = 0; z < Size.z; z++) {
				Instantiate (CellPrefab, new Vector3 (x * 2, 0, z * 2), Quaternion.Euler (90, 0, 0));
				cellZ++;
				if (cellZ == Size.z) {
					cellZ = 0;
				}
			}

		}
	}

	void CreateSpawnPositions(){
		spawnPositions = new ArrayList ();

		ArrayList scene_1 = new ArrayList ();
		SpawnPosition position = new SpawnPosition ();
		position.XPOS = 3;
		position.ZPOS = 3;

		scene_1.Add (position);

		SpawnPosition position2 = new SpawnPosition ();

		position2.XPOS = 4;
		position2.ZPOS = 2;

		scene_1.Add (position2);

		spawnPositions.Add (scene_1);
	}
}
