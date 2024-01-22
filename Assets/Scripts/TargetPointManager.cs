using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // List of specified points to spawn cubes
    public GameObject cubePrefab;    // The cube prefab to instantiate

    void Start()
    {
        // Spawn cubes at specified points
        SpawnCubes();
    }

    void SpawnCubes()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(cubePrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}