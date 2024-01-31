using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Transform[] targetPoints; 
    public GameObject targetPrefab;
    public Text timer;
    public Text gameOverText;
    public Slider fuelSlider;
    public Slider nitrousSlider;
    public GameObject pausePanel;
    public GameObject helpPanel;

    public float countdownTimer = 40.0f;
    public Camera frontView;
    private bool isFrontView = false;
    public bool hasPaused;
    private CarController controller;
    void Start()
    {
        SpawnTargetRandomly();
        controller = FindObjectOfType<CarController>();
        frontView.gameObject.SetActive(false);
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }

        ChangeCamera();
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

    public void SetMaxFuelCapacity(float capacity)
    {
        fuelSlider.maxValue = capacity;
        fuelSlider.value = capacity;
    }

    public void SetFuelCapacity(float capacity)
    {
        fuelSlider.value = capacity;
    }
    
    public void SetMaxNitrousCapacity(float capacity)
    {
        nitrousSlider.maxValue = capacity;
        nitrousSlider.value = capacity;
    }

    public void SetNitrousCapacity(float capacity)
    {
        nitrousSlider.value = capacity;
    }

    public void PauseMenu()
    { 
        if (!hasPaused)
        {
            pausePanel.SetActive(true);
            FindObjectOfType<ButtonHandler>().ShowButtons();
            hasPaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            helpPanel.SetActive(false);
            pausePanel.SetActive(false);
            hasPaused = false;
            Time.timeScale = 1f;
        }       
    }  

    private void ChangeCamera()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isFrontView)
            {
                frontView.gameObject.SetActive(true);
                isFrontView = true;
            }
            else
            {
                frontView.gameObject.SetActive(false);
                isFrontView=false;
            }
        }

    }
}