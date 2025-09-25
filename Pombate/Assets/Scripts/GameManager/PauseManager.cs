using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseBackground;
    public GameObject pauseText;

    private bool isPaused = false;

    void Start()
    {
        pauseBackground.SetActive(false);
        pauseText.SetActive(false);
        Time.timeScale = 1f; 
        AudioListener.pause = false; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true; 
            pauseBackground.SetActive(true);
            pauseText.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            AudioListener.pause = false; 
            pauseBackground.SetActive(false);
            pauseText.SetActive(false);
        }
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}