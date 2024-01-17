using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

	public GameObject enemyAPrefab;
	public GameObject enemyBPrefab;
	public GameObject enemyCPrefab;
	public GameObject itemCoinPrefab;
	public GameObject itemPowerPrefab;
	public GameObject itemBoomPrefab;
	public GameObject bulletPlayerAPrefab;
	public GameObject bulletPlayerBPrefab;
	public GameObject bulletEnemyAPrefab;
	public GameObject bulletEnemyBPrefab;
	public GameObject bulletFollowerPrefab;
	
	private GameObject[] enemyA;
	private GameObject[] enemyB;
	private GameObject[] enemyC;
	
	private GameObject[] itemCoin;
	private GameObject[] itemPower;
	private GameObject[] itemBoom;

	private GameObject[] bulletPlayerA;
	private GameObject[] bulletPlayerB;
	private GameObject[] bulletEnemyA;
	private GameObject[] bulletEnemyB;
	private GameObject[] bulletFollower;

	private GameObject[] targetPool;

	private void Awake() {
		enemyA = new GameObject[10];
		enemyB = new GameObject[10];
		enemyC = new GameObject[10];

		itemCoin = new GameObject[20];
		itemPower = new GameObject[20];
		itemBoom = new GameObject[20];

		bulletPlayerA = new GameObject[100];
		bulletPlayerB = new GameObject[100];
		bulletEnemyA = new GameObject[100];
		bulletEnemyB = new GameObject[100];
		bulletFollower = new GameObject[100];
		
		Generate();
	}

	private void Generate() {
		for (int i = 0; i < enemyA.Length; i++) {
			enemyA[i] = Instantiate(enemyAPrefab);
			enemyA[i].SetActive(false);
		}
		for (int i = 0; i < enemyB.Length; i++) {
			enemyB[i] = Instantiate(enemyBPrefab);
			enemyB[i].SetActive(false);
		}
		for (int i = 0; i < enemyC.Length; i++) {
			enemyC[i] = Instantiate(enemyCPrefab);
			enemyC[i].SetActive(false);
		}
		for (int i = 0; i < itemCoin.Length; i++) {
			itemCoin[i] = Instantiate(itemCoinPrefab);
			itemCoin[i].SetActive(false);
		}
		for (int i = 0; i < itemPower.Length; i++) {
			itemPower[i] = Instantiate(itemPowerPrefab);
			itemPower[i].SetActive(false);
		}
		for (int i = 0; i < itemBoom.Length; i++) {
			itemBoom[i] = Instantiate(itemBoomPrefab);
			itemBoom[i].SetActive(false);
		}
		for (int i = 0; i < bulletPlayerA.Length; i++) {
			bulletPlayerA[i] = Instantiate(bulletPlayerAPrefab);
			bulletPlayerA[i].SetActive(false);
		}
		for (int i = 0; i < bulletPlayerB.Length; i++) {
			bulletPlayerB[i] = Instantiate(bulletPlayerBPrefab);
			bulletPlayerB[i].SetActive(false);
		}
		for (int i = 0; i < bulletEnemyA.Length; i++) {
			bulletEnemyA[i] = Instantiate(bulletEnemyAPrefab);
			bulletEnemyA[i].SetActive(false);
		}
		for (int i = 0; i < bulletEnemyB.Length; i++) {
			bulletEnemyB[i] = Instantiate(bulletEnemyBPrefab);
			bulletEnemyB[i].SetActive(false);
		}
		for (int i = 0; i < bulletFollower.Length; i++) {
			bulletFollower[i] = Instantiate(bulletFollowerPrefab);
			bulletFollower[i].SetActive(false);
		}
	}

	public GameObject MakeObj(string type) {
		switch (type) {
			case "EnemyA":
				targetPool = enemyA;
				break;
			case "EnemyB":
				targetPool = enemyB;
				break;
			case "EnemyC":
				targetPool = enemyC;
				break;
			case "ItemCoin":
				targetPool = itemCoin;
				break;
			case "ItemPower":
				targetPool = itemPower;
				break;
			case "ItemBoom":
				targetPool = itemBoom;
				break;
			case "BulletPlayerA":
				targetPool = bulletPlayerA;
				break;
			case "BulletPlayerB":
				targetPool = bulletPlayerB;
				break;
			case "BulletEnemyA":
				targetPool = bulletEnemyA;
				break;
			case "BulletEnemyB":
				targetPool = bulletEnemyB;
				break;
			case "BulletFollower":
				targetPool = bulletFollower;
				break;
		}

		for (int i = 0; i < targetPool.Length; i++) {
			if (!targetPool[i].activeSelf) {
				targetPool[i].SetActive(true);
				return targetPool[i];
			}
		}

		return null;
	}

	public GameObject[] GetPool(string type) {
		switch (type) {
			case "EnemyA":
				targetPool = enemyA;
				break;
			case "EnemyB":
				targetPool = enemyB;
				break;
			case "EnemyC":
				targetPool = enemyC;
				break;
			case "ItemCoin":
				targetPool = itemCoin;
				break;
			case "ItemPower":
				targetPool = itemPower;
				break;
			case "ItemBoom":
				targetPool = itemBoom;
				break;
			case "BulletPlayerA":
				targetPool = bulletPlayerA;
				break;
			case "BulletPlayerB":
				targetPool = bulletPlayerB;
				break;
			case "BulletEnemyA":
				targetPool = bulletEnemyA;
				break;
			case "BulletEnemyB":
				targetPool = bulletEnemyB;
				break;
			case "BulletFollower":
				targetPool = bulletFollower;
				break;
		}

		return targetPool;
	}
}
