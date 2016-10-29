using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	CharacterController enemyController;
	static PlayerController player;

	TextMesh txtHP;
	//GameObject player;
	public float movementSpeed = 3.0f;
//	float verticalVelocity = 0;

	[SerializeField]
	private int hitPoint;

	public int HitPoint
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
		txtHP = GetComponentInChildren<TextMesh> ();
	
	}
	
	// Update is called once per frame
	void Update () {
//		verticalVelocity += Physics.gravity.y * Time.fixedDeltaTime;
		float distance = Vector3.Distance (gameObject.transform.position, player.transform.position);
		Vector3 moveTo = player.transform.position - transform.position;
//		moveTo.y = verticalVelocity;


		if (distance <= 10) {
			transform.LookAt (player.transform);	
			enemyController.Move (moveTo.normalized * Time.deltaTime * movementSpeed);
		}
//		Debug.Log (distance);



	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		//GameObject.FindWithTag("Enemy");
		if (player.gameObject == hit.gameObject && player.IsAttacking) {
			HitPoint -= 10;
		}
		//Destroy (col.gameObject);
	}


}
