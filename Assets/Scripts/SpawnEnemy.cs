using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {

    public static PlayerController player;
    float playerDistance;
    Vector3 enemyToPlayer;
    public float spawnDistance;


	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
        enemyToPlayer = player.FocusObject.position - transform.position;
        playerDistance = enemyToPlayer.magnitude;
        if (playerDistance <= spawnDistance)
            transform.FindChild("Spawn").gameObject.SetActive(true);
	}
}
