using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

	public string[] enemyObjects;
	public Transform[] spawnPoints;

	public float nextSpawnDelay;
	public float curSpawnDelay;

	public GameObject player;
	public Text scoreText;
	public Image[] lifeImage;
	public Image[] boomImage;
	public GameObject gameoverPanel;
	public PoolManager poolManager;

	public List<Spawn> spawnList;
	public int spawnIndex;
	public bool isSpawnFinish;

	public Animator startAnim;
	public Animator clearAnim;
	public Animator fadeAnim;
	private int stage = 1;

	private void Awake() {
		spawnList = new List<Spawn>();
		enemyObjects = new string[] { "EnemyA", "EnemyB", "EnemyC", "EnemyD" };
		StageStart();
	}

	public void StageStart() {
		startAnim.SetTrigger("TextOn");
		startAnim.GetComponent<Text>().text = "Stage " + stage + "\nStart!";
		clearAnim.GetComponent<Text>().text = "Stage " + stage + "\nClear!!";
		
		ReadSpawnFile();
		
		fadeAnim.SetTrigger("FadeIn");
	}

	public void StageEnd() {
		clearAnim.SetTrigger("TextOn");

		stage++;
		
		fadeAnim.SetTrigger("FadeOut");

	}

	private void ReadSpawnFile() {
		// 초기화
		spawnList.Clear();
		spawnIndex = 0;
		isSpawnFinish = false;
		// 파일 읽기
		TextAsset textFile = Resources.Load("stage " + stage) as TextAsset;
		StringReader stringReader = new StringReader(textFile.text);

		while (stringReader != null) {
			string line = stringReader.ReadLine();
			
			if(line == null) break;
			// 데이터 생성 및 리스트에 추가
			Spawn spawnData = new Spawn();
			spawnData.delay = float.Parse(line.Split(',')[0]);
			spawnData.type = line.Split(',')[1];
			spawnData.point = int.Parse(line.Split(',')[2]);
			spawnList.Add(spawnData);
		}
		// 파일 닫기
		stringReader.Close();
		nextSpawnDelay = spawnList[0].delay;
	}

	private void Update() {
		curSpawnDelay += Time.deltaTime;

		if (curSpawnDelay > nextSpawnDelay && !isSpawnFinish) {
			SpawnEnemy();
			curSpawnDelay = 0;
		}

		Player playerLogic = player.GetComponent<Player>();
		scoreText.text = string.Format("{0:n0}", playerLogic.score);
	}

	private void SpawnEnemy() {
		int enemyIndex = 0;
		switch (spawnList[spawnIndex].type) {
			case "A":
				enemyIndex = 0;
				break;
			case "B":
				enemyIndex = 1;
				break;
			case "C":
				enemyIndex = 2;
				break;
			case "D":
				enemyIndex = 3;
				break;
		}
		
		int enemyPoint = spawnList[spawnIndex].point;
		GameObject enemy = poolManager.MakeObj(enemyObjects[enemyIndex]);
		enemy.transform.position = spawnPoints[enemyPoint].position;
		
		Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
		Enemy enemyLogic = enemy.GetComponent<Enemy>();
		enemyLogic.player = player;
		enemyLogic.gameManager = this;
		enemyLogic.poolManager = poolManager;
		
		if (enemyPoint == 5 || enemyPoint == 6) {
			enemy.transform.Rotate(Vector3.back * 90);
			rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
		} else if (enemyPoint == 7 || enemyPoint == 8) {
			rigid.velocity = new Vector2(enemyLogic.speed, -1);
			enemy.transform.Rotate(Vector3.forward * 90);
		} else {
			rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
		}

		spawnIndex++;
		if (spawnIndex == spawnList.Count) {
			isSpawnFinish = true;
			return;
		}

		nextSpawnDelay = spawnList[spawnIndex].delay;
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

	public void SetExplosion(Vector3 pos, string type) {
		GameObject explosion = poolManager.MakeObj("Explosion");
		Explosion explosionLogic = explosion.GetComponent<Explosion>();

		explosion.transform.position = pos;
		explosionLogic.StartExplosion(type);
	}
	
	public void RespawnPlayer() {
		Invoke("RespawnPlayerExe", 2f);
	}

	private void RespawnPlayerExe() {
		player.transform.position = Vector3.down * 3.75f;
		player.SetActive(true);
	}
}
