using UnityEngine;

public class CargoManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject cubePrefab;

    private GameObject[] lastLoadedBoxes;
    private bool removalCooldown = false;

    void Start()
    {
        lastLoadedBoxes = new GameObject[spawnPoints.Length];
        SpawnInitialCubes();
    }

    void SpawnInitialCubes()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject newBox = Instantiate(cubePrefab, spawnPoints[i].position, Quaternion.identity);
            newBox.transform.parent = spawnPoints[i];
            lastLoadedBoxes[i] = newBox;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target") && !removalCooldown)
        {
            removalCooldown = true;

            // Destroy the collided object
            Destroy(other.gameObject);

            // Remove the last loaded box for each spawn point in reverse order
            for (int i = spawnPoints.Length - 1; i >= 0; i--)
            {
                if (lastLoadedBoxes[i] != null)
                {
                    Destroy(lastLoadedBoxes[i]);
                    lastLoadedBoxes[i] = null;
                    break;  // Stop after removing the first non-null box
                }
            }

            Debug.Log("Hit");

            // Start cooldown coroutine
            StartCoroutine(RemovalCooldown());
        }
    }

    System.Collections.IEnumerator RemovalCooldown()
    {
        yield return new WaitForSeconds(1f);  // Adjust the cooldown duration as needed
        removalCooldown = false;
    }
}