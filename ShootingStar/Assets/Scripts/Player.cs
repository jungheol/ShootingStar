using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float moveSpeed;
	
	private void Update() {
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		Vector3 curPos = transform.position;
		Vector3 nextPos = new Vector3(h, v, 0) * moveSpeed * Time.deltaTime;

		transform.position = curPos + nextPos;
	}
}
