using System.Collections;
using UnityEngine;

public class SpawnSite : MonoBehaviour
{
    static PlayerController player;
    GameObject monsterPrefab;

    Vector3 spawnedPosition;

    float enemyRadar = 50f;
    float respawnTime = 60f;

    public bool playerDetected
    {
        get
        {
            return (player.FocusObject.position - transform.position).magnitude <= enemyRadar;
        }
    }

    public void Activate(GameObject monsterPrefab)
    {
        if (this.monsterPrefab != null)
            return;
        
        player = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
        this.monsterPrefab = monsterPrefab;
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        GameObject monsterClone;
        while (true)
        {
            if (playerDetected)
            {
                spawnedPosition = transform.position;
                monsterClone = (GameObject)Instantiate(monsterPrefab, spawnedPosition, Quaternion.identity);
                monsterClone.transform.SetParent(transform);

                while (monsterClone != null)
                {
                    yield return null;
                }
                yield return new WaitForSeconds(respawnTime);
            }
            else
            {
                yield return null;
            }
        }
    }
}
