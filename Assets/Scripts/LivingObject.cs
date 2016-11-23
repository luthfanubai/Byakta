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
	protected float movementSpeed;

	[SerializeField]
	protected float attackRange;
	[SerializeField]
	protected float attackTime;

//    public abstract int baseStr { get; }
//    public abstract int gainStr { get; }
//
//    public abstract int baseAgi { get; }
//    public abstract int gainAgi { get; }
//
//    public abstract int baseInt { get; }
//    public abstract int gainInt { get; }
//
//    public abstract int baseAtkDmg { get; }
//    public abstract int baseCritChance { get; }
//
//    public abstract int baseDef { get; }
//    public abstract int baseMagRes { get; }
//
//    public abstract int baseAtkSpd { get; }
//    public abstract int baseMovSpd { get; }
//
//    public abstract int baseHP { get; }
//    public abstract int baseHPRegen { get; }
//    public abstract int baseMP { get; }
//    public abstract int baseMPRegen { get; }


}