using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class ZombieAgent : Agent
{
    [SerializeField] Transform targetTransform;
    [SerializeField] SpawnArea spawnArea;

    [SerializeField] float forceMultiplier;
    [SerializeField] float rotationSpeed = 200;

    public bool useVectorObs;
    Rigidbody rb;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }
    

    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

        spawnArea.PositionTargetAtRandomPos();       
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(targetTransform.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        //distance to target
        sensor.AddObservation(Vector3.Distance(this.transform.localPosition, targetTransform.localPosition));

        //direction towards target
        Vector3 targetDirection = transform.position - targetTransform.position;
        sensor.AddObservation(targetDirection);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        AddReward(-1f / MaxStep);     

        MoveAgent(actionBuffers.DiscreteActions);
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        int forward = act[0]; // can be 0 (don't move) to 1 (move)
        int leftOrRight = 0; //can be 0 (no turn), -1 (turn left) or 1 (turn right);

        if (act[1] == 1)
        {
            leftOrRight = -1;
        } else if (act[1] == 2)
        {
            leftOrRight = 1;
        }

        transform.Rotate(transform.up * leftOrRight, Time.deltaTime * rotationSpeed);
        rb.AddForce(transform.forward* forward * forceMultiplier, ForceMode.VelocityChange);
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[1] = 2;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[1] = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 0;
        }
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "target")
        {
            SetReward(1f);
            EndEpisode();
        } 
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "obstacle")
        {
            SetReward(-50f/MaxStep);
        } else if (other.gameObject.tag == "wall")
        {
            AddReward(-1f / MaxStep);
        }
    }

}
