using UnityEngine;

public class BehaviourControllerNavMesh : BehaviourController
{
    [SerializeField] private NavMeshPathFollower navMeshPathFollower;
    [SerializeField] private bool stopOnTargetReached = true;

    private void OnEnable()
    {
        navMeshPathFollower.NewTargetSet += MoveTowardsTarget;
        navMeshPathFollower.TargetReached += TargetReachedBehaviour;
    }

    private void OnDisable()
    {
        navMeshPathFollower.NewTargetSet -= MoveTowardsTarget;
        navMeshPathFollower.TargetReached -= TargetReachedBehaviour;
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
