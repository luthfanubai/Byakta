using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	static PlayerController player;
	[SerializeField]
	int bulletDamage;
	[SerializeField]
	float bulletSpeed;
	[SerializeField]
	float timeSpan;

	void Start () {
		player = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
	}
	

	void Update () {

	}

	public void Launch(Vector3 direction, LivingObject caster, IRangedAttacker iRangedAttacker)
	{
		BulletController bulletClone;
		bulletClone = (BulletController)Instantiate (this, iRangedAttacker.BarrelTip.position, iRangedAttacker.BarrelTip.rotation);
		bulletClone.GetComponent<Rigidbody> ().velocity = direction.normalized * bulletSpeed;
		bulletClone.caster = caster;
		Destroy (bulletClone.gameObject, timeSpan);
	}

	LivingObject caster;
	void OnCollisionEnter(Collision hit)
	{
		var livingObject = hit.gameObject.GetComponent<LivingObject> ();
		if (livingObject != null) //exist
		{
			if (livingObject.ally != caster.ally) 
			{
				livingObject.HitPoint -= bulletDamage;
			}
		}

		Destroy (gameObject);
	}
}
