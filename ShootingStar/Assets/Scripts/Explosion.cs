using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	private Animator anim;

	private void Awake() {
		anim = GetComponent<Animator>();
	}

	public void StartExplosion(string target) {
		anim.SetTrigger("OnExp");

		switch (target) {
			case "Player":
			case "EnemyB":
				transform.localScale = Vector3.one * 1f;
				break;
			case "EnemyA":
				transform.localScale = Vector3.one * 0.7f;
				break;
			case "EnemyC":
				transform.localScale = Vector3.one * 2f;
				break;
			case "EnemyD":
				transform.localScale = Vector3.one * 3f;
				break;
		}
	}
}
