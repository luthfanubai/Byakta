using UnityEngine;
using System.Collections;

public class EnemyController : LivingObject {

	public static PlayerController player;
	float playerDistance;

    [SerializeField]
    GameObject meleeTip;

	CharacterController enemyController;
	TextMesh txtHP;

	public override float HitPoint 
	{
		get 
		{
			return base.HitPoint;
		}
		set 
		{
			base.HitPoint = value;
			txtHP.text = hitPoint.ToString();
		}
	}

	void Start ()
	{
		enemyController = GetComponent<CharacterController> ();
		player = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		txtHP = GetComponentInChildren<TextMesh> ();

		ally = Alliance.Enemy;

		StartCoroutine (Patrol ());
	}
        
    void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject == GameObject.FindGameObjectWithTag("MeleeTip") && player.IsMeleeAttacking) {
            HitPoint -= 10;
        }
    }

	void OnControllerColliderHit (ControllerColliderHit hit)
	{
//		Debug.Break ();		
		//GameObject.FindWithTag("Enemy");
//		if (player.gameObject == hit.gameObject && player.IsMeleeAttacking) {
//			HitPoint -= 10;
//		}
		//Destroy (col.gameObject);
	}

	Vector3 enemyToPlayer;
	IEnumerator Patrol ()
	{
		while (true) {


			enemyToPlayer = player.FocusObject.position - transform.position;
			playerDistance = enemyToPlayer.magnitude;
			//if (playerDistance <= safeDistance) {"Flee();"}
			if (playerDistance <= attackRange) {
                yield return StartCoroutine(Chase());
			}

			yield return null;
		}

	}

    IEnumerator Chase()
    {
        Vector3 directionToTarget, normVector;
        Transform target = player.transform;
        while (true)
        {
            directionToTarget = target.position - transform.position;
            if(directionToTarget.magnitude <= 30)
            {
                transform.LookAt(target);
            }
            else 
            {
                transform.rotation = Quaternion.identity;
                directionToTarget = transform.parent.position - transform.position;
            }

            if(directionToTarget.magnitude > 1E-02) enemyController.Move (directionToTarget.normalized * Time.deltaTime * movementSpeed);
            yield return null;
        }
    }

    public void ChangeTarget(Transform target)
    {

    }

    public void MoveTo(Transform target)
    {
//        transform.position - target.position
    }



	protected override void Death () //virtual method
	{
		StopAllCoroutines ();
		Destroy (gameObject);
	}



}