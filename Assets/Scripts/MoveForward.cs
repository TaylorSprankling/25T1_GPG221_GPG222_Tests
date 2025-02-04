using UnityEngine;
using UnityEngine.Serialization;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3 moveForce;
    private void FixedUpdate()
    {
        rb.AddRelativeForce(moveForce);
    }
}
