using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class LivingObject : MonoBehaviour
{
	[SerializeField]
	protected int maxHitPoint;
	
	[SerializeField]
	protected float hitPoint;

	public virtual float HitPoint 
	{
		get 
		{
			return hitPoint;
		}
		set 
		{
			hitPoint = value;
			if (value > maxHitPoint) 
			{
				hitPoint = maxHitPoint;
			} 
			else if (value <= 0) 
			{
				hitPoint = 0;
				Death ();
			}
		}
	}

	public Alliance ally;

	protected abstract void Death ();

	[SerializeField]
	protected bool isStationary;
	[SerializeField]
	protected float movementSpeed;

	[SerializeField]
	protected bool canAttack;
	[SerializeField]
	protected float attackRange;
	[SerializeField]
	protected float attackTime;
}