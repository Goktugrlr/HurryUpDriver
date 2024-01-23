using UnityEngine;

public class CargoManager : MonoBehaviour
{
    public Transform[] cargoSpawnPoints;
    public GameObject cargoPrefab;

    private GameObject[] lastLoadedBoxes;
    private bool removalCooldown = false;
    private int remainingBoxes;

    void Start()
    {
        lastLoadedBoxes = new GameObject[cargoSpawnPoints.Length];
        remainingBoxes = cargoSpawnPoints.Length;
        LoadTruck();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target") && !removalCooldown)
        {
            removalCooldown = true;

            Destroy(other.gameObject);

            for (int i = cargoSpawnPoints.Length - 1; i >= 0; i--)
            {

                Destroy(lastLoadedBoxes[i]);
                lastLoadedBoxes[i] = null;

                remainingBoxes--;

                    if (remainingBoxes == 0)
                    {
                        LoadTruck();
                    }
                    break;          
            }
            StartCoroutine(RemovalCooldown());

            Invoke(nameof(SpawnTargetRandomly), 1f); 
        }
    }

    System.Collections.IEnumerator RemovalCooldown()
    {
        yield return new WaitForSeconds(1f);  
        removalCooldown = false;
    }

    void SpawnTargetRandomly()
    {
        FindObjectOfType<GameManager>().SpawnTargetRandomly();
    }

    void LoadTruck()
    {
        for (int i = 0; i < cargoSpawnPoints.Length; i++)
        {
            GameObject newBox = Instantiate(cargoPrefab, cargoSpawnPoints[i].position, Quaternion.identity);
            newBox.transform.parent = cargoSpawnPoints[i];
            lastLoadedBoxes[i] = newBox;
        }

        remainingBoxes = cargoSpawnPoints.Length;
    }
}