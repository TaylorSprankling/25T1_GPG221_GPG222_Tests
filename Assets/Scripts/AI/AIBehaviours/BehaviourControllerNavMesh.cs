using UnityEngine;
using UnityEngine.Serialization;

public class BehaviourControllerNavMesh : BehaviourController
{
    [SerializeField] private NavMeshPathCalculator navMeshPathCalculator;
    [SerializeField] private bool stopOnTargetReached = true;

    private void OnEnable()
    {
        navMeshPathCalculator.NewTargetSet += MoveTowardsTarget;
        navMeshPathCalculator.TargetReached += TargetReachedBehaviour;
    }

    private void OnDisable()
    {
        navMeshPathCalculator.NewTargetSet -= MoveTowardsTarget;
        navMeshPathCalculator.TargetReached -= TargetReachedBehaviour;
    }

    private void MoveTowardsTarget()
    {
        moveForward.enabled = true;
        wander.enabled = false;
        avoid.enabled = true;
        avoid.IgnoreWalls = true;
    }

    private void TargetReachedBehaviour()
    {
        if (stopOnTargetReached)
        {
            moveForward.enabled = false;
            avoid.enabled = false;
        }
        else
        {
            moveForward.enabled = true;
            wander.enabled = true;
            avoid.enabled = true;
            avoid.IgnoreWalls = false;
        }
    }
}
