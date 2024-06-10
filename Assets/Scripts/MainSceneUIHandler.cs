using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneUIHandler : MonoBehaviour
{
    [SerializeField] GameObject pauseButton, resumeButton;
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        ///Pause game code
        Time.timeScale = 0.0f;
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);

    }

    public void ResumeGame()
    {
        //unpause game
        Time.timeScale = 1.0f;
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
    }
}
