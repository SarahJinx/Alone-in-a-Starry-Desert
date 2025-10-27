using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "GroundPatrol", menuName = "ScriptableObjects/States/GroundPatrol")]
public class GroundPatrol : State
{
    public float stopDistance = 1.0f;

    public override State Run(GameObject owner)
    {
        var agent = owner.GetComponent <UnityEngine.AI.NavMeshAgent>();
        if (agent == null) return this;
        var points = owner.GetComponent<PatrolPoints>();
        if (points == null || points.pointA == null || points.pointB == null) return this;
        var mem = owner.GetComponent<PatrolABData>();
        if (mem == null) mem = owner.AddComponent<PatrolABData>();

        Vector3 target = mem.goingToB ? points.pointB.position : points.pointA.position;

        if (!agent.hasPath)
        {
            agent.SetDestination(target);
        }
        if(agent.remainingDistance >= stopDistance && !agent.pathPending)
        {
            mem.goingToB = !mem.goingToB;
            target = mem.goingToB ? points.pointB.position : points.pointA.position;
            agent.SetDestination(target);
        }
        if(action != null && action.Length > 0 && action[0] != null && action[0].Check(owner))
        {
            return nextState != null && nextState.Length > 0 ? nextState[0] : this;
        }
        return this;
    }
}
