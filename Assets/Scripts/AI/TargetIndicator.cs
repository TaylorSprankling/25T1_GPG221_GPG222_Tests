using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] private GameObject visuals;
    [SerializeField] private NavMeshPathFollower navMeshPathFollower;

    private void OnEnable()
    {
        MouseTarget.NewTargetPosition += MoveTargetIndicator;
        navMeshPathFollower.TargetReached += DisableTargetIndicator;
    }

    private void OnDisable()
    {
        MouseTarget.NewTargetPosition -= MoveTargetIndicator;
        navMeshPathFollower.TargetReached -= DisableTargetIndicator;
    }

    private void MoveTargetIndicator(Vector3 targetPosition)
    {
        transform.position = targetPosition;
        visuals.SetActive(true);
    }

    private void DisableTargetIndicator()
    {
        visuals.SetActive(false);
    }
}