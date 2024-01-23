using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform[] targetPoints; 
    public GameObject targetPrefab;
    public Text timer;
    public Text gameOverText;

    public float countdownTimer = 40.0f;

    void Start()
    {
        SpawnTargetRandomly();
    }

    private void Update()
    {
        if (countdownTimer > 0)
        {
            countdownTimer -= Time.deltaTime;
            timer.text = countdownTimer.ToString("F1");
        }
        else
        {
            countdownTimer = 0;
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
        gameOverText.gameObject.SetActive(true);
        Movement movement = FindObjectOfType<Movement>();
        movement.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        movement.enabled = false;
    }
}