﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 5.0f;	
	public float mouseSensitivity = 5.0f;	
//	float rotUpDown = 0;
	public float upDownRange = 90.0f;
	public float jumpSpeed = 20.0f;

	float verticalVelocity = 0;
	bool lari=false;

	Vector3 faceVector, sideVector;

	CharacterController characterController;

	void Start () {
		characterController = GetComponent<CharacterController>();

		Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = false;
	}

	void Update () {

		#region rotation
		//rotation
		float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate (0, rotLeftRight, 0);

//		rotUpDown -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
//		rotUpDown = Mathf.Clamp(rotUpDown, -upDownRange, upDownRange);
//		Camera.main.transform.localRotation = Quaternion.Euler(rotUpDown, 0,0);

		#endregion

		#region movement

		//movement
		faceVector = transform.TransformDirection(Vector3.forward);
		sideVector = transform.TransformDirection (Vector3.right);

		float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis ("Horizontal") * movementSpeed;


		if (lari == false && Input.GetKeyDown (KeyCode.LeftShift)) {
			movementSpeed *= 2;
			lari = true;
		}
		if(lari == true && Input.GetKeyUp(KeyCode.LeftShift))
		{
			movementSpeed *= 0.5f;
			lari = false;
		}


		verticalVelocity += Physics.gravity.y * Time.fixedDeltaTime;

		if(!characterController.isGrounded)
		{
			forwardSpeed = forwardSpeed * 0.5f;
			sideSpeed = sideSpeed * 0.5f;
		}

		//Vector3 speed = new Vector3 (sideSpeed, Physics.gravity.y, forwardSpeed);
		//Vector3 speed = new Vector3 (forwardSpeed, verticalVelocity,  sideSpeed);
		Vector3 speed = faceVector * forwardSpeed + sideVector * sideSpeed;
		speed.y = verticalVelocity;
		//Debug.Log(verticalVelocity);

		// characterController.SimpleMove (speed);
		if(characterController.Move(speed * Time.deltaTime) == CollisionFlags.CollidedBelow)
		{
			verticalVelocity = 0;
		}

		if(characterController.isGrounded && Input.GetButtonDown("Jump"))
		{
			verticalVelocity = jumpSpeed;
		}
			
		#endregion

//		if (Input.GetKey (KeyCode.Space)) {
//			transform.position = Vector3.zero;
//		}

	}


	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		//GameObject.FindWithTag("Enemy");
		if (GameObject.FindWithTag ("Enemy") == hit.gameObject) {
			hit.gameObject.GetComponent<EnemyController> ().HitPoint--;
		}
		//Destroy (col.gameObject);
	}

}