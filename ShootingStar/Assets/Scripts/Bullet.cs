using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public int dmg;
	public bool isRotate;

	private void Update() {
		if(isRotate) transform.Rotate(Vector3.forward * 10);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("BorderBullet")) {
			gameObject.SetActive(false);
		}
	}
}
