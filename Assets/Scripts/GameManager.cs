using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform[] targetPoints; 
    public GameObject targetPrefab;
    public Text timer;
    public Text gameOverText;
    public Slider fuelSlider;
    public float countdownTimer = 40.0f;

    private CarController controller;
    void Start()
    {
        SpawnTargetRandomly();
        controller = FindObjectOfType<CarController>();
    }

    private void Update()
    {
        if (countdownTimer > 0)
        {
            countdownTimer -= Time.deltaTime;
            timer.text = countdownTimer.ToString("F1");
            if (!controller.CheckFuel() && controller.GetComponent<Rigidbody>().velocity.magnitude <= 1)
            {
                GameOver();
            }
        }
        else
        {
            GameOver();
        }
        
    }
    public void SpawnTargetRandomly()
    {
        int randomIndex = Random.Range(0, targetPoints.Length);
        Instantiate(targetPrefab, targetPoints[randomIndex].position, Quaternion.identity);
    }

    void GameOver()
    {
        countdownTimer = 0;
        gameOverText.gameObject.SetActive(true);
        controller.GetComponent<Rigidbody>().velocity = Vector3.zero;
        controller.enabled = false;
    }

    public void SetMaxCapacity(float capacity)
    {
        fuelSlider.maxValue = capacity;
        fuelSlider.value = capacity;
    }

    public void SetCapacity(float capacity)
    {
        fuelSlider.value = capacity;
    }
}