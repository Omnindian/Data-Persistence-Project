using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

/* Attach to UI canvas */

public class UIMenuHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject inputField, acceptNameButton, playButton, playerButtons, settingsPanel, musicVolumeToggle;
    private Button acceptNameButtonComponent, playButtonComponent;
    private string inputText;
    private Toggle musicToggle;
    private AudioSource menuSound;    
    [SerializeField]
    private Slider musicVolumeSlider;
    [SerializeField]
    private GameObject attachedToMusicAudio;
    private AudioSource musicAudio;


    void Start()
    {
        acceptNameButtonComponent = acceptNameButton.GetComponent<Button>();
        playButtonComponent = playButton.GetComponent<Button>();
        musicToggle = musicVolumeToggle.GetComponent<Toggle>();
        musicAudio = attachedToMusicAudio.GetComponent<AudioSource>();
        menuSound = GetComponent<AudioSource>();

        UpdatePlayerName();
        UpdateMusicToggle();
        UpdateMusicVolume();

    }
    public void PlayButtonClicked()
    {
        menuSound.Play();
        GameManager.Instance.SavePlayerInfo(); // to transfer current settings to next scene
        SceneManager.LoadScene(1);
    }

    private string GetInput()
    {
        inputText = inputField.GetComponent<TMP_InputField>().text;
        return inputText;
    }

    private void SetInput(string name)
    {
        inputField.GetComponent<TMP_InputField>().text = name;
        
    }

    private void SetMusicOn(bool boolValue)
    {
        musicToggle.isOn = boolValue;
    }

    private void SetMusicVolume(float volume)
    {
        musicVolumeSlider.value = volume;
        musicAudio.volume = volume;
        GameManager.Instance.MusicVolume = volume;
    }

    public void AcceptNameClicked()
    {
        if (GameManager.Instance.PlayerName != null)
        {
            menuSound.Play();
            GameManager.Instance.PlayerName = GetInput();
            playButtonComponent.interactable = true;
        }
    }

    public void SettingsButtonClicked()
    {
        menuSound.Play();
        playerButtons.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void MusicOnChanged()
    {
        bool newBool = musicToggle.isOn;
        GameManager.Instance.MusicOn = newBool;
    }

    public void MusicVolumeChanged()
    {
        float newVolume = musicVolumeSlider.value;
        GameManager.Instance.MusicVolume = newVolume;
    }

    public void SettingsCloseButtonClicked ()
    {
        menuSound.Play();
        playerButtons.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ExitButtonPressed()
    {
        menuSound.Play();
        GameManager.Instance.SavePlayerInfo();
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    private void UpdatePlayerName()
    {
        if (GameManager.Instance.PlayerName != null)
        {
            SetInput(GameManager.Instance.PlayerName);
            acceptNameButtonComponent.interactable = true;
            playButtonComponent.interactable = true;
        }
        else if (GameManager.Instance.GameRunning == true)
        {
            SetInput(GameManager.Instance.PlayerName);
            acceptNameButtonComponent.interactable = true;
            playButtonComponent.interactable = true;
        }
        else
        {
            acceptNameButtonComponent.interactable = false;
            playButtonComponent.interactable = false;
        }
    }

    private void UpdateMusicToggle()
    {
        if (GameManager.Instance.MusicOn == true)
        {
            SetMusicOn(true);
        }
        else
        {
            SetMusicOn(false);
        }

    }

    private void UpdateMusicVolume()
    {
        if (GameManager.Instance.MusicVolume != 0.0f)
        {
            SetMusicVolume(GameManager.Instance.MusicVolume);
        }
        else
        {
            SetMusicVolume(0.4f);
        }
    }


}
