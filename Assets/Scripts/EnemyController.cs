using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	CharacterController enemyController;
	static PlayerController player;

	public GameObject bullet;
	Transform bulletSpawn;

	TextMesh txtHP;
	//GameObject player;
	public float movementSpeed = 3.0f;
//	float verticalVelocity = 0;

	[SerializeField]
	private float hitPoint;

	public float HitPoint
	{
		get { return hitPoint; }
		set 
		{
			hitPoint = value;
			txtHP.text = hitPoint.ToString();
			if(value <= 0)
			{Destroy (gameObject);}

		}
	}

	// Use this for initialization
	void Start () {
		enemyController = GetComponent<CharacterController> ();
		player = GameObject.FindWithTag ("Player").GetComponent<PlayerController>();
		bulletSpawn = bullet.transform;
		txtHP = GetComponentInChildren<TextMesh> ();
		StartCoroutine (Patrol ());
	}
	
	// Update is called once per frame
	void Update () {
//		verticalVelocity += Physics.gravity.y * Time.fixedDeltaTime;
		float distance = Vector3.Distance (gameObject.transform.position, player.transform.position);
		Vector3 moveToVector = player.transform.position - transform.position;
//		moveTo.y = verticalVelocity;


		if (distance <= 10) {
			transform.LookAt (player.transform);	
//			enemyController.Move (moveToVector.normalized * Time.deltaTime * movementSpeed);
			//Fire();
		}



	}
		
	void Fire()
	{
		GameObject bulletClone;
		bulletClone = (GameObject)Instantiate (bullet, transform.position, transform.rotation);
		Debug.Log (transform.position + "  " + transform.localPosition);

		bulletClone.GetComponent<Rigidbody> ().velocity = bullet.transform.forward * 6;

		Destroy(bulletClone, 2.0f);


	}


//	void OnCollisionEnter (Collision col)
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Debug.Break ();		
		//GameObject.FindWithTag("Enemy");
		if (player.gameObject == hit.gameObject && player.IsAttacking) {
			HitPoint -= 10;
		}
		//Destroy (col.gameObject);
	}

	IEnumerator Shoot(float fireTime)
	{
		
		Vector3 moveToPlayer;
		float distance;
		GameObject bulletClone;

		while (true) {
			moveToPlayer = player.transform.FindChild("FocusObject").position - transform.position;
			distance = moveToPlayer.magnitude;
			if (distance <= 10) {
				transform.LookAt (player.transform);
//				enemyController.Move (moveToPlayer * Time.deltaTime * movementSpeed);
				bulletClone = (GameObject)Instantiate (bullet, transform.localPosition, transform.rotation);
				bulletClone.GetComponent<Rigidbody> ().velocity = moveToPlayer.normalized * 10;
				Destroy(bulletClone, 2.0f);


				yield return new WaitForSeconds (fireTime);
			} else
				yield break;
		}
	}

	bool isAlive = true;
	IEnumerator Patrol()
	{
		float distance;
		while (isAlive) {
			distance = Vector3.Distance (gameObject.transform.position, player.transform.position);
			if (distance <= 10) {
				yield return StartCoroutine (Shoot (0.5f));
			}
			else yield return new WaitForSeconds(0.2f);
		}
	}

	void OnDestroy()
	{
		isAlive = false;
	}
}
