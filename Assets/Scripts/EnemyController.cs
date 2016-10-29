using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	[SerializeField]
	private int hitPoint;

	public int HitPoint
	{
		get { return hitPoint; }
		set 
		{
			hitPoint = value;
			if(value <= 0)
			{Destroy (gameObject);}
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (hitPoint);
	}


}
