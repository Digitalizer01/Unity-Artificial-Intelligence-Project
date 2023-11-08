using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Elder : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject elderPrefab;
    public float spawnRadius = 5f;
    public float patrolDurationMin = 2f;
    public float patrolDurationMax = 8f;
    public float waitDuration = 2f;
    public float idleDuration = 10f;

    public List<GameObject> benchs = new List<GameObject>();
    public float distanceBenchElder;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float desiredDuration;
    private float elapsedTime;
    private float percentageComplete;
    private enum State { Wander, ChangingDestination, Idle };
    private State currentState;
    private float coolingTime = 10f;
    private float patrollingTime = 0f;

    void Start()
    {
        startPosition = GetRandomSpawnPosition();
        agent = Instantiate(elderPrefab, startPosition, Quaternion.identity).GetComponent<NavMeshAgent>();
        SetRandomDestination();
        currentState = State.Wander;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Wander:
                patrollingTime += Time.deltaTime;

                if (IsNearBanco() && patrollingTime >= coolingTime)
                {
                    currentState = State.Idle;
                    elapsedTime = 0f; // Restart the idle timer
                }
                else
                {
                    PatrollingState();
                }
                break;
            case State.ChangingDestination:
                ChangingDestinationState();
                break;
            case State.Idle:
                // Remain in a resting state for a certain amount of time
                if (elapsedTime >= idleDuration) {
                    currentState = State.Wander;
                    SetRandomDestination(); // At the end of the idle state, resume patrolling
                    currentState = State.Wander;
                }
                else {
                    elapsedTime += Time.deltaTime;
                }
                patrollingTime = 0f;

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

    bool IsNearBanco()
    {
        // Check if Elder is close to a bench of the bench list.
        foreach (GameObject bench in benchs) {
            float distance = Vector3.Distance(agent.transform.position, bench.transform.position);
            if (distance < distanceBenchElder) {
                Debug.DrawLine(agent.transform.position, bench.transform.position, Color.green); // Draw a red line between the bench and the elder
                return true;
            }
            else{
                Debug.DrawLine(agent.transform.position, bench.transform.position, Color.red); // Draw a green line between the bench and the elder
            }
        }
        return false;
    }
}
