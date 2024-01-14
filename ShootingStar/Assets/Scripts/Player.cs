using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float moveSpeed;
	public bool isTouchTop;
	public bool isTouchBottom;
	public bool isTouchLeft;
	public bool isTouchRight;

	public GameObject bulletA;
	public GameObject bulletB;

	private Animator anim;

	private void Awake() {
		anim = GetComponent<Animator>();
	}

	private void Update() {
		PlayerMove();
		Fire();
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
		GameObject bullet = Instantiate(bulletA, transform.position, transform.rotation);
		Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
		rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
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
		}
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
