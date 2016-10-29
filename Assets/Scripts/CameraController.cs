using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(GameObject.Find("Sphere").transform);
		Debug.Log (GameObject.Find ("Sphere").transform);
	}
}
