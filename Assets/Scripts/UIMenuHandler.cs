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
    private GameObject inputField, acceptNameButton, playButton, playerButtons, settingsPanel, musicVolumeToggle, soundEffectsToggleGameobject;
    private Button acceptNameButtonComponent, playButtonComponent;
    private string inputText;
    private Toggle musicToggle, soundEffectsToggle;
    private AudioSource menuSound;
    [SerializeField]
    private AudioClip sampleClip;
    [SerializeField]
    private Slider musicVolumeSlider, soundEffectsVolumeSlider;
    [SerializeField]
    private GameObject attachedToMusicAudio;
    private AudioSource musicAudio;


    void Start()
    {
        // acceptNameButtonComponent = acceptNameButton.GetComponent<Button>();
        playButtonComponent = playButton.GetComponent<Button>();
        musicToggle = musicVolumeToggle.GetComponent<Toggle>();
        soundEffectsToggle = soundEffectsToggleGameobject.GetComponent<Toggle>();
        musicAudio = attachedToMusicAudio.GetComponent<AudioSource>();
        menuSound = GetComponent<AudioSource>();

        UpdatePlayerName();
        UpdateMusicToggle();
        UpdateMusicVolume();
        UpdateSoundEffectsToggle();
        UpdateSoundEffectsVolume();
        GameManager.Instance.GetHighScores();

    }

    void Update()
    {
        if (GetInput() == null || GetInput() == "")
        {
            playButtonComponent.interactable = false;
        }
        else
        {
            playButtonComponent.interactable = true;
        }
    }
        public void PlayButtonClicked()
    {
        GameManager.Instance.PlayerName = GetInput();
        // if (GameManager.Instance.PlayerName != null)
        // {
        PlayMenuSoundIfOn();
            
        // }

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


    #region MUSIC RELATED FUNTIONS

    private void SetMusicOn(bool boolValue)
    {
        musicToggle.isOn = boolValue;
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

    private void SetMusicVolume(float volume)
    {
        musicVolumeSlider.value = volume;
        musicAudio.volume = volume;
        GameManager.Instance.MusicVolume = volume;
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

    private void UpdateMusicVolume()
    {
        if (GameManager.Instance.MusicVolume != 0.0f)
        {
            SetMusicVolume(GameManager.Instance.MusicVolume);
        }
        else
        {
            SetMusicVolume(0.4f); // starting volume if not defined
        }
    }

    #endregion MUSIC RELATED FUNTIONS


    #region SOUND EFFECTS RELATED FUNCTIONS

    private void SetSoundEffectsOn(bool boolValue)
    {
        soundEffectsToggle.isOn = boolValue;
    }

        private void UpdateSoundEffectsToggle()
    {
        if (GameManager.Instance.SoundEffectsOn == true)
        {
            SetSoundEffectsOn(true);
        }
        else
        {
            SetSoundEffectsOn(false);
        }
    }

    private void SetSoundEffectVolume(float volume)
    {
        soundEffectsVolumeSlider.value = volume;
        GameManager.Instance.SoundEffectVolume = volume;
        // Set MenuSound here but
        // SoundEffectVolume to be used to set volume of sounds
        // on respective audiosources for
        // respective gameobjects on different scenes
        menuSound.volume = volume;

    }

    public void SoundEffectsOnChanged()
    {
        bool newBool = soundEffectsToggle.isOn;
        GameManager.Instance.SoundEffectsOn = newBool;
    }

    public void SoundEffectsVolumeChanged()
    {
        float newVolume = soundEffectsVolumeSlider.value;
        GameManager.Instance.SoundEffectVolume = newVolume;
        // need a preview of volume change so play sample sound
        if (GameManager.Instance.SoundEffectsOn) 
        {
            menuSound.PlayOneShot(sampleClip, newVolume);
        }
    }

    private void UpdateSoundEffectsVolume()
    {
        if (GameManager.Instance.SoundEffectVolume != 0.0f)
        {
            SetSoundEffectVolume(GameManager.Instance.SoundEffectVolume);
        }
        else
        {
            SetSoundEffectVolume(0.4f); // starting volume if not defined
        }
    }

    private void PlayMenuSoundIfOn()
    {
        if (GameManager.Instance.SoundEffectsOn)
        {
            menuSound.Play();
        }
    }


    #endregion SOUND EFFECTS RELATED FUNCTIONS



    // public void AcceptNameClicked()
    // {
    //     if (GameManager.Instance.PlayerName != null)
    //     {
    //         PlayMenuSoundIfOn();
    //         GameManager.Instance.PlayerName = GetInput();
    //         playButtonComponent.interactable = true;
    //     }
    // }

    public void SettingsButtonClicked()
    {
        PlayMenuSoundIfOn();
        playerButtons.SetActive(false);
        settingsPanel.SetActive(true);
    }


    public void SettingsCloseButtonClicked ()
    {
        PlayMenuSoundIfOn();
        playerButtons.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ExitButtonPressed()
    {
        PlayMenuSoundIfOn();
        GameManager.Instance.SavePlayerInfo();
        GameManager.Instance.AddHighScoreIfPossible(new HighScoreElement("Omni", Random.Range(0, 100)));
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
            // acceptNameButtonComponent.interactable = true;
            playButtonComponent.interactable = true;
        }
        else if (GameManager.Instance.GameRunning == true)
        {
            SetInput(GameManager.Instance.PlayerName);
            // acceptNameButtonComponent.interactable = true;
            playButtonComponent.interactable = true;
        }
        else
        {
            // acceptNameButtonComponent.interactable = false;
            playButtonComponent.interactable = false;
        }
    }



}
