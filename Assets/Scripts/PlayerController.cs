using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float speed;
	public int maxDelta;
	public Text countText;
	public Text winText;
	public Image diskImage;

	private Rigidbody rb;
	private int count = 0;
	private Nullable<Touch> firstTouch = null;

	void Start()
	{
		rb = GetComponent<Rigidbody> ();
		count = 0;
		SetCountText ();
		winText.text = "";
		diskImage.enabled = false;
	}

	void FixedUpdate() 
	{
		if (Input.touchCount > 0) {
			Touch touchNow = Input.GetTouch (0);
			if (firstTouch == null) {
				firstTouch = touchNow;
			}
			diskImage.transform.position = new Vector3 (firstTouch.GetValueOrDefault().position.x, firstTouch.GetValueOrDefault().position.y, 0);
			if (touchNow.phase == TouchPhase.Began) {
				diskImage.enabled = true;
			}
			if (touchNow.phase == TouchPhase.Stationary || touchNow.phase == TouchPhase.Moved) {
				Vector2 touchDeltaPosition = Input.GetTouch (0).position - firstTouch.GetValueOrDefault ().position;
				if (touchDeltaPosition.x != 0 || touchDeltaPosition.y != 0) {
					Vector3 movement = new Vector3 (Math.Max (-maxDelta, Math.Min (maxDelta, touchDeltaPosition.x / 2)), 0, Math.Max (-maxDelta, Math.Min (maxDelta, touchDeltaPosition.y / 2)));
					rb.AddForce (movement);
				}
			}
			if (touchNow.phase == TouchPhase.Ended) {
				diskImage.enabled = false;
				firstTouch = null;
			}
		} else {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);
			rb.AddForce(movement * speed);
		}
	}

	void OnTriggerEnter(Collider other)
	{
//		Destroy(other.gameObject);
		if (other.gameObject.CompareTag ("Pick Up")) {
			other.gameObject.SetActive (false);
			count = count + 1;
			SetCountText ();
		}
	}

	void SetCountText() {
		countText.text = "Count: " + count.ToString ();
		if (count >= 12) {
			winText.text = "You Win!";
		}
	}
}
