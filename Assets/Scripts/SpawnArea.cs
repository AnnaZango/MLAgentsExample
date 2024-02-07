using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] float spawnRangeX;
    [SerializeField] float spawnRangeZ;
    [SerializeField] float spawnRange;

    Vector3 resultPosition;
    TargetRunaway targetRunaway;

    private void Awake()
    {
        targetRunaway = targetTransform.GetComponent<TargetRunaway>();
    }

    public void PositionTargetAtRandomPos()
    {
        while (!GetPosition())
        {
            CanGetRandomPoint(spawnRange, out resultPosition);
        }

        targetTransform.localPosition = new Vector3(resultPosition.x, 0.5f, resultPosition.z);
        targetRunaway.StopMovement();
    }
        

    private bool GetPosition()
    {
        if(CanGetRandomPoint(spawnRange, out resultPosition))
        {
            return true;
        } else 
        {
            return false; 
        }
    }

    private bool CanGetRandomPoint(float range, out Vector3 result)
    {
        Vector3 randomPointInArea = new Vector3(Random.Range(-spawnRangeX, spawnRangeZ), 0, Random.Range(-spawnRangeZ, spawnRangeZ));

        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPointInArea, out hit, 0.2f, NavMesh.AllAreas))
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

}
