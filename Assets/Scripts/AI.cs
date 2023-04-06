using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    enum State
    {
        Patrolling,
        Chasing,
        Attacking
    }

    State currentState;
    NavMeshAgent agent;

    public Transform[] destinationPoints;
    int destinationIndex = 0;    

    public Transform player;

    [SerializeField]
    float visionRange;

    [SerializeField] float attackRange;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Patrolling;

        destinationIndex = Random.Range(0, destinationPoints.Length);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
            break;
            case State.Chasing:
                Chase();
            break;
            case State.Attacking:
                Attack();
            break;
            default:
                Patrol();
            break;
        }
    }

    void Patrol()
    {
        agent.destination = destinationPoints[destinationIndex].position;

        if(Vector3.Distance(transform.position, destinationPoints[destinationIndex].position) < 1f)
        {
            destinationIndex = Random.Range(0, destinationPoints.Length);
        }

        /*if(Vector3.Distance(transform.position, player.position) < visionRange)
        {
            currentState = State.Chasing;
        } */

        if(DistanceToTarget(visionRange))
        {
            currentState = State.Chasing;
        }       
    }

    /*void Patrol()
    {
        Vector3 randomPosition;
        if(RandomPoint(patrolZone.position, patrolRange, out randomPosition))
        {
            agent.destination = randomPosition;
            Debug.DrawRay(randomPosition, Vector3.up * 5, Color.blue, 5f);
        }

        if(FindTarget())
        {
            currentState = State.Chasing;
        }

        currentState = State.Traveling;        
    }*/

    /*bool RandomPoint(Vector3 center, float range, out Vector3 point)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint, out hit, 4, NavMesh.AllAreas))
        {
            point = hit.position;
            return true;
        }
        point = Vector3.zero;
        return false;
    }*/

    /*void Travel()
    {
        if(agent.remainingDistance <= 0.2)
        {
            currentState = State.Patrolling;
        }

        if(FindTarget())
        {
            currentState = State.Chasing;
        }
    }*/

    void Chase()
    {
        agent.destination = player.position;

        if(DistanceToTarget(visionRange) == false)
        {
            currentState = State.Patrolling;
        }

        if(DistanceToTarget(attackRange))
        {
            currentState = State.Attacking;
        }
        
    }

    void Attack()
    {
        Debug.Log("Ataque");

        if(!DistanceToTarget(attackRange))
        {
            currentState = State.Chasing;
        }
    }

    bool DistanceToTarget(float distance)
    {
        if(Vector3.Distance(transform.position, player.position) < distance)
        {
            return true;
        }

        return false;
    }

    void OnDrawGizmos()
    {
        foreach (Transform point in destinationPoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(point.position, 1);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        /*Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(patrolZone.position, patrolRange);*/

        /*Vector3 dirAngle = new Vector3(Mathf.Sin((visionAngle / 2) * Mathf.Deg2Rad), 0, Mathf.Cos((visionAngle / 2) * Mathf.Deg2Rad));
        Vector3 angleA = -dirAngle;
        Vector3 angleB = dirAngle;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + angleA * visionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + angleB * visionRange);*/
    }
}
