using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Runner : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject runnerPrefab;
    public float spawnRadius = 5f;
    public float patrolDurationMin = 2f;
    public float patrolDurationMax = 8f;
    public float waitDuration = 2f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float desiredDuration;
    private float elapsedTime;
    private float percentageComplete;
    private enum State { Wander, ChangingDestination };
    private State currentState;

    void Start()
    {
        startPosition = GetRandomSpawnPosition();
        agent = Instantiate(runnerPrefab, startPosition, Quaternion.identity).GetComponent<NavMeshAgent>();
        SetRandomDestination();
        currentState = State.Wander;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Wander:
                PatrollingState();
                break;
            case State.ChangingDestination:
                ChangingDestinationState();
                break;
        }
    }

    void PatrollingState()
    {
        if (percentageComplete >= 1)
        {
            ChangingDestinationState();
        }
        else
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / desiredDuration;
            agent.destination = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0, 1, percentageComplete));
        }
    }

    void ChangingDestinationState()
    {
        if (percentageComplete >= 1)
        {
            SetRandomDestination();
            currentState = State.Wander;
        }
        else
        {
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / waitDuration;
        }
    }

    void SetRandomDestination()
    {
        float randomX = Random.Range(-10f, 20f);
        float randomZ = Random.Range(-10f, 20f);
        endPosition = new Vector3(randomX, 0f, randomZ);
        elapsedTime = 0f;
        startPosition = agent.transform.position;
        desiredDuration = Random.Range(patrolDurationMin, patrolDurationMax);
        percentageComplete = 0f;
        agent.destination = endPosition;
    }
    Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);
        Vector3 randomPosition = transform.position + new Vector3(randomX, 0f, randomZ);
        return randomPosition;
    }

}
