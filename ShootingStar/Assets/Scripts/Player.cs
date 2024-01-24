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
	public GameObject[] followers;
	public GameManager manager;
	public PoolManager poolManager;
	public bool isBoom;
	public bool[] joyControl;
	public bool isClick;

	private Animator anim;
	private bool isRespawn;
	private SpriteRenderer spriteRenderer;

	private void Awake() {
		anim = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void OnEnable() {
		Unbeatable();
		Invoke("Unbeatable", 2f);
	}

	private void Unbeatable() {
		isRespawn = !isRespawn;
		
		if (isRespawn) {
			spriteRenderer.color = new Color(1, 1, 1, 0.5f);
			for (int i = 0; i < followers.Length; i++) {
				followers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
			}
		} else {
			spriteRenderer.color = new Color(1, 1, 1, 1);
			for (int i = 0; i < followers.Length; i++) {
				followers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
			}
		}
		
	}

	private void Update() {
		PlayerMove();
		Fire();
		Boom();
		Reload();
	}

	public void JoyPad(int type) {
		for (int i = 0; i < joyControl.Length; i++) {
			joyControl[i] = i == type;
		}
	}

	public void JoyDown() {
		isClick = true;
	}
	
	public void JoyUp() {
		isClick = false;
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
				SpawnBullet(Vector3.zero, "BulletPlayerA");
				break;

			case 1:
				SpawnBullet(Vector3.right * 0.1f, "BulletPlayerA");
				SpawnBullet(Vector3.left * 0.1f, "BulletPlayerA");
				break;

			default:
				SpawnBullet(Vector3.right * 0.3f, "BulletPlayerA");
				SpawnBullet(Vector3.zero, "BulletPlayerB");
				SpawnBullet(Vector3.left * 0.3f, "BulletPlayerA");
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
					
		GameObject[] enemiesA = poolManager.GetPool("EnemyA");
		GameObject[] enemiesB = poolManager.GetPool("EnemyB");
		GameObject[] enemiesC = poolManager.GetPool("EnemyC");
		for (int i = 0; i < enemiesA.Length; i++) {
			if (enemiesA[i].activeSelf) {
				Enemy enemyLogic = enemiesA[i].GetComponent<Enemy>();
				enemyLogic.OnHit(500);
			}
		}
		for (int i = 0; i < enemiesB.Length; i++) {
			if (enemiesB[i].activeSelf) {
				Enemy enemyLogic = enemiesB[i].GetComponent<Enemy>();
				enemyLogic.OnHit(500);
			}
		}
		for (int i = 0; i < enemiesC.Length; i++) {
			if (enemiesC[i].activeSelf) {
				Enemy enemyLogic = enemiesC[i].GetComponent<Enemy>();
				enemyLogic.OnHit(500);
			}
		}

		GameObject[] bulletsA = poolManager.GetPool("BulletEnemyA");
		GameObject[] bulletsB = poolManager.GetPool("BulletEnemyB");
		for (int i = 0; i < bulletsA.Length; i++) {
			if(bulletsA[i].activeSelf) {
				bulletsA[i].SetActive(false);
			}
		}
		for (int i = 0; i < bulletsB.Length; i++) {
			if(bulletsB[i].activeSelf) {
				bulletsB[i].SetActive(false);
			}
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
			if (isRespawn) return;
			
			life--;
			manager.UpdateLifeIcon(life);
			manager.SetExplosion(transform.position, "Player");

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
					else {
						power++;
						AddFollower();
					}
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

	private void AddFollower() {
		if (power == 3) followers[0].SetActive(true);
		else if (power == 4) followers[1].SetActive(true);
		else if (power == 5) followers[2].SetActive(true);
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
