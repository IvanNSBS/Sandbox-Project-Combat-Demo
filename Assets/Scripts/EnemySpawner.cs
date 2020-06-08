using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] float minDelayBetweenSpawns = 10f;
    [SerializeField] float maxDelayBetweenSpawns = 10f;
    [SerializeField] int maxNumberOfAliveEnemies = 10;


    [SerializeField] List<GameObject> referenceTest;


    void Start()
    {
        foreach(Transform t in spawnPoints){
            var spawned = Instantiate(enemyToSpawn);
            spawned.transform.position = t.position;
        }        
    }

    void Update(){
        Debug.Log(referenceTest.Count);
    }
}
