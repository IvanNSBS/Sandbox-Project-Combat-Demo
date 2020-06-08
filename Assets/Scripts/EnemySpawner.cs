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

    int aliveEnemies = 0;
    List<GameObject> enemies = new List<GameObject>();
    float? clockToSpawnEnemy = null;


    void Start()
    {
        aliveEnemies = maxNumberOfAliveEnemies;


        for(int i = 0; i < maxNumberOfAliveEnemies; i++){
            enemies.Add(SpawnEnemy());
        }        
    }

    GameObject SpawnEnemy(){
        if(spawnPoints.Count == 0 || enemyToSpawn == null)
            return null;

        int idx = Random.Range(0, spawnPoints.Count - 1);
        GameObject spawnedEnemy = Instantiate(enemyToSpawn);
        spawnedEnemy.transform.position = spawnPoints[idx].position;
        return spawnedEnemy;
    }

    void Update(){

        enemies.RemoveAll( item => item == null );
        aliveEnemies = enemies.Count;

        if(aliveEnemies < maxNumberOfAliveEnemies){
            clockToSpawnEnemy = Random.Range(minDelayBetweenSpawns, maxDelayBetweenSpawns);
        }

        if(clockToSpawnEnemy != null){
            clockToSpawnEnemy -= Time.deltaTime;

            if(clockToSpawnEnemy <= 0f){
                enemies.Add(SpawnEnemy());
                clockToSpawnEnemy = null;
            }
        }
    }
}
