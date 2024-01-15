using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

	public GameObject[] enemyObjects;
	public Transform[] spawnPoints;

	public float maxSpawnDelay;
	public float curSpawnDelay;

	private void Update() {
		curSpawnDelay += Time.deltaTime;

		if (curSpawnDelay > maxSpawnDelay) {
			SpawnEnemy();
			maxSpawnDelay = Random.Range(0.5f, 2.5f);
			curSpawnDelay = 0;
		}
	}

	private void SpawnEnemy() {
		int ranEnemy = Random.Range(0, 3);
		int ranPoint = Random.Range(0, 5);
		Instantiate(enemyObjects[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation);
	}
}
