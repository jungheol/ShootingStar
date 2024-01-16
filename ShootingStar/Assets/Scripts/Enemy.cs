using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public GameObject bulletA;
	public GameObject bulletB;
	public GameObject itemCoin;
	public GameObject itemPower;
	public GameObject itemBoom;
	public GameObject player;

	private SpriteRenderer spriteRenderer;


	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update() {
		Fire();
		Reload();
	}

	private void Fire() {
		if (curShotDelay < maxShotDelay) return;

		if (enemyName == "enemyA") {
			GameObject bullet = Instantiate(bulletA, transform.position, transform.rotation);
			Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
			Vector3 dirVec = player.transform.position - transform.position;
			rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
			
		} else if (enemyName == "enemyC") {
			GameObject bulletR = Instantiate(bulletB, transform.position + Vector3.right * 0.3f, transform.rotation);
			GameObject bulletL = Instantiate(bulletB, transform.position + Vector3.left * 0.3f, transform.rotation);
			
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
		spriteRenderer.sprite = sprites[(int)SpriteName.Hit];
		Invoke("ReturnSprites", 0.1f);
		
		if (health <= 0) {
			Player playerLogic = player.GetComponent<Player>();
			playerLogic.score += enemyScore;

			int ran = Random.Range(0, 10);
			if (ran < 5) {
				
			} else if (ran < 8) {
				Instantiate(itemCoin, transform.position, itemCoin.transform.rotation);
			} else if (ran < 9) {
				Instantiate(itemPower, transform.position, itemPower.transform.rotation);
			} else if (ran < 10) {
				Instantiate(itemBoom, transform.position, itemBoom.transform.rotation);
			}
			gameObject.SetActive(false);
		}
	}

	private void ReturnSprites() {
		spriteRenderer.sprite = sprites[(int)SpriteName.Normal];
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("BorderBullet")) gameObject.SetActive(false);
		else if (other.CompareTag("PlayerBullet")) {
			Bullet bullet = other.gameObject.GetComponent<Bullet>();
			OnHit(bullet.dmg);
			other.gameObject.SetActive(false);
		}
	}
}
