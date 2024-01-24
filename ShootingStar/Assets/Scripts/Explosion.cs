using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	private Animator anim;

	private void Awake() {
		anim = GetComponent<Animator>();
	}

	private void OnEnable() {
		Invoke("Disable", 0.5f);
	}

	private void Disable() {
		gameObject.SetActive(false);
	}

	public void StartExplosion(string target) {
		anim.SetTrigger("OnExp");

		switch (target) {
			case "Player":
			case "enemyB":
				transform.localScale = Vector3.one * 1f;
				break;
			case "enemyA":
				transform.localScale = Vector3.one * 0.7f;
				break;
			case "enemyC":
				transform.localScale = Vector3.one * 2f;
				break;
			case "enemyD":
				transform.localScale = Vector3.one * 3f;
				break;
		}
	}
}
