using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);
    }
}
