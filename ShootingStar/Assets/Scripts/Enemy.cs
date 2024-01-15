using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	enum SpriteName {
		Normal = 0,
		Hit
	}
	
	public float speed;
	public int health;
	public Sprite[] sprites;

	private SpriteRenderer spriteRenderer;
	private Rigidbody2D rigid;


	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		rigid = GetComponent<Rigidbody2D>();
		rigid.velocity = Vector2.down * speed;
	}

	private void OnHit(int dmg) {
		health -= dmg;
		spriteRenderer.sprite = sprites[(int)SpriteName.Hit];
		Invoke("ReturnSprites", 0.1f);
		
		if (health <= 0) {
			Destroy(gameObject);
		}
	}

	private void ReturnSprites() {
		spriteRenderer.sprite = sprites[(int)SpriteName.Normal];
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("BorderBullet")) Destroy(gameObject);
		else if (other.CompareTag("PlayerBullet")) {
			Bullet bullet = other.gameObject.GetComponent<Bullet>();
			OnHit(bullet.dmg);
			Destroy(other.gameObject);
		}
	}
}
