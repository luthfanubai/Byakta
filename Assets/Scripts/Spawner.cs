using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    [SerializeField]
    GameObject[] monsterPrefabs;
    SpawnSite[] spawnSites;

	// Use this for initialization
	void Start () {
        
        int number = monsterPrefabs.Length;
        spawnSites = new SpawnSite[number];
        for (int i = 0; i < number; i++)
        {
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * 20;
            randomPosition.y = transform.position.y;

            spawnSites[i] = new GameObject("Spawn Site " + (i + 1)).AddComponent<SpawnSite>();
            spawnSites[i].transform.SetParent(transform);
            spawnSites[i].transform.position = randomPosition;
            spawnSites[i].Activate(monsterPrefabs[i]);
        }
	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
}
