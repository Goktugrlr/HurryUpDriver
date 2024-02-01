using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public GameObject[] buttonBorders;  
    private GameManager gameManager;

    void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameManager.hasPaused = true;
        gameManager.PauseMenu();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1f;
    }

    public void Help()
    {
        gameManager.helpPanel.gameObject.SetActive(true);
        HideButtons();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void OKButton()
    {
        gameManager.helpPanel.gameObject.SetActive(false);
        ShowButtons();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void HideButtons()
    {
        foreach (GameObject button in buttonBorders)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void ShowButtons()
    {
        foreach (GameObject button in buttonBorders)
        {
            button.gameObject.SetActive(true);
        }
    }
}