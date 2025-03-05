using System;
using UnityEngine;

[RequireComponent(typeof(TurnTowards), typeof(MoveForward))]
public class FollowPath : MonoBehaviour
{
    [SerializeField] private TurnTowards turnTowards;
    [SerializeField] private Wander wander;
    [SerializeField] private Avoid avoid;
    [SerializeField] private float waypointReachedDistance = 1f;
    private IPathingCalculator pathingCalculator;
    private Vector3[] targetWaypoints;
    private int currentWaypointIndex = 0;
    
    public event Action TargetReached;
    
    private void Awake()
    {
        wander ??= GetComponent<Wander>();
        avoid ??= GetComponent<Avoid>();
        turnTowards ??= GetComponent<TurnTowards>();
        pathingCalculator ??= GetComponent<IPathingCalculator>();
    }

    private void OnEnable()
    {
        pathingCalculator.NewPathCalculated += SetNewPath;
    }

    private void OnDisable()
    {
        pathingCalculator.NewPathCalculated -= SetNewPath;
    }

    private void Update()
    {
        HandlePathFollow();
    }

    private void SetNewPath(Vector3[] waypoints)
    {
        currentWaypointIndex = 0;
        targetWaypoints = waypoints;
        turnTowards.TargetPosition = targetWaypoints[0];
        turnTowards.HasTarget = true;
        if (avoid != null)
        {
            avoid.IgnoreWalls = true;
        }

        if (wander != null)
        {
            wander.enabled = false;
        }
    }

    private void HandlePathFollow()
    {
        if (targetWaypoints == null || targetWaypoints.Length == 0) return;

        if (!(Vector3.Distance(transform.position, targetWaypoints[currentWaypointIndex]) <= waypointReachedDistance)) return;
        
        if (currentWaypointIndex >= targetWaypoints.Length - 1)
        {
            targetWaypoints = null;
            turnTowards.HasTarget = false;
            TargetReached?.Invoke();
            if (avoid)
            {
                avoid.IgnoreWalls = false;
            }

            if (wander)
            {
                wander.enabled = true;
            }

            return;
        }
        
        currentWaypointIndex++;
        turnTowards.TargetPosition = targetWaypoints[currentWaypointIndex];

    }
    
    private void OnDrawGizmos()
    {
        if (targetWaypoints == null || targetWaypoints.Length == 0 || !DebugToggles.DrawTargetRoutes) return;
        Gizmos.color = Color.green;
        for (int i = 0; i < targetWaypoints.Length - currentWaypointIndex; i++)
        {
            if (i == 0)
            {
                Gizmos.DrawLine(transform.position, targetWaypoints[currentWaypointIndex]);
            }
            else
            {
                Gizmos.DrawLine(targetWaypoints[i + currentWaypointIndex - 1], targetWaypoints[i + currentWaypointIndex]);
            }
        }
    }
}
