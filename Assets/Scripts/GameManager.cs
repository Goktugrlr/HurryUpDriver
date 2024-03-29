using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform[] targetPoints; 
    public GameObject targetPrefab;

    public Text timer;
    public Text gameOverText;
    public Text pauseInfoText;
    public Slider fuelSlider;
    public Slider nitrousSlider;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    public GameObject helpPanel;

    public float countdownTimer = 150.0f;
    public bool hasPaused;
    public Camera frontView;
    public AudioListener audioListener;

    private bool isFrontView = false;
    private CarController controller;

    void Start()
    {
        SpawnTargetRandomly();
        controller = FindObjectOfType<CarController>();
        frontView.gameObject.SetActive(false);
    }

    void Update()
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

        HidePauseInfo();
    }
    public void SpawnTargetRandomly()
    {
        int randomIndex = Random.Range(0, targetPoints.Length);
        Instantiate(targetPrefab, targetPoints[randomIndex].position, Quaternion.identity);
    }

    void GameOver()
    {
        countdownTimer = 0;
        gameOverText.text = "You Lost\nTotal Delivered Cargo: " + FindObjectOfType<CargoManager>().deliveredCargoCount.ToString();
        gameOverPanel.SetActive(true);
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
            audioListener.enabled = false;
            pausePanel.SetActive(true);
            FindObjectOfType<ButtonHandler>().ShowButtons();
            hasPaused = true;
            Time.timeScale = 0f;

        }
        else
        {
            audioListener.enabled = true;
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

    private void HidePauseInfo()
    {
        if (Time.time > 10)
        {
            pauseInfoText.gameObject.SetActive(false);
        }
    }
}