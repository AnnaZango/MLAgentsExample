using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetRunaway : MonoBehaviour
{
    [SerializeField] GameObject zombie;
    [SerializeField] float runawaySpeed = 2f;
    [SerializeField] float detectionDistance = 3f;
    [SerializeField] float runawayDistance = 2f;
    float distanceToZombie;
    NavMeshAgent agent;
    Vector3 resultPosition;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();  
        agent.speed = runawaySpeed;
    }

    
    void Update()
    {
        distanceToZombie = Vector3.Distance(new Vector3(zombie.transform.position.x, 0, zombie.transform.position.z),
            new Vector3(transform.position.x, 0, transform.position.z));

        if(distanceToZombie < detectionDistance)
        {
            agent.isStopped = false;
            RunAway();
        }
    }

    private void RunAway()
    {
        Vector3 runawayDir = (transform.position - new Vector3(zombie.transform.position.x, 
            transform.position.y, 
            zombie.transform.position.z)).normalized;
       
        if(CanGetPosition(runawayDir, runawayDistance, out  resultPosition))
        {
            agent.SetDestination(transform.position + resultPosition);
        }
        else
        {
            //Debug.Log("Cannot run away!");
        }
    }

    
    private bool CanGetPosition(Vector3 runAwayDir, float runawayDis, out Vector3 result)
    {
        Vector3 runawayPoint = runAwayDir * runawayDis;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(runawayPoint, out hit, 0.2f, NavMesh.AllAreas))
        {
            result = hit.position;
            Debug.DrawRay(result, Vector3.up, Color.blue, 1.0f);
            return true;
        }
        else
        {
            result = Vector3.zero;
            return false;
        }
    }

    public void StopMovement()
    {
        agent.isStopped = true;
        agent.SetDestination(transform.position);
    }


}
