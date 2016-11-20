using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : LivingObject, IRangedAttacker, IMeleeAttacker
{
	[SerializeField]
	Transform focusObject;


	public BulletController bullet;
	public float cameraDistance = -5.0f;
	public float jumpSpeed = 20.0f;
	public float mouseSensitivity = 5.0f;
	public float runSpeedMultiplier = 3.0f;
	public Animator anim;
	float verticalVelocity = 0;

	bool isAttacking = false;
	bool isDead = false;
	bool isJumping = false;
	bool isRunning = false;
	bool isWalking = false;
	bool canDoubleJump;
	int jumpCount = 2;

	float manaPoint;
	float maxManaPoint = 100.0f;
	float manaRegen = 2.5f;
	float hitRegen = 2.5f;
	public float manaCost = 10.0f;

	AudioSource[] attackSound;
	CharacterController characterController;
	Slider sliderHP;
	Slider sliderMP;
	public GameObject notEnoughMana;
	Vector3 faceVector, sideVector;
	Vector3[] respawnPos;

	#region IRangedAttacker implementation

	Transform barrelTip;

	Transform IRangedAttacker.BarrelTip
	{
		get
		{
			return barrelTip;
		}
		set
		{
			barrelTip = value;
		}
	}

	#endregion

	#region Property

	public Transform FocusObject
	{
		get
		{
			return focusObject;
		}
	}

	public override float HitPoint
	{
		get
		{
			return base.HitPoint;
		}
		set
		{
			base.HitPoint = value;
			sliderHP.value = hitPoint;
		}
	}

	public float ManaPoint
	{
		get
		{
			return manaPoint;
		}
		set
		{
			manaPoint = value;
			sliderMP.value = manaPoint;

		}
	}

	public bool IsRunning
	{
		get
		{ 
			return isRunning; 
		}
		private set
		{ 
			isRunning = value; 
			anim.SetBool("run", IsRunning);
		}
	}

	public bool IsWalking
	{
		get
		{ 
			return isWalking; 
		}
		private set
		{ 
			isWalking = value;
		}
	}

	public bool IsJumping
	{
		get
		{ 
			return isJumping; 
		}
		private set
		{ 
			isJumping = value; 
			anim.SetBool("jump", IsJumping);
		}
	}

	public bool IsAttacking
	{
		get
		{ 
			return isAttacking; 
		}
		private set
		{ 
			isAttacking = value; 
		}
	}

	public bool IsDead
	{
		get
		{ 
			return isDead; 
		}
		private set
		{
			isDead = value; 
			anim.SetBool("dead", isDead);
		}
	}

	#endregion

	void Start()
	{
		characterController = GetComponent<CharacterController>();
		anim.GetComponent<Animator>();
		attackSound = GetComponents<AudioSource>();
		sliderHP = GameObject.Find("playerHP").GetComponent<Slider>();
		sliderMP = GameObject.Find("playerMP").GetComponent<Slider>();
//		notEnoughMana = GameObject.Find ("NotEnoughMana");
		//barrelTip = transform.FindChild ("Sphere");
		((IRangedAttacker)this).BarrelTip = transform.FindChild("Barrel Tip");

		ally = Alliance.Player;

		respawnPos = new Vector3[5];
		respawnPos[0] = transform.position;

		HitPoint = maxHitPoint;
		ManaPoint = maxManaPoint;

		InitializeSkill();

		Cursor.lockState = CursorLockMode.Locked; //atau Cursor.visible = false;
	}

	void FixedUpdate()
	{
		ManaPoint += manaRegen * Time.fixedDeltaTime;
		if (ManaPoint > maxManaPoint)
		{
			ManaPoint = maxManaPoint;
		}

		HitPoint += hitRegen * Time.fixedDeltaTime;
		if (HitPoint > maxHitPoint)
		{
			HitPoint = maxHitPoint;
		}
	}

	WrappedCoroutine unknownAction;

	void Update()
	{
		if (Input.anyKey)
		{
			anim.transform.rotation = focusObject.transform.rotation;
		}

//		Debug.Log (notEnoughMana);

		CheckCamera();

		#region movement
		faceVector = focusObject.TransformDirection(Vector3.forward);
		sideVector = focusObject.TransformDirection(Vector3.right);

		float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

		//animation blend tree
		anim.SetFloat("inputV", forwardSpeed);
		anim.SetFloat("inputH", sideSpeed);

		#region Running
		if (isRunning == false && Input.GetKeyDown(KeyCode.LeftShift))
		{
			movementSpeed *= runSpeedMultiplier;
			IsRunning = true;
		}

		if (isRunning == true && Input.GetKeyUp(KeyCode.LeftShift))
		{
			movementSpeed /= runSpeedMultiplier;
			IsRunning = false;
		}



		if (!characterController.isGrounded)
		{
			forwardSpeed = forwardSpeed * 0.5f;
			sideSpeed = sideSpeed * 0.5f;
		}
		#endregion

		//Gravity works here
		verticalVelocity += Physics.gravity.y * Time.deltaTime;

		//legacy code:
		//Vector3 speed = new Vector3 (sideSpeed, Physics.gravity.y, forwardSpeed);
		//Vector3 speed = new Vector3 (forwardSpeed, verticalVelocity,  sideSpeed);
		Vector3 speed = faceVector * forwardSpeed + sideVector * sideSpeed;
		speed.y = verticalVelocity;

		#region Move/Walk
		//legacy code:
		//characterController.SimpleMove (speed);
		if (characterController.Move(speed * Time.deltaTime) == CollisionFlags.CollidedBelow)  //or && characterController.isGrounded)
		{
			IsJumping = false;
			verticalVelocity = 0;

		}
		#endregion //move/walk

		#region Jump

		//Infinite Jump
//		if(characterController.isGrounded && Input.GetButtonDown("Jump"))
		if (Input.GetButtonDown("Jump"))
		{
			IsJumping = true;
			verticalVelocity = jumpSpeed;
			canDoubleJump = true;
		}
//		else if(Input.GetButtonDown("Jump") && canDoubleJump == true)
//		{
//			verticalVelocity = jumpSpeed;
//			canDoubleJump = false;
//			anim.SetBool("doubleJump", true);
//		}
//		else
//		{
//			anim.SetBool("doubleJump", false);
//		}
			
		#endregion //jump

		#endregion //movement

		#region Attack
		//Melee Attack
		if (Input.GetMouseButtonUp(0))
		{
			if(AllowedToAttack()) meleeAttackWC.Play();
		}
			
		//Ranged Attack
		else if (Input.GetMouseButtonUp(1))
		{
			if (AllowedToAttack())
			{
				if (ManaPoint >= manaCost)
				{
					IsAttacking = true;
					bullet.Launch(anim.transform.forward, this, this);
					ManaPoint -= manaCost;
					notEnoughMana.SetActive(false);
				}
				else
				{
					notEnoughMana.SetActive(true);
					return;
				}
			}
		}
		else
		{
			IsAttacking = false;
			anim.SetBool("attack", isAttacking);
		}
		#endregion //attack

		#region DeathFall
		if (transform.position.y <= -20)
		{
			Death();
			transform.position = Vector3.zero;
		}
		#endregion //DeathFall
	}


	float horizontalInput, verticalInput, accVerticalInput;

	void CheckCamera()
	{
		horizontalInput = Input.GetAxis("Mouse X") * mouseSensitivity;
		verticalInput = -Input.GetAxis("Mouse Y"); //*mouseSensitivity;

		//Rotasi terhadap sumbu Y
		Camera.main.transform.RotateAround(transform.position, focusObject.TransformDirection(Vector3.up), horizontalInput);
		focusObject.Rotate(focusObject.TransformDirection(Vector3.up), horizontalInput);
		barrelTip.RotateAround(transform.position, focusObject.TransformDirection(Vector3.up), horizontalInput);

		//if (verticalInput + accVerticalInput > 60) {
		if (verticalInput > 60 - accVerticalInput)
		{
			verticalInput = 60 - accVerticalInput;
		}
		//else if(verticalInput + accVerticalInput < 0) {
		else if (verticalInput < -accVerticalInput)
		{
			verticalInput = -accVerticalInput;
		}
		accVerticalInput += verticalInput;

		//Rotasi terhadap sumbu X
		Camera.main.transform.RotateAround(transform.position, focusObject.TransformDirection(Vector3.right), verticalInput);
	}

	void OnCollisionEnter(Collision col)
	{
		Debug.Log(col);
	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Debug.Log(hit.gameObject);
	}

	void Revive()
	{
//		transform.position = Vector3.zero;
		transform.position = respawnPos[0];
		HitPoint = maxHitPoint;
		ManaPoint = maxManaPoint;
		IsDead = false;
	}

	#region implemented abstract members of LivingObject

	protected override void Death()
	{
		IsDead = true;

		Revive();
		//do whatever you want
	}

	#endregion

	WrappedCoroutine meleeAttackWC;

	IEnumerator MeleeAttack()
	{
		IsAttacking = true;
		anim.SetBool("attack", isAttacking);
		//random AttackSound
		int n = Random.Range(0, 2);
		attackSound[n].Play();
        yield return new WaitForSeconds(3);
	}

	void InitializeSkill()
	{
		meleeAttackWC = new WrappedCoroutine(this, MeleeAttack());
	}

	bool AllowedToAttack()
	{
		if(meleeAttackWC.IsRunning) return false;

		return true;
	}
}
