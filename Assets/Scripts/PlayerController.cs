using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 5.0f;
	public float runSpeedMultiplier = 3.0f;
	public float mouseSensitivity = 5.0f;	
//	float rotUpDown = 0;
	float posUpDown = 2;
	public float upDownRange = 2; //90.0f;
	public float jumpSpeed = 20.0f;
	public float cameraDistance = -5.0f;
	bool isAttacking = false;

	float verticalVelocity = 0;
	bool lari=false;
	MeshRenderer mrSphere;
	public Animator anim;
	new AudioSource[] audio;

	Vector3 faceVector, sideVector;

	CharacterController characterController;


	public bool IsAttacking
	{
		get { return isAttacking; }
		private set { isAttacking = value; }
	}


	void Start () {
		characterController = GetComponent<CharacterController>();
		mrSphere = GetComponentInChildren<MeshRenderer> ();
		anim.GetComponent<Animator>();
		audio = GetComponents<AudioSource> ();

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

		posUpDown -= Input.GetAxis("Mouse Y") * 0.03f;
		posUpDown = Mathf.Clamp(posUpDown, 0.5f , 3);
		Camera.main.transform.localPosition = new Vector3(0, posUpDown, -5.0f);
//		Debug.Log(posUpDown);


		#endregion

		#region movement

		//movement
		faceVector = transform.TransformDirection(Vector3.forward);
		sideVector = transform.TransformDirection (Vector3.right);

		float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis ("Horizontal") * movementSpeed;

		//animation blend tree
		anim.SetFloat("inputV", forwardSpeed);
		anim.SetFloat("inputH", sideSpeed);

		if (lari == false && Input.GetKeyDown (KeyCode.LeftShift)) {
			movementSpeed *= runSpeedMultiplier;
			mrSphere.material.color = Color.red;
			lari = true;
		}
		if(lari == true && Input.GetKeyUp(KeyCode.LeftShift))
		{
			movementSpeed /= runSpeedMultiplier;
			mrSphere.material.color = new Color (0.800f, 0.501f, 0.267f, 1.000f);
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

		// characterController.SimpleMove (speed);
		if(characterController.Move(speed * Time.deltaTime) == CollisionFlags.CollidedBelow)
		{
			verticalVelocity = 0;
		}

//		if(characterController.isGrounded && Input.GetButtonDown("Jump"))
		if(Input.GetButtonDown("Jump"))
		{
			verticalVelocity = jumpSpeed;
		}
			
		#endregion

//		if (Input.GetKey (KeyCode.Space)) {
//			transform.position = Vector3.zero;
//		}

		if (Input.GetMouseButtonUp (0)) {
			anim.SetBool ("attack", true);


			int n = Random.Range (0, 3);
			audio[n].Play ();

			IsAttacking = true;
		}
		else 
		{
			anim.SetBool ("attack", false);
			IsAttacking = false;
		}

	}



}
