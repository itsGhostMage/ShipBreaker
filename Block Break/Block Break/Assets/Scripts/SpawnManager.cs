using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject[] enemies;
    public GameObject[] powerUps;

    [System.Serializable]
    public class SpawnPoint
    {
        public Transform spawnTransform;
        public Vector3[] possibleTargets;
    }
    public SpawnPoint[] spawnPattern; // Set in inspector with 5 positions

    public float moveSpeed = 5f;
    public float rotationSpeed = 360f;
    public float flightHeight = 3f;

    public List<GameObject> activeEnemies = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemyWave();
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        // Clean up destroyed enemies
        activeEnemies.RemoveAll(item => item == null);

        if (activeEnemies.Count == 0)
        {
            SpawnEnemyWave();
        }
    }

    void SpawnEnemyWave()
    {
        foreach (SpawnPoint spawn in spawnPattern)
        {
            if (spawn.spawnTransform != null && spawn.possibleTargets.Length > 0)
            {
                SpawnAtSpawnPoint(spawn);
            }
        }
    }

    
    void SpawnAtSpawnPoint(SpawnPoint spawn)
    {
        int randomEnemy = Random.Range(0, enemies.Length);
        Vector3 target = spawn.possibleTargets[Random.Range(0, spawn.possibleTargets.Length)];

        GameObject enemy = Instantiate(enemies[randomEnemy], spawn.spawnTransform.position, Quaternion.identity);

        EnemyMover mover = enemy.AddComponent<EnemyMover>();
        mover.Initialize(
            startPos: spawn.spawnTransform.position,
            endPos: target,
            height: flightHeight,
            speed: moveSpeed,
            rotationSpeed: rotationSpeed
        );

        activeEnemies.Add(enemy);
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10f, 20f));
            Vector3 spawnPos = new Vector3(Random.Range(-22f, 22f), -11, 0);
            Instantiate(powerUps[Random.Range(0, powerUps.Length)], spawnPos, Quaternion.identity);
        }
    }

    // Modify your enemy destruction logic to remove from list
    public void RemoveEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
    }
}
