using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float moveSpeed;
	public int power;
	public int maxPower;
	public int boom;
	public int maxBoom;
	public float maxShotDelay;
	public float curShotDelay;

	public int life;
	public int score;
	
	public bool isTouchTop;
	public bool isTouchBottom;
	public bool isTouchLeft;
	public bool isTouchRight;

	public GameObject boomEffect;
	
	public GameManager manager;
	public PoolManager poolManager;
	public bool isBoom;

	private Animator anim;
	private string bulletA;
	private string bulletB;

	private void Awake() {
		anim = GetComponent<Animator>();
		bulletA = "BulletPlayerA";
		bulletB = "BulletPlayerB";
	}

	private void Update() {
		PlayerMove();
		Fire();
		Boom();
		Reload();
	}

	private void PlayerMove() {
		float h = Input.GetAxisRaw("Horizontal");
		if((h == 1 && isTouchRight) || (h == -1 && isTouchLeft)) h = 0;

		float v = Input.GetAxisRaw("Vertical");
		if ((v == 1 && isTouchTop) || (v == -1 && isTouchBottom)) v = 0;
		
		Vector3 curPos = transform.position;
		Vector3 nextPos = new Vector3(h, v, 0) * moveSpeed * Time.deltaTime;

		transform.position = curPos + nextPos;
		
		if((Input.GetButtonDown("Horizontal")) || (Input.GetButtonUp("Horizontal"))) {
			anim.SetInteger("Input", (int)h);
		}
	}

	private void Fire() {
		if (!Input.GetButton("Fire1")) return;

		if (curShotDelay < maxShotDelay) return;
		
		switch (power) {
			case 0:
				SpawnBullet(Vector3.zero, bulletA);
				break;

			case 1:
				SpawnBullet(Vector3.right * 0.1f, bulletA);
				SpawnBullet(Vector3.left * 0.1f, bulletA);
				break;

			case 2:
				SpawnBullet(Vector3.right * 0.3f, bulletA);
				SpawnBullet(Vector3.zero, bulletB);
				SpawnBullet(Vector3.left * 0.3f, bulletA);
				break;
		}

		curShotDelay = 0;
	}

	private void SpawnBullet(Vector3 positionOffset, string bulletPrefab) {
		GameObject bullet = poolManager.MakeObj(bulletPrefab);
		bullet.transform.position = transform.position + positionOffset;
		Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
		rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
	}

	private void Reload() {
		curShotDelay += Time.deltaTime;
	}

	private void Boom() {
		if (!Input.GetButton("Fire2")) return;

		if (isBoom) return;
		
		if (boom == 0) return;

		boom--;
		manager.UpdateBoomIcon(boom);
		isBoom = true;
		boomEffect.SetActive(true);
		Invoke("OffBoomEffect", 3f);
					
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < enemies.Length; i++) {
			Enemy enemyLogic = enemies[i].GetComponent<Enemy>();
			enemyLogic.OnHit(500);
		}
					
		GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
		for (int i = 0; i < bullets.Length; i++) {
			bullets[i].SetActive(false);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Border")) {
			switch (other.gameObject.name) {
				case "Top":
					isTouchTop = true;
					break;
				case "Bottom":
					isTouchBottom = true;
					break;
				case "Left":
					isTouchLeft = true;
					break;
				case "Right":
					isTouchRight = true;
					break;
			}
		} else if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet")) {
			life--;
			manager.UpdateLifeIcon(life);

			if (life == 0) {
				manager.GameOver();
			} else {
				manager.RespawnPlayer();
			}
			
			gameObject.SetActive(false);
		} else if (other.CompareTag("Item")) {
			Item item = other.gameObject.GetComponent<Item>();
			switch (item.type) {
				case "Coin":
					score += 1000;
					break;
				case "Power":
					if (power == maxPower) score += 500;
					else power++;
					break;
				case "Boom":
					if (boom == maxBoom) score += 500;
					else {
						boom++;
						manager.UpdateBoomIcon(boom);
					}
					break;
			}
			other.gameObject.SetActive(false);
		}
	}

	private void OffBoomEffect() {
		boomEffect.SetActive(false);
		isBoom = false;
	}
	
	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Border")) {
			switch (other.gameObject.name) {
				case "Top":
					isTouchTop = false;
					break;
				case "Bottom":
					isTouchBottom = false;
					break;
				case "Left":
					isTouchLeft = false;
					break;
				case "Right":
					isTouchRight = false;
					break;
			}
		}
	}
}
