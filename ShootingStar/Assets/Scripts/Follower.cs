using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {
	
	public float maxShotDelay;
	public float curShotDelay;
	public PoolManager poolManager;
	
	
	private void Update() {
		Follow();
		Fire();
		Reload();
	}

	private void Follow() {
		
	}

	private void Fire() {
		if (!Input.GetButton("Fire1")) return;

		if (curShotDelay < maxShotDelay) return;
		SpawnBullet("BulletFollower");
		curShotDelay = 0;
	}

	private void SpawnBullet(string bulletPrefab) {
		GameObject bullet = poolManager.MakeObj(bulletPrefab);
		bullet.transform.position = transform.position;
		Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
		rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
	}

	private void Reload() {
		curShotDelay += Time.deltaTime;
	}
}
