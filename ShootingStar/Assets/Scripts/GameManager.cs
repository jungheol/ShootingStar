using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

	public GameObject[] enemyObjects;
	public Transform[] spawnPoints;

	public float maxSpawnDelay;
	public float curSpawnDelay;

	public GameObject player;

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
		int ranPoint = Random.Range(0, 9);
		GameObject enemy = Instantiate(enemyObjects[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation);

		Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
		Enemy enemyLogic = enemy.GetComponent<Enemy>();
		enemyLogic.player = player;
		
		if (ranPoint == 5 || ranPoint == 6) {
			enemy.transform.Rotate(Vector3.back * 90);
			rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
		} else if (ranPoint == 7 || ranPoint == 8) {
			rigid.velocity = new Vector2(enemyLogic.speed, -1);
			enemy.transform.Rotate(Vector3.forward * 90);
		} else {
			rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
		}
	}
}
