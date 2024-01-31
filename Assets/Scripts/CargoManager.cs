using UnityEngine;
using UnityEngine.UI;

public class CargoManager : MonoBehaviour
{
    public Transform[] cargoSpawnPoints;
    public GameObject cargoPrefab;
    public Text deliveredCargoCounter;
    public float deliveredCargoCount = 0;

    private GameObject[] lastLoadedBoxes;
    private bool removalCooldown = false;
    private int remainingBoxes;
    private GameManager gameManager;
    void Start()
    {
        lastLoadedBoxes = new GameObject[cargoSpawnPoints.Length];
        remainingBoxes = cargoSpawnPoints.Length;
        LoadTruck();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        deliveredCargoCounter.text = deliveredCargoCount.ToString();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target") && !removalCooldown)
        {
            removalCooldown = true;
            Destroy(other.gameObject);
            deliveredCargoCount += 1;

            for (int i = cargoSpawnPoints.Length - 1; i >= 0; i--)
            {

                Destroy(lastLoadedBoxes[i]);

                if (lastLoadedBoxes[i] == null)
                {
                    continue;
                }

                remainingBoxes--;

                if (remainingBoxes == 0)
                {
                    LoadTruck();
                }
                break;
            }
            SpawnTargetRandomly();
            
            StartCoroutine(RemovalCooldown());

            gameManager.countdownTimer += 10;
        }
    }

    System.Collections.IEnumerator RemovalCooldown()
    {
        yield return new WaitForSeconds(1f);
        removalCooldown = false;
    }

    void SpawnTargetRandomly()
    {
        gameManager.SpawnTargetRandomly();
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