using UnityEngine;
using UnityEngine.Serialization;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] private GameObject visuals;
    [SerializeField] private NavMeshPathCalculator navMeshPathCalculator;

    private void OnEnable()
    {
        MouseTarget.LeftClickNewTargetPosition += MoveTargetIndicator;
        navMeshPathCalculator.TargetReached += DisableTargetIndicator;
    }

    private void OnDisable()
    {
        MouseTarget.LeftClickNewTargetPosition -= MoveTargetIndicator;
        navMeshPathCalculator.TargetReached -= DisableTargetIndicator;
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