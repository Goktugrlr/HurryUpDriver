using UnityEngine;
using UnityEngine.UI;

public class CargoManager : MonoBehaviour
{
    public Transform[] cargoSpawnPoints;
    public GameObject cargoPrefab;
    public Text deliveredCargoCounter;
    public int deliveredCargoCount = 0;
    public AudioSource targetReached;

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

    void Update()
    {
        deliveredCargoCounter.text = deliveredCargoCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target") && !removalCooldown)
        {
            removalCooldown = true;
            Destroy(other.gameObject);
            deliveredCargoCount += 1;
            targetReached.Play();

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

    private void SpawnTargetRandomly()
    {
        gameManager.SpawnTargetRandomly();
    }

    private void LoadTruck()
    {
        for (int i = 0; i < cargoSpawnPoints.Length; i++)
        {
            GameObject newBox = Instantiate(cargoPrefab, cargoSpawnPoints[i].position, cargoSpawnPoints[i].rotation);
            newBox.transform.parent = cargoSpawnPoints[i];
            lastLoadedBoxes[i] = newBox;
        }

        remainingBoxes = cargoSpawnPoints.Length;
    }
}