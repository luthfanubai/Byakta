using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController2 : LivingObject
{
	public float runSpeedMultiplier = 3.0f;
	public float mouseSensitivity = 5.0f;
	float posUpDown = 2;
	public float upDownRange = 2;
	public float jumpSpeed = 20.0f;
	public float cameraDistance = -5.0f;
	bool isAttacking = false;
	bool isJumping = false;
	bool isRunning = false;
	bool isDead = false;

	public Vector3 centerPos
	{
		get { return transform.position; }// add something
	}

	float verticalVelocity = 0;

	public Animator anim;
	new AudioSource[] audio;

	Vector3 faceVector, sideVector;

	CharacterController characterController;

	Slider sliderHP;

	public bool IsRunning
	{
		get { return isRunning; }
		private set { isRunning = value; }
	}
	public bool IsJumping
	{
		get { return isJumping; }
		private set { isJumping = value; }
	}

	public bool IsAttacking
	{
		get { return isAttacking; }
		private set { isAttacking = value; anim.SetBool ("attack", isAttacking);}
	}

	public bool IsDead
	{
		get { return isDead; }
		private set 
		{ 
			isDead = value; 
			anim.SetBool ("dead", isDead);
		}
	}


	void Start () {
		characterController = GetComponent<CharacterController>();
		anim.GetComponent<Animator>();
		audio = GetComponents<AudioSource> ();

		sliderHP = GameObject.Find ("playerHP").GetComponent<Slider> ();;

		Cursor.lockState = CursorLockMode.Locked;
		//Cursor.visible = false;

	}

	void Update () {

		CheckCamera ();

		#region movement

		//movement
		faceVector = transform.TransformDirection(Vector3.forward);
		sideVector = transform.TransformDirection (Vector3.right);

		float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis ("Horizontal") * movementSpeed;

		//animation blend tree
		anim.SetFloat("inputV", forwardSpeed);
		anim.SetFloat("inputH", sideSpeed);

		if (isRunning == false && Input.GetKeyDown (KeyCode.LeftShift)) {
			movementSpeed *= runSpeedMultiplier;
			IsRunning = true;
			anim.SetBool("run", IsRunning);
		}
		if(isRunning == true && Input.GetKeyUp(KeyCode.LeftShift))
		{
			movementSpeed /= runSpeedMultiplier;
			IsRunning = false;
			anim.SetBool("run", IsRunning);
		}


		verticalVelocity += Physics.gravity.y * Time.deltaTime;



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
		if(characterController.Move(speed * Time.deltaTime) == CollisionFlags.CollidedBelow)// && characterController.isGrounded)
		{
			IsJumping = false;
			anim.SetBool("jump", IsJumping);
			verticalVelocity = 0;


		}

		//		if(characterController.isGrounded && Input.GetButtonDown("Jump"))
		if(Input.GetButtonDown("Jump"))
		{

			IsJumping = true;
			anim.SetBool("jump", IsJumping);
			verticalVelocity = jumpSpeed;
		}

		#endregion

		//		if (Input.GetKey (KeyCode.Space)) {
		//			transform.position = Vector3.zero;
		//		}

		if (Input.GetMouseButtonUp (0)) {
			IsAttacking = true;
			 
			int n = Random.Range (0, 2);
			audio[n].Play ();
		}
		else 
		{
			IsAttacking = false;
		}

		if (transform.position.y <= -20) {
			IsDead = true;
			transform.position = Vector3.zero;
		}
		else 
		{
			IsDead = false;
		}

	}

	void CheckCamera ()
	{
		float horizontalInput = Input.GetAxis ("Mouse X") * mouseSensitivity;
		float verticalInput = Input.GetAxis ("Mouse Y") * mouseSensitivity;

		Camera.main.transform.RotateAround (transform.position, Vector3.up, horizontalInput);
		Camera.main.transform.RotateAround (transform.position, Vector3.right, verticalInput);
	}

	#region implemented abstract members of LivingObject

	protected override void Death ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion
}
