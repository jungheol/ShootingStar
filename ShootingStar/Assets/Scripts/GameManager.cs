using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

	public string[] enemyObjects;
	public Transform[] spawnPoints;

	public float maxSpawnDelay;
	public float curSpawnDelay;

	public GameObject player;
	public Text scoreText;
	public Image[] lifeImage;
	public Image[] boomImage;
	public GameObject gameoverPanel;
	public PoolManager poolManager;

	private void Awake() {
		enemyObjects = new string[] { "EnemyA", "EnemyB", "EnemyC" };
	}

	private void Update() {
		curSpawnDelay += Time.deltaTime;

		if (curSpawnDelay > maxSpawnDelay) {
			SpawnEnemy();
			maxSpawnDelay = Random.Range(0.5f, 2.5f);
			curSpawnDelay = 0;
		}

		Player playerLogic = player.GetComponent<Player>();
		scoreText.text = string.Format("{0:n0}", playerLogic.score);
	}

	private void SpawnEnemy() {
		int ranEnemy = Random.Range(0, 3);
		int ranPoint = Random.Range(0, 9);
		GameObject enemy = poolManager.MakeObj(enemyObjects[ranEnemy]);
		enemy.transform.position = spawnPoints[ranPoint].position;
		
		Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
		Enemy enemyLogic = enemy.GetComponent<Enemy>();
		enemyLogic.player = player;
		enemyLogic.poolManager = poolManager;
		
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

	public void UpdateLifeIcon(int life) {
		for (int i = 0; i < 3; i++) {
			lifeImage[i].color = new Color(1, 1, 1, 0);
		}
		
		for (int i = 0; i < life; i++) {
			lifeImage[i].color = new Color(1, 1, 1, 1);
		}
	}
	
	public void UpdateBoomIcon(int boom) {
		for (int i = 0; i < 3; i++) {
			boomImage[i].color = new Color(1, 1, 1, 0);
		}
		
		for (int i = 0; i < boom; i++) {
			boomImage[i].color = new Color(1, 1, 1, 1);
		}
	}

	public void GameOver() {
		gameoverPanel.SetActive(true);
		Time.timeScale = 0;
	}

	public void RetryGame() {
		SceneManager.LoadScene(0);
		Time.timeScale = 1;
	}
	
	public void RespawnPlayer() {
		Invoke("RespawnPlayerExe", 2f);
	}

	private void RespawnPlayerExe() {
		player.transform.position = Vector3.down * 3.75f;
		player.SetActive(true);
	}
}
