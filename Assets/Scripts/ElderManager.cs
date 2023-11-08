using System.Collections.Generic;
using UnityEngine;

public class ElderManager : MonoBehaviour
{
    public GameObject elderPrefab;
    public int numberOfElders = 10;
    public float spawnRadius = 5f;
    public List<GameObject> benchs = new List<GameObject>();

    void Start()
    {
        Vector3 prefabPosition = elderPrefab.transform.position;

        for (int i = 0; i < numberOfElders; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition(prefabPosition);
            GameObject elderObject = Instantiate(elderPrefab, spawnPosition, Quaternion.identity);
            Elder elderScript = elderObject.GetComponent<Elder>();
            elderScript.benchs = benchs;
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
