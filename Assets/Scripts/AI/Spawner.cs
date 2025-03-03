using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int spawnAmount;
    [SerializeField] private Vector3 spawnArea;

    private void Start()
    {
        if (prefab != null)
            Spawn();
    }

    private void Spawn()
    {
        for (var i = 0; i < spawnAmount; i++)
            Instantiate(prefab,
                transform.position + new Vector3(Random.Range(-spawnArea.x, spawnArea.x),
                    Random.Range(-spawnArea.y, spawnArea.y), Random.Range(-spawnArea.z, spawnArea.z)),
                Quaternion.Euler(0, Random.Range(0, 360), 0));
    }
}