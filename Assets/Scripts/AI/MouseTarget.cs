using System;
using UnityEngine;

public class MouseTarget : MonoBehaviour
{
    public Vector3 TargetWorldPosition { get; private set; }

    public static event Action<Vector3> NewTargetPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                TargetWorldPosition = hit.point;
                NewTargetPosition?.Invoke(TargetWorldPosition);
            }
        }
    }
}
