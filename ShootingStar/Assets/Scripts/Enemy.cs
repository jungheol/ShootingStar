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
	public GameManager gameManager;
	public PoolManager poolManager;

	private SpriteRenderer spriteRenderer;
	private Animator anim;

	private int patternIndex = -1;
	private int curPatternCount;
	public int[] maxPatternCount;

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
			case "Boss":
				health = 3000;
				Invoke("Stop", 2f);
				break;
		}
	}

	private void Stop() {
		if(!gameObject.activeSelf)
			return;
		
		Rigidbody2D rigid = GetComponent<Rigidbody2D>();
		rigid.velocity = Vector2.zero;
		
		Invoke("BossPattern", 2f);
	}

	private void BossPattern() {
		patternIndex = patternIndex == 3 ? 0 : patternIndex+1;
		curPatternCount = 0;

		switch (patternIndex) {
			case 0:
				FirePattern1();
				break;
			case 1:
				FirePattern2();
				break;
			case 2:
				FirePattern3();
				break;
			case 3:
				FirePattern4();
				break;
		}
	}

	private void FirePattern1() {
		GameObject bulletR = poolManager.MakeObj("BulletBossA");
		bulletR.transform.position = transform.position + Vector3.right * 0.5f;
		GameObject bulletRR = poolManager.MakeObj("BulletBossA");
		bulletRR.transform.position = transform.position + Vector3.right * 0.8f;
		GameObject bulletL = poolManager.MakeObj("BulletBossA");
		bulletL.transform.position = transform.position + Vector3.left * 0.5f;
		GameObject bulletLL = poolManager.MakeObj("BulletBossA");
		bulletLL.transform.position = transform.position + Vector3.left * 0.8f;
			
		Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
		Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
		Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
		Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
		
		rigidR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
		rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
		rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
		rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
		
		curPatternCount++;
		
		if (curPatternCount < maxPatternCount[patternIndex]) Invoke("FirePattern1", 2f);
		else Invoke("BossPattern", 3f);
	}

	private void FirePattern2() {
		for (int i = 0; i < 4; i++) {
			GameObject bullet = poolManager.MakeObj("BulletEnemyB");
			bullet.transform.position = transform.position;
			
			Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
			Vector2 dirVec = player.transform.position - transform.position;
			Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
			dirVec += ranVec;
			rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
		}
		
		curPatternCount++;
		
		if (curPatternCount < maxPatternCount[patternIndex]) Invoke("FirePattern2", 3f);
		else Invoke("BossPattern", 3f);
	}

	private void FirePattern3() {
		GameObject bullet = poolManager.MakeObj("BulletEnemyA");
		bullet.transform.position = transform.position;
		bullet.transform.rotation = Quaternion.identity;

		Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
		Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 10 * curPatternCount/maxPatternCount[patternIndex]), -1);
		rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
		
		curPatternCount++;
		
		if (curPatternCount < maxPatternCount[patternIndex]) Invoke("FirePattern3", 0.15f);
		else Invoke("BossPattern", 3f);
	}

	private void FirePattern4() {
		int roundNumA = 50;
		int roundNumB = 40;
		int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;
		for (int i = 0; i < roundNum; i++) {
			GameObject bullet = poolManager.MakeObj("BulletBossB");
			bullet.transform.position = transform.position;
			bullet.transform.rotation = Quaternion.identity;

			Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
			Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));
			rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);
			
			Vector3 rotVec = Vector3.forward * 360 * i / roundNum + Vector3.forward * 90;
			bullet.transform.Rotate(rotVec);
		}
		
		curPatternCount++;
		
		if (curPatternCount < maxPatternCount[patternIndex]) Invoke("FirePattern4", 0.7f);
		else Invoke("BossPattern", 3f);
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
			gameManager.SetExplosion(transform.position, enemyName);
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
