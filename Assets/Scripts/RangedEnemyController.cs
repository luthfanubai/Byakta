using UnityEngine;
using System.Collections;

public class RangedEnemyController : LivingObject, IRangedAttacker
{
	public static PlayerController player;
	public BulletController bullet;
	public float safeDistance;
	float playerDistance;

	CharacterController characterController;
	TextMesh txtHP;

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
		characterController = GetComponent<CharacterController> ();
		player = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		txtHP = GetComponentInChildren<TextMesh> ();
		((IRangedAttacker)this).BarrelTip = transform.FindChild ("Barrel Tip");

		ally = Alliance.Enemy;

		StartCoroutine (Patrol ());
	}

	void Update()
	{
		if (playerDistance <= attackRange) {
			transform.LookAt (player.FocusObject);
		}
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
		if (player.gameObject == hit.gameObject && player.IsMeleeAttacking) {
			HitPoint -= 10;
		}
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
				Fire (enemyToPlayer);
				yield return new WaitForSeconds (attackTime);
			}

			yield return null;
		}

	}

	void Fire (Vector3 direction)
	{
		bullet.Launch (direction, this, (IRangedAttacker)this);

//		transform.LookAt (player.transform.FindChild("FocusObject"));
//
//		GameObject bulletClone;
//		bulletClone = (GameObject)Instantiate (bullet, barrelTip.position, barrelTip.rotation);
//		bulletClone.GetComponent<Rigidbody> ().velocity = direction.normalized * 10;
//		Destroy (bulletClone, 2f);
	}

	protected override void Death () //virtual method
	{
		StopAllCoroutines ();
		Destroy (gameObject);
	}
}