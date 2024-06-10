using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIMenuHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject inputField, acceptNameButton, playButton;
    private Button acceptNameButtonComponent, playButtonComponent;
    private string inputText;

    void Start()
    {
        acceptNameButtonComponent = acceptNameButton.GetComponent<Button>();
        playButtonComponent = playButton.GetComponent<Button>();

        if (GameManager.Instance.GameRunning == true)
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
    public void PlayButtonClicked()
    {
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

    public void AcceptNameClicked()
    {
        if (GameManager.Instance.PlayerName != null)
        {
            GameManager.Instance.PlayerName = GetInput();
            playButtonComponent.interactable = true;
        }
    }

    void Update()
    {
        // string inputText = inputField.GetComponent<TMP_InputField>().text;
        // if (inputText == null)
        // {
        //     acceptNameButtonComponent.interactable = true;
        // }
    }

    // public void Exit()
    // {
    //     MainManager.Instance.SaveColor();
    //     #if UNITY_EDITOR
    //         EditorApplication.ExitPlaymode();
    //     #else
    //         Application.Quit();
    //     #endif
    // }

    // public void SaveColorClicked()
    // {
    //     MainManager.Instance.SaveColor();
    // }

    // public void LoadColorClicked()
    // {
    //     MainManager.Instance.LoadColor();
    //     ColorPicker.SelectColor(MainManager.Instance.TeamColor);
    // }
}
