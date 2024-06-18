using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneUIHandler : MonoBehaviour
{
    [SerializeField] GameObject pauseButton, resumeButton;
    private AudioSource menuSound;

    void Start()
    {
        menuSound = GetComponent<AudioSource>();
        menuSound.volume = GameManager.Instance.SoundEffectVolume;
    }

    public void BackToMenu()
    {
        menuSound.Play();
        // // Update HighScores
        // GameManager.Instance.AddHighScoreIfPossible(new HighScoreElement(GameManager.Instance.PlayerName, MainManager.m_Points));
        SceneManager.LoadScene(0);
    }

    public void PauseGame()
    {
        ///Pause game code
        menuSound.Play();
        Time.timeScale = 0.0f;
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);

    }

    public void ResumeGame()
    {
        //unpause game
        menuSound.Play();
        Time.timeScale = 1.0f;
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
    }
}
