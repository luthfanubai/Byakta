using UnityEngine;
using System.Collections;

public class OrbController : MonoBehaviour {

	static PlayerController player;
	Behaviour haloSphere;

	void Start () 
	{
		player = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
		haloSphere = (Behaviour)GetComponent ("Halo");
		haloSphere.enabled = false;
	}

	void Update () 
	{
		if (player.IsRunning) 
		{
			haloSphere.enabled = true;
		} 
		else 
		{
			haloSphere.enabled = false;
		}

		if (player.IsAttacking) 
		{
			//haloSphere.
		}
	}
}
