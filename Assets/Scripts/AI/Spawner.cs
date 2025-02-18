using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int spawnAmount;

    private void Start()
    {
        if (prefab != null)
            Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}
