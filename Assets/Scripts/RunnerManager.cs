using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerManager : MonoBehaviour
{
    public GameObject runnerPrefab;
    public int numberOfElders = 10;
    public float spawnRadius = 5f;

    void Start()
    {
        Vector3 prefabPosition = runnerPrefab.transform.position;

        for (int i = 0; i < numberOfElders; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition(prefabPosition);
            GameObject elderObject = Instantiate(runnerPrefab, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomSpawnPosition(Vector3 prefabPosition)
    {
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);
        Vector3 randomOffset = new Vector3(randomX, 0f, randomZ);
        Vector3 randomPosition = prefabPosition + randomOffset;
        return randomPosition;
    }
}
