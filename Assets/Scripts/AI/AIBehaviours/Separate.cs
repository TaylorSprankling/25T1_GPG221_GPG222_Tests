using UnityEngine;

public class Separate : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Neighbours neighbours;
    [SerializeField] private float separateStrength;

    private void Awake()
    {
        if (rigidBody == null) rigidBody = GetComponent<Rigidbody>();
        if (neighbours == null) neighbours = GetComponent<Neighbours>();
    }

    private void FixedUpdate()
    {
        MoveAwayFromNeighbours();
    }

    private void MoveAwayFromNeighbours()
    {
        if (neighbours.List.Count <= 0) return;
        Vector3 averageDirection = Vector3.zero;

        foreach (GameObject n in neighbours.List)
        {
            Vector3 neighbourDirection = n.transform.position - transform.position;
            Vector3 forceDirection = -neighbourDirection.normalized * (neighbours.NeighbourRadius - neighbourDirection.magnitude);
            averageDirection += forceDirection;
        }

        averageDirection /= neighbours.List.Count;
        rigidBody.AddForce(averageDirection * separateStrength);
    }
}