using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

	public float speed;
	
	private void Update() {
		Vector3 curPos = transform.position;
		Vector3 nextPos = Vector3.down * speed * Time.deltaTime;
		transform.position = curPos + nextPos;
	}
}
