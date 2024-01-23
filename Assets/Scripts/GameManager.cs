using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform[] targetPoints; 
    public GameObject targetPrefab;    

    void Start()
    {
        SpawnTargetRandomly();
    }

    public void SpawnTargetRandomly()
    {
        int randomIndex = Random.Range(0, targetPoints.Length);
        Instantiate(targetPrefab, targetPoints[randomIndex].position, Quaternion.identity);
    }
}