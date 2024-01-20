using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {

	enum SpriteName {
		Normal = 0,
		Hit
	}

	public string enemyName;
	public int enemyScore;
	public float speed;
	public int health;
	public float maxShotDelay;
	public float curShotDelay;
	public Sprite[] sprites;
	public GameObject player;
	public PoolManager poolManager;

	private SpriteRenderer spriteRenderer;
	private Animator anim;


	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		if (enemyName == "Boss") anim = GetComponent<Animator>();
	}

	private void OnEnable() {
		switch (enemyName) {
			case "enemyA":
				health = 3;
				Debug.Log("enemyA");
				break;
			case "enemyB":
				health = 15;
				Debug.Log("enemyB");
				break;
			case "enemyC":
				health = 50;
				Debug.Log("enemyC");
				break;
		}
	}

	private void Update() {
		if(enemyName == "Boss") return;
		
		Fire();
		Reload();
	}

	private void Fire() {
		if (curShotDelay < maxShotDelay) return;

		if (enemyName == "enemyA") {
			GameObject bullet = poolManager.MakeObj("BulletEnemyA");
			bullet.transform.position = transform.position;
			Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
			Vector3 dirVec = player.transform.position - transform.position;
			rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
			
		} else if (enemyName == "enemyC") {
			GameObject bulletR = poolManager.MakeObj("BulletEnemyB");
			bulletR.transform.position = transform.position;
			GameObject bulletL = poolManager.MakeObj("BulletEnemyB");
			bulletL.transform.position = transform.position;
			
			Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
			Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
			
			Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
			Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);
			
			rigidR.AddForce(dirVecR.normalized * 4, ForceMode2D.Impulse);
			rigidL.AddForce(dirVecL.normalized * 4, ForceMode2D.Impulse);
		}

		curShotDelay = 0;
	}
	
	private void Reload() {
		curShotDelay += Time.deltaTime;
	}

	public void OnHit(int dmg) {
		if(health <= 0) return;
		
		health -= dmg;
		if (enemyName == "Boss") {
			anim.SetTrigger("OnHit");
		} else {
			spriteRenderer.sprite = sprites[(int)SpriteName.Hit];
			Invoke("ReturnSprites", 0.1f);
		}
		
		if (health <= 0) {
			Player playerLogic = player.GetComponent<Player>();
			playerLogic.score += enemyScore;

			int ran = enemyName == "Boss" ? 0 : Random.Range(0, 10);
			if (ran < 5) {
				
			} else if (ran < 8) {
				GameObject itemCoin = poolManager.MakeObj("ItemCoin");
				itemCoin.transform.position = transform.position;
			} else if (ran < 9) {
				GameObject itemPower = poolManager.MakeObj("ItemPower");
				itemPower.transform.position = transform.position;
			} else if (ran < 10) {
				GameObject itemBoom = poolManager.MakeObj("ItemBoom");
				itemBoom.transform.position = transform.position;
			}
			gameObject.SetActive(false);
			transform.rotation = Quaternion.identity;
		}
	}

	private void ReturnSprites() {
		spriteRenderer.sprite = sprites[(int)SpriteName.Normal];
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("BorderBullet") && enemyName != "Boss") {
			gameObject.SetActive(false);
			transform.rotation = Quaternion.identity;
		} else if (other.CompareTag("PlayerBullet")) {
			Bullet bullet = other.gameObject.GetComponent<Bullet>();
			OnHit(bullet.dmg);
			other.gameObject.SetActive(false);
		}
	}
}
