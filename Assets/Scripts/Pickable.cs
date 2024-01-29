using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    public Transform[] fuelSpawnPoints;
    public Transform[] nitroSpawnPoints;
    public GameObject fuelCan;
    public GameObject nitroTube;
    
    private float respawnDelay = 10f;
    private Vector3 fuelRespawnPosition;

    private void Start()
    {
        SpawnInitialFuel();
        SpawnNitro();
    }

    public void SpawnInitialFuel()
    {
        foreach (Transform spawnPoint in fuelSpawnPoints)
            Instantiate(fuelCan, spawnPoint.position, Quaternion.identity);
    }

    public void RespawnFuel(Vector3 destroyedFuelCanPosition)
    {
        fuelRespawnPosition = destroyedFuelCanPosition;
        Invoke("SpawnFuelAtPosition", respawnDelay);
    }

    private void SpawnFuelAtPosition()
    {
        Instantiate(fuelCan, fuelRespawnPosition, Quaternion.identity);
    }

    public void SpawnNitro()
    {
        int randomIndex = Random.Range(0, nitroSpawnPoints.Length);
        Instantiate(nitroTube, nitroSpawnPoints[randomIndex].position, Quaternion.identity);
    }
}