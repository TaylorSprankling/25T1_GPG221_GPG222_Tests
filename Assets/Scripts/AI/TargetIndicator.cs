using UnityEngine;
using UnityEngine.Serialization;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] private GameObject navMeshVisual;
    [SerializeField] private GameObject customPathVisual;
    [SerializeField] private FollowPath navMeshFollowPath;
    [SerializeField] private FollowPath customFollowPath;

    private void OnEnable()
    {
        MouseTarget.LeftClickNewTargetPosition += MoveNavMashTargetIndicator;
        MouseTarget.RightClickNewTargetPosition += MoveCustomTargetIndicator;
        navMeshFollowPath.TargetReached += DisableNavMeshTargetIndicator;
        customFollowPath.TargetReached += DisableCustomTargetIndicator;
    }

    private void OnDisable()
    {
        MouseTarget.LeftClickNewTargetPosition -= MoveNavMashTargetIndicator;
        MouseTarget.RightClickNewTargetPosition -= MoveCustomTargetIndicator;
        navMeshFollowPath.TargetReached -= DisableNavMeshTargetIndicator;
        customFollowPath.TargetReached -= DisableCustomTargetIndicator;
    }

    private void MoveNavMashTargetIndicator(Vector3 targetPosition)
    {
        navMeshVisual.transform.position = targetPosition;
        navMeshVisual.SetActive(true);
    }

    private void DisableNavMeshTargetIndicator()
    {
        navMeshVisual.SetActive(false);
    }

    private void MoveCustomTargetIndicator(Vector3 targetPosition)
    {
        customPathVisual.transform.position = targetPosition;
        customPathVisual.SetActive(true);
    }
    
    private void DisableCustomTargetIndicator()
    {
        customPathVisual.SetActive(false);
    }
}